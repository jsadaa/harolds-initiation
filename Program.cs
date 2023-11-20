using System.Reflection;
using HaroldsInitiation;

// Variables
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
char[] floorMaterials = { '@','#','$','%','&','?','!','/','\\','|','(',')','[',']','{','}' };
var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var audioPath = Path.Combine(assemblyPath!, "..", "..", "..", "Assets", "Sounds") + "/";
const byte volume = 50;

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
            Events.PlayerGoBackward(player: player);
            break;
        case ConsoleKey.RightArrow:
            Events.PlayerGoForward(player: player);
            break;
        default:
            game.ShouldExit = true;
            game.GameOverMessage = "Invalid input. Program exiting.";
            break;
    }
    
    // Check if player is at gem
    if (player.IsAt(gem.CurrentPosition()[0]) && !player.IsHigh)
    {
        Events.GotGemAudioTimer(audioPlayer: audioPlayer, volume: volume);
        
        Layout.Show(floor: floorMaterials[new Random().Next(0, floorMaterials.Length)]);
        
        game.Score.Add(1);
        Layout.Show(score: game.Score);
        
        Events.PlayerIsHighTimer(player: player, gem: gem);
    }
}

// Game over
Layout.GameOver(message: game.GameOverMessage);