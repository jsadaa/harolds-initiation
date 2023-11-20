using System.Reflection;
using HaroldsInitiation;

// Variables
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
char[] floorMaterials = { '@','#','$','%','&','?','!','/','\\','|','(',')','[',']','{','}' };
var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var audioPath = Path.Combine(assemblyPath!, "..", "..", "..", "Assets", "Sounds") + "/";
const byte volume = 20;

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
audioPlayer.PlayAsync("s6.mp3", volume);
Layout.Menu();
audioPlayer.Stop();

// Game
Layout.Initialize();
Layout.Show(title: Game.Title);
Layout.Show(score: game.Score);
Layout.Show(floor: floorMaterials[new Random().Next(0, floorMaterials.Length)]);

while (player.IsAt(gem.CurrentPosition()[0]))
{
    gem.Randomize();
}

Layout.Show(player: player);
Layout.Show(gem: gem);

/*************
 * Game Loop *
 * ***********/

while (!game.ShouldExit)
{
    // Check if the console was resized
    if (Layout.TerminalResized(initHeight, initWidth))
    {
        game.ShouldExit = true;
        game.GameOverMessage = "Console was resized. Program exiting.";
        break;
    }
    
    // Move player
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.LeftArrow:
            Layout.Erase(player: player);
            player.Backward();
            Layout.Show(player: player);
            break;
        case ConsoleKey.RightArrow:
            Layout.Erase(player: player);
            player.Forward();
            Layout.Show(player: player);
            break;
        default:
            game.ShouldExit = true;
            game.GameOverMessage = "Invalid input. Program exiting.";
            break;
    }
    
    // Check if player is at gem
    if (player.IsAt(gem.CurrentPosition()[0]) && !player.IsHigh)
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

        while (player.IsAt(gem.CurrentPosition()[0]))
        {
            gem.Randomize();
        }
        
        // Update score
        Layout.Show(floor: floorMaterials[new Random().Next(0, floorMaterials.Length)]);
        Layout.Show(score: game.Score);
        Layout.Show(player: player);
            
        // Launch async events
        AsyncEvents.GotGemSound(audioPlayer: audioPlayer, volume: volume, fileName: sound);
        AsyncEvents.PlayerGetsBackNormal(player: player, gem: gem);
    }
}

// Game over
audioPlayer.Stop();
Layout.GameOver(message: game.GameOverMessage);