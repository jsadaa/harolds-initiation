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

// Resources
var resources = new ResourceManager("HaroldsInitiation.Resources.Resources", Assembly.GetExecutingAssembly());

// params
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
byte volume = 20;

// Instances of game objects
var game = new Game();
var audioPlayer = new AudioPlayer(soundDir);


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
var difficultyLoad = game.GetDifficultyLoad();

// Game objects entities
var floor = new Floor();
var player = new Player();
var gem1 = new Gem(difficultyLoad);
var gem2 = new Gem(difficultyLoad);

// Display game
audioPlayer.Stop();
floor.Randomize();
Layout.Clear();
Layout.Show(game.Title);
Layout.Show(game.Score);
Layout.Show(game.Level);
Layout.Show(floor);
Layout.Show(player);

// Randomize gem position (until it's not at player)
while (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
{
    gem1.Randomize();
    while (gem1.IsAt(gem2.CurrentPosition()[0])) gem2.Randomize();
}

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

    // Actions based on user input
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
            // if async events were active, it means we don't have to display the gem
            // as it will be updated in async event
            if (AsyncEvents.HasActiveEvents()) Layout.Resume(player, game.Score, floor, game.Level, game.Title);
            else Layout.Resume(player, game.Score, new[] { gem1, gem2 }, floor, game.Level, game.Title);
            AsyncEvents.ResumeAll();

            break;
        default:
            game.ShouldExit = true;
            game.EndMessage = resources.GetString("ErrorInvalidInput")!;
            break;
    }

    // Check if player is at gem
    if (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
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
            if (currentGem.IsCursed())
            {
                game.Score.Subtract(1);
                player.GetsCursed();
                sound = resources.GetString("SoundCursed")!;
            }
            else
            {
                game.Score.Add(1);
                player.GetsHigher();
                sound = resources.GetString("SoundHigher")!;
            }

            // Erase all gems
            Layout.Erase(gem1);
            Layout.Erase(gem2);

            // Update floor
            floor.Randomize();

            // Display (not gem as it will be updated in async event)
            Layout.Show(floor);
            Layout.Show(game.Score);
            Layout.Show(player);

            // Play sound
            audioPlayer.PlayAsync(sound, volume);
            
            // Launch async events
            AsyncEvents.EndTranceSoundEvent(audioPlayer, volume,resources.GetString("SoundDigest")!);
            AsyncEvents.CreatePlayerGetsBackNormalEvent(player, new[] { gem1, gem2 });
        }
    }

    // Check if game is won or lost
    if (game.Score.Get() >= 2)
    {
        game.IsWon = true;
        game.ShouldExit = true;
        game.EndMessage = resources.GetString("WonGame")!;
    }
    else if (game.Score.Get() <= -2)
    {
        game.ShouldExit = true;
        game.EndMessage = resources.GetString("GameOver")!;
    }
}

// Game end
Layout.Clear();
AsyncEvents.CancelAll();

audioPlayer.Stop();
audioPlayer.PlayAsync(resources.GetString("SoundIntro")!, volume);

if (game.IsWon) Layout.Win(game.EndMessage);
else Layout.GameOver(game.EndMessage);

audioPlayer.Stop();
Layout.Clear();

return 0;