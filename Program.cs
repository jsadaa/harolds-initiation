using System.Reflection;
using System.Resources;
using HaroldsInitiation.Audio;
using HaroldsInitiation.Entities;
using HaroldsInitiation.Events;
using HaroldsInitiation.Game;
using HaroldsInitiation.UI;

// Paths
var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var audioPath = Path.Combine(assemblyPath!, "..", "..", "..", "Assets", "Sounds") + "/";

// Resources
var resources = new ResourceManager("HaroldsInitiation.Resources.Resources", Assembly.GetExecutingAssembly());
char[] floorMaterials = { '@', '#', '$', '%', '&', '?', '!', '/', '\\', '|', '(', ')', '[', ']', '{', '}' };

// params
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
byte volume = 20;

// Instances of game objects
var game = new Game();
var audioPlayer = new AudioPlayer(audioPath);

// Game objects entities
var player = new Player();
var gem = new Gem();

/*************
 * Game Init *
 * ***********/

// Menu
Layout.Clear();
audioPlayer.PlayAsync("s6.mp3", volume);
Layout.Menu();
audioPlayer.Stop();

// Game
Layout.Clear();
Layout.Show(Game.Title);
Layout.Show(game.Score);
Layout.Show(floorMaterials[new Random().Next(0, floorMaterials.Length)]);

// Randomize gem position (until it's not at player)
while (player.IsAt(gem.CurrentPosition()[0])) gem.Randomize();

Layout.Show(player);
Layout.Show(gem);

/*************
 * Game Loop *
 * ***********/

while (!game.ShouldExit)
{
    // Check if the console was resized and exit if so
    if (Layout.TerminalHasResized(initHeight, initWidth))
    {
        game.ShouldExit = true;
        game.GameOverMessage = resources.GetString("ErrorTerminalResized")!;
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
        case ConsoleKey.O:
            // Options
            AsyncEvents.PauseAll();
            Layout.Clear();
            volume = Layout.VolumeOption();
            Layout.Clear();
            // if async events were active, it means we don't have to display the gem
            // as it will be updated in async event
            if (AsyncEvents.HasActiveEvents())
                Layout.Resume(player, game.Score, floorMaterials[new Random().Next(0, floorMaterials.Length)],
                    Game.Title);
            else
                Layout.Resume(player, game.Score, gem, floorMaterials[new Random().Next(0, floorMaterials.Length)],
                    Game.Title);
            AsyncEvents.ResumeAll();
            break;
        default:
            game.ShouldExit = true;
            game.GameOverMessage = resources.GetString("ErrorInvalidInput")!;
            break;
    }

    // Check if player is at gem and not higher (higher player can't get gems)
    if (player.IsAt(gem.CurrentPosition()[0]) && !player.IsHigher)
    {
        string sound;

        // Check if gem is cursed or not and update score
        if (gem.IsCursed())
        {
            game.Score.Subtract(1);
            player.GetsCursed();
            sound = "s5.mp3";
        }
        else
        {
            game.Score.Add(1);
            player.GetsHigher();
            sound = "s9.mp3";
        }

        // Update gem position (randomize until it's not at player)
        while (player.IsAt(gem.CurrentPosition()[0])) gem.Randomize();

        // display (not gem as it will be updated in async event)
        Layout.Show(floorMaterials[new Random().Next(0, floorMaterials.Length)]);
        Layout.Show(game.Score);
        Layout.Show(player);

        // Launch async events
        AsyncEvents.CreateGotGemSoundEvent(audioPlayer, volume, sound);
        AsyncEvents.CreatePlayerGetsBackNormalEvent(player, gem);
    }
}

// Game over
audioPlayer.Stop();
Layout.Clear();
Layout.GameOver(game.GameOverMessage);
return 0;