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
var gem1 = new Gem();
var gem2 = new Gem();

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
while (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
{
    gem1.Randomize();
    while (gem1.IsAt(gem2.CurrentPosition()[0])) gem2.Randomize();
}

Layout.Show(player);
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
                Layout.Resume(player, game.Score, new[] {gem1, gem2}, floorMaterials[new Random().Next(0, floorMaterials.Length)],
                    Game.Title);
            AsyncEvents.ResumeAll();
            break;
        default:
            game.ShouldExit = true;
            game.GameOverMessage = resources.GetString("ErrorInvalidInput")!;
            break;
    }

    // Check if player is at gem and not higher (higher player can't get gems)
    if (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
    {
        var currentGem = player.IsAt(gem1.CurrentPosition()[0]) ? gem1 : gem2;
        if (player is { IsHigher: false, IsCrouching: false })
        {
            string sound;

            // Check if gem is cursed or not and update score
            if (currentGem.IsCursed())
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
            
            // erase all gems
            Layout.Erase(gem1);
            Layout.Erase(gem2);

            currentGem.Randomize();
            // Update gem position (randomize until it's not at player)
            while (player.IsAt(gem1.CurrentPosition()[0]) || player.IsAt(gem2.CurrentPosition()[0]))
            {
                gem1.Randomize();
                while (gem2.IsAt(gem1.CurrentPosition()[0])) gem2.Randomize();
            }
            
            // display (not gem as it will be updated in async event)
            Layout.Show(floorMaterials[new Random().Next(0, floorMaterials.Length)]);
            Layout.Show(game.Score);
            Layout.Show(player);

            // Launch async events
            AsyncEvents.CreateGotGemSoundEvent(audioPlayer, volume, sound);
            AsyncEvents.CreatePlayerGetsBackNormalEvent(player, new[] { gem1, gem2 });
        }
    }
}

// Game over
audioPlayer.Stop();
Layout.Clear();
Layout.GameOver(game.GameOverMessage);
return 0;