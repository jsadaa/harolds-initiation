namespace HaroldsInitiation;

public static class AsyncEvents
{
    public static void GotGemSound(AudioPlayer audioPlayer, byte volume, string fileName)
    {
        audioPlayer.PlayAsync(fileName, volume);
        var gotGemSoundTimer = new System.Timers.Timer(4000);
        gotGemSoundTimer.Elapsed += (sender, args) =>
        {
            audioPlayer.Stop();
            audioPlayer.PlayAsync("s12.mp3", volume);
        };
        gotGemSoundTimer.AutoReset = false;
        gotGemSoundTimer.Start();
    }
    
    public static void PlayerGetsBackNormal(Player player, Gem gem)
    {
        var playerGetsBackNormalTimer = new System.Timers.Timer(4000);
        
        playerGetsBackNormalTimer.Elapsed += (sender, args) =>
        {
            player.GetsNormal();
            Layout.Show(player: player);
            while (player.IsAt(gem.CurrentPosition()[0]))
            {
                gem.Randomize();
            }
            Layout.Show(gem: gem);
        };
        playerGetsBackNormalTimer.AutoReset = false;
        playerGetsBackNormalTimer.Start();
    }
}