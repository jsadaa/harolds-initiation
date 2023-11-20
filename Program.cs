using System.Reflection;
using HaroldsInitiation;

// Variables
var initHeight = Console.WindowHeight;
var initWidth = Console.WindowWidth;
char[] floorMaterials = { '@','#','$','%','&','?','!','/','\\','|','(',')','[',']','{','}','\'' };
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
        // Play audio
        Events.GotGemAudioTimer_Elapsed(audioPlayer: audioPlayer, volume: volume);
        
        // Randomize floor
        Layout.Show(floor: floorMaterials[new Random().Next(0, floorMaterials.Length)]);
        
        // Update score
        game.Score.Add(1);
        Layout.Show(score: game.Score);
        
        // Update player
        player.GetsHigh();
        Layout.Show(player: player);
        
        // Set timer to sober up player and regenerate gem
        Events.PlayerIsHighTimer_Elapsed(player: player, gem: gem);
    }
}

// Game over
Layout.GameOver(message: game.GameOverMessage);