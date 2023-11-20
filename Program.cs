using System.Reflection;
using HaroldsInitiation;

// Variables
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
char[] floorMaterials = { '@', '#', '$', '%', '&', '?', '!', '/', '\\', '|', '(', ')', '[', ']', '{', '}' };
var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var audioPath = Path.Combine(assemblyPath!, "..", "..", "..", "Assets", "Sounds") + "/";
byte volume = 20;

// Instances
var game = new Game();
var audioPlayer = new AudioPlayer(audioPath);

// Game objects
var player = new Player();
var gem = new Gem();

/*************
 * Game Init *
 * ***********/

// Menu
Layout.Initialize();
audioPlayer.PlayAsync("s6.mp3", volume);
Layout.Menu();
audioPlayer.Stop();

// Game
Layout.Initialize();
Layout.Show(Game.Title);
Layout.Show(game.Score);
Layout.Show(floorMaterials[new Random().Next(0, floorMaterials.Length)]);

while (player.IsAt(gem.CurrentPosition()[0])) gem.Randomize();

Layout.Show(player);
Layout.Show(gem);

/*************
 * Game Loop *
 * ***********/

while (!game.ShouldExit)
{
    // Check if the console was resized
    if (Layout.TerminalHasResized(initHeight, initWidth))
    {
        game.ShouldExit = true;
        game.GameOverMessage = "Console was resized. Program exiting.";
        break;
    }

    // Move player
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
            AsyncEvents.PauseAll();
            volume = Layout.VolumeOption();
            Layout.Initialize();
            if (AsyncEvents.HasActiveEvents())
                Layout.ResumeShow(
                    player,
                    game.Score,
                    floorMaterials[new Random().Next(0, floorMaterials.Length)],
                    Game.Title
                );
            else
                Layout.ResumeShow(
                    player,
                    game.Score,
                    gem,
                    floorMaterials[new Random().Next(0, floorMaterials.Length)],
                    Game.Title
                );
            AsyncEvents.ResumeAll();
            break;
        default:
            game.ShouldExit = true;
            game.GameOverMessage = "Invalid input. Program exiting.";
            break;
    }

    // Check if player is at gem
    if (player.IsAt(gem.CurrentPosition()[0]) && !player.IsHigher)
    {
        string sound;
        
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

        while (player.IsAt(gem.CurrentPosition()[0])) gem.Randomize();

        // Update view (not gem as it will be updated in async event)
        Layout.Show(floorMaterials[new Random().Next(0, floorMaterials.Length)]);
        Layout.Show(game.Score);
        Layout.Show(player);

        // Launch async events
        AsyncEvents.GotGemSound(audioPlayer, volume, sound);
        AsyncEvents.PlayerGetsBackNormal(player, gem);
    }
}

// Game over
audioPlayer.Stop();
Layout.GameOver(game.GameOverMessage);
return 0;