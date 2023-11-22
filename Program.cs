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
var volume = 0.2f;

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
Layout.ShowLevelOptions();
var levelInput = Console.ReadLine();
var level = int.TryParse(levelInput, out var levelInt) ? Math.Clamp(levelInt, 1, 3) : 1;
game.SetLevel(level);
Layout.Clear();

// Set Volume
Layout.ShowVolumeOptions();
var volumeInput = Console.ReadLine();
volume = float.TryParse(volumeInput, out var volumeInt) ? Math.Clamp(volumeInt, 0, 100) / 100f : 0.7f;
Layout.Clear();

// Set the first riddle
var riddle = game.GetRiddle();
if (riddle is null) return 0;

// Game objects entities
var floor = new Floor();
var player = new Player();
var gem1 = new Gem(riddle.GoodSide());
var gem2 = new Gem(riddle.BadSide());

// Display game Layout
audioPlayer.Stop();
floor.Randomize();
Layout.ShowTitle(Game.Title);
Layout.ShowScore(game.Score);
Layout.ShowLevel(game.Level);
Layout.ShowRiddle(riddle);
Layout.ShowFloor(floor);

// Display player
Layout.ShowPlayer(player);

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
Layout.ShowGem(gem1);
Layout.ShowGem(gem2);

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

            Layout.ErasePlayer(player);
            player.Backward();
            Layout.ShowPlayer(player);

            break;
        case ConsoleKey.RightArrow:

            Layout.ErasePlayer(player);
            player.Forward();
            Layout.ShowPlayer(player);

            break;
        case ConsoleKey.DownArrow:

            Layout.ErasePlayer(player);
            player.Crouch();
            Layout.ShowPlayer(player);

            break;
        case ConsoleKey.UpArrow:

            Layout.ErasePlayer(player);
            player.Stand();
            Layout.ShowPlayer(player);

            break;
        case ConsoleKey.O:
            // Pause game
            Layout.Clear();

            // Options
            Layout.ShowVolumeOptions();
            volumeInput = Console.ReadLine();
            volume = float.TryParse(volumeInput, out var volumeIntO)
                ? Math.Clamp(volumeIntO, 0, 100) / 100f
                : 0.7f;
            Layout.Clear();

            // Resume game
            Layout.Resume(player, game.Score, new[] { gem1, gem2 }, floor, riddle, game.Level, Game.Title);
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
            // Mark riddle as used
            game.MarkeRiddleAsUsed(riddle);

            // Update riddle
            var newRiddle = game.GetRiddle();

            // Check if there are no more riddles
            if (newRiddle is null)
            {
                game.ShouldExit = true;
            }
            else
            {
                string sound;

                // Check if gem is cursed or not
                // update score
                // update player
                // set sound
                if (riddle.GoodSide() == currentGem.GetSide())
                {
                    game.Score.Add(1);
                    game.TurnIsWon = true;
                    player.GetsHigher();
                    sound = resources.GetString("SoundHigher")!;
                }
                else
                {
                    game.Score.Subtract(1);
                    game.TurnIsWon = false;
                    player.GetsCursed();
                    sound = resources.GetString("SoundCursed")!;
                }

                // Erase riddle
                Layout.EraseRiddle(riddle);

                // Erase all gems
                Layout.EraseGem(gem1);
                Layout.EraseGem(gem2);

                // Update gems
                gem1.NewState(newRiddle.GoodSide());
                gem2.NewState(newRiddle.BadSide());

                // Update floor
                floor.Randomize();

                // Display (not gem as it will be updated in async event)
                Layout.ShowFloor(floor);
                Layout.ShowScore(game.Score);
                Layout.ShowPlayer(player);

                // Play Result sound
                audioPlayer.PlayAsync(sound, volume);
                
                // Set Iteration message result
                var message = game.TurnIsWon ? resources.GetString("TurnWon")! : resources.GetString("TurnLost")!;

                // Launch async events
                AsyncEvents.EndTranceSoundEvent(audioPlayer, volume, resources.GetString("SoundDigest")!);
                AsyncEvents.PlayerGetsBackNormalEvent(player, new[] { gem1, gem2 }, newRiddle, message);

                // Assign new riddle
                riddle = newRiddle;
                
                // Block until all async events are done to go to next iteration and display result message
                Layout.ShowResultMessage(message, game.TurnIsWon ? ConsoleColor.Yellow : ConsoleColor.DarkGray);
                while (AsyncEvents.HasActiveEvents()) Console.ReadKey(true);
            }
        }
    }
}

/************
 * Game End *
 * ***********/

// Check if player won or lost
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

// Clear console and cancel async events
Layout.Clear();
AsyncEvents.CancelAll();

// Stop sound and play end sound
audioPlayer.PlayAsync(resources.GetString("SoundIntro")!, volume);

// Display end message
if (game.IsWon) Layout.Win(game.EndMessage);
else Layout.GameOver(game.EndMessage);

// Stop sound and clear console
audioPlayer.Stop();
Layout.Clear();

return 0;