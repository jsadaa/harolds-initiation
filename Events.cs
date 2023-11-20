namespace HaroldsInitiation;

public static class Events
{
    public static void GotGemAudioTimer_Elapsed(AudioPlayer audioPlayer, byte volume)
    {
        var gotGemAudioTimer = new System.Timers.Timer(1);
        gotGemAudioTimer.Elapsed += (sender, args) =>
        {
            audioPlayer.PlayAsync("s12.mp3", volume);
        };
        gotGemAudioTimer.AutoReset = false;
        gotGemAudioTimer.Start();
    }
    
    public static void PlayerIsHighTimer_Elapsed(Player player, Gem gem)
    {
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
}