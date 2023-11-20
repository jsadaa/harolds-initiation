namespace HaroldsInitiation;

public static class Events
{
    public static void GotGemAudioTimer(AudioPlayer audioPlayer, byte volume)
    {
        var gotGemAudioTimer = new System.Timers.Timer(1);
        gotGemAudioTimer.Elapsed += (sender, args) =>
        {
            audioPlayer.PlayAsync("s12.mp3", volume);
        };
        gotGemAudioTimer.AutoReset = false;
        gotGemAudioTimer.Start();
    }
    
    public static void PlayerIsHighTimer(Player player, Gem gem)
    {
        player.GetsHigh();
        Layout.Show(player: player);
        
        var playerIsHighTimer = new System.Timers.Timer(4000);
        playerIsHighTimer.Elapsed += (sender, args) =>
        {
            player.GetsSober();
            Layout.Show(player: player);
            while (player.IsAt(gem.CurrentPosition()[0]))
            {
                gem.Randomize();
            }
            Layout.Show(gem: gem);
        };
        playerIsHighTimer.AutoReset = false;
        playerIsHighTimer.Start();
    }
    
    public static void PlayerGoForward(Player player)
    {
        Layout.Erase(player: player);
        player.Forward();
        Layout.Show(player: player);
    }
    
    public static void PlayerGoBackward(Player player)
    {
        Layout.Erase(player: player);
        player.Backward();
        Layout.Show(player: player);
    }
}