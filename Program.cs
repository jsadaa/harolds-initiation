using System.Reflection;
using System.Resources;
using HaroldsInitiation.Audio;
using HaroldsInitiation.Entities;
using HaroldsInitiation.Events;
using HaroldsInitiation.Game;
using HaroldsInitiation.UI;

// Paths
var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var soundDir = Path.Combine(assemblyPath!, "..", "..", "..", "Assets", "Sounds") + "/";
var riddlesDir = Path.Combine(assemblyPath!, "..", "..", "..", "Resources", "Riddles.json");

// Resources
var resources = new ResourceManager("HaroldsInitiation.Resources.Resources", Assembly.GetExecutingAssembly());

// params
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
byte volume = 20;

// Instances of game objects
var game = new Game();
var audioPlayer = new AudioPlayer(soundDir);

// Load riddles
game.LoadRiddles(riddlesDir);

/*************
 * Game Init *
 * ***********/

// Menu
Layout.Clear();
audioPlayer.PlayAsync(resources.GetString("SoundIntro")!, volume);
Layout.Menu();
Layout.Clear();

// Set level
game.SetLevel(Layout.SetLevel());
var riddle = game.GetRiddle();

// Game objects entities
var floor = new Floor();
var player = new Player();
var gem1 = new Gem(riddle.GoodSide());
var gem2 = new Gem(riddle.BadSide());

// Display game Layout
audioPlayer.Stop();
floor.Randomize();
Layout.Clear();
Layout.Show(Game.Title);
Layout.Show(game.Score);
Layout.Show(game.Level);
Layout.Show(riddle);
Layout.Show(floor);

// Display player
Layout.Show(player);

// Randomize gem position
gem1.Randomize(Console.WindowWidth);
gem2.Randomize(Console.WindowWidth);

// Check if gems are at the same position as player or each other
while (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
{
    gem1.Randomize(Console.WindowWidth);
    while (gem1.IsAt(gem2.CurrentPosition()[0])) gem2.Randomize(Console.WindowWidth);
}

// Display gems
Layout.Show(gem1);
Layout.Show(gem2);

/*************
 * Game Loop *
 * ***********/

while (!game.ShouldExit)
{
    // Check if the console was resized and exit if so
    if (Layout.TerminalHasResized(initHeight, initWidth))
    {
        game.ShouldExit = true;
        game.EndMessage = resources.GetString("ErrorTerminalResized")!;
        break;
    }

    // Actions
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.LeftArrow:

            Layout.Erase(player);
            player.Backward();
            Layout.Show(player);

            break;
        case ConsoleKey.RightArrow:

            Layout.Erase(player);
            player.Forward();
            Layout.Show(player);

            break;
        case ConsoleKey.DownArrow:

            Layout.Erase(player);
            player.Crouch();
            Layout.Show(player);

            break;
        case ConsoleKey.UpArrow:

            Layout.Erase(player);
            player.Stand();
            Layout.Show(player);

            break;
        case ConsoleKey.O:
            // Pause game
            AsyncEvents.PauseAll();
            Layout.Clear();

            // Options
            volume = Layout.SetVolume();
            Layout.Clear();

            // Resume game
            // if async events were active, it means we don't have to display the gem and riddle
            // as it will be updated in async event
            if (AsyncEvents.HasActiveEvents()) Layout.Resume(player, game.Score, floor, game.Level, Game.Title);
            else Layout.Resume(player, game.Score, new[] { gem1, gem2 }, floor, riddle, game.Level, Game.Title);
            AsyncEvents.ResumeAll();

            break;
        default:
            //game.ShouldExit = true;
            //game.EndMessage = resources.GetString("ErrorInvalidInput")!;
            break;
    }

    // Check if player is at gem
    if (player.IsAt(gem1.CurrentPosition()[0]) || (player.IsAt(gem2.CurrentPosition()[0]) && !game.HasNoMoreRiddles()))
    {
        var currentGem = player.IsAt(gem1.CurrentPosition()[0]) ? gem1 : gem2;

        // Check if player is higher, cursed or crouching
        // if it is, player can't get gem
        if (player is { IsHigher: false, IsCursed: false, IsCrouching: false })
        {
            string sound;

            // Check if gem is cursed or not
            // update score
            // update player
            // set sound
            if (riddle.GoodSide() == currentGem.GetSide())
            {
                game.Score.Add(1);
                player.GetsHigher();
                sound = resources.GetString("SoundHigher")!;
            }
            else
            {
                game.Score.Subtract(1);
                player.GetsCursed();
                sound = resources.GetString("SoundCursed")!;
            }

            // Erase riddle
            Layout.Erase(riddle);

            // Update riddle
            var newRiddle = game.GetRiddle();

            // Erase all gems
            Layout.Erase(gem1);
            Layout.Erase(gem2);

            // Update gems
            gem1.NewState(newRiddle.GoodSide());
            gem2.NewState(newRiddle.BadSide());

            // Update floor
            floor.Randomize();

            // Display (not gem as it will be updated in async event)
            Layout.Show(floor);
            Layout.Show(game.Score);
            Layout.Show(player);

            // Play Result sound
            audioPlayer.PlayAsync(sound, volume);

            // Update game
            game.MarkeRiddleAsUsed(riddle); ;

            // Launch async events
            AsyncEvents.EndTranceSoundEvent(audioPlayer, volume, resources.GetString("SoundDigest")!);
            AsyncEvents.PlayerGetsBackNormalEvent(player, new[] { gem1, gem2 }, newRiddle);
            
            riddle = newRiddle;
        }
    }

    // Check if there are no more riddles then check if player won or lost
    if (game.HasNoMoreRiddles())
    {
        if (game.Score.Get() > 0)
        {
            game.IsWon = true;
            game.ShouldExit = true;
            game.EndMessage = resources.GetString("WonGame")!;
        }
        else if (game.Score.Get() <= 0)
        {
            game.ShouldExit = true;
            game.EndMessage = resources.GetString("GameOver")!;
        }
    }
}

/************
  * Game End *
   * ***********/

// Clear console and cancel async events
Layout.Clear();
AsyncEvents.CancelAll();

// Stop sound and play end sound
audioPlayer.Stop();
audioPlayer.PlayAsync(resources.GetString("SoundIntro")!, volume);

// Display end message
if (game.IsWon) Layout.Win(game.EndMessage);
else Layout.GameOver(game.EndMessage);

// Stop sound and clear console
audioPlayer.Stop();
Layout.Clear();

return 0;