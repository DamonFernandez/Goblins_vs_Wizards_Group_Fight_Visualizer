global using static Program; // Makes static fields on Program available globally (for `RNG`).


using COIS2020.DamonFernandez0813575.Assignments;


class Program
{
    /// <summary>
    /// The random number generator used for all RNG in the program.
    /// </summary>
    public static Random RNG = new(/* Seed here */);


    private static void Main()
    {
        // The wizard/goblin ToString methods use emojis. You need this line if you want to Console.WriteLine them.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Feel free to fiddle with the parameters here as you test.
        using var game = new WizardFighterDX()
        {
            TickDelayMS = 250,  // 250 = 250ms per frame. Change to a lower/higher value for faster/slower playback.
            IsPlaying = true,   // Set to true/false to determine if playback should start playing or paused.
        };

        game.Run();
    }
}
