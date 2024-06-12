namespace COIS2020.DamonFernandez0813575.Assignments;

using Microsoft.Xna.Framework; // Needed for `Vector2`
using TrentCOIS.Tools.Visualization.EntityViz;


public enum GoblinClan
{
    Cragmaw,
    BogBrotherhood,
    Boko,
}

public class Goblin : CombatEntity, IComparable<Goblin>, IEquatable<Goblin>
{
    private static readonly string[] names; // Names get filled from txt file automatically


    // Instance fields
    // --------------------------------

    public GoblinClan Clan { get; set; }
    public int AttackPower { get; set; }


    // Constructors
    // --------------------------------

    #region Constructors

    /// <summary>
    /// Creates a new goblin with a pre-seeded random number generator and a random name.
    /// </summary>
    public Goblin() : this(names[RNG.Next(names.Length)])
    { }

    /// <summary>
    /// Creates a new goblin with a pre-seeded random number generator.
    /// </summary>
    public Goblin(string name) : base(name)
    {
        var clans = Enum.GetValues<GoblinClan>();
        Clan = clans[RNG.Next(clans.Length)];
        SpriteName = PickRandomSpriteName();

        if (RNG.NextDouble() <= 0.85)           // Most goblins are pretty tame...
        {
            AttackPower = RNG.Next(2, 4 + 1);   // Normal goblin attack ranges from 2 to 4.
            MaxHP = HP = RNG.Next(4, 6 + 1);    // Normal goblin health ranges from 4 to 6.
        }
        else                                    // But sometimes they're stronger!
        {
            AttackPower = RNG.Next(4, 6 + 1);   // Boss goblins have attack-strength 4 at their weakest, up to 6!
            MaxHP = HP = RNG.Next(8, 14 + 1);   // As well as 8-14 health. Stronger than some wizards! :O
        }
    }

    /// <summary>
    /// Creates a new goblin at the given position.
    /// </summary>
    public Goblin(string name, float x, float y) : this(name, new Vector2(x, y))
    { }

    /// <summary>
    /// Creates a new goblin at the given position.
    /// </summary>
    public Goblin(string name, Vector2 position) : this(name)
    {
        Position = position;
    }

    #endregion


    // Methods
    // --------------------------------

    public int CompareTo(Goblin? other)
    {
        // You shouldn't *need* to implement this version of CompareTo to do the assignment (to sort your AllEntities
        // list, you want CombatEntity.CompareTo). If you *want* to implement this one for fun, though, make it so that
        // they get sorted by AttackPower instead of MaxHP. Goblins are brutish, and value their raw strength. :)
        throw new NotImplementedException();
    }


    #region Provided methods

    public bool Equals(Goblin? other) => Equals((CombatEntity?)other); // Use base CombatEntity equality comparison
    public override bool Equals(object? obj) => Equals(obj as Goblin);
    public override int GetHashCode() => base.GetHashCode();

    public override string ToString()
    {
        string clan;

        #pragma warning disable format // Force VS not to mess up my indenting!
        switch (Clan)
        {
            case GoblinClan.Boko:           clan = "\U0001F43D"; break;     // "Pig Nose" emoji 🐽
            case GoblinClan.BogBrotherhood: clan = "\U0001F438"; break;     // "Frog Face" 🐸
            case GoblinClan.Cragmaw:        clan = "\u26F0\uFE0F"; break;   // "Mountain" ⛰
            default:                        clan = "\u2753"; break;         // "Red Question Mark" ❓
        }
        #pragma warning restore format

        return $"{clan} {Name} (pow. {AttackPower})";
    }

    // Static stuff
    // --------------------------------

    // Static constructor; runs once when this class is first used. Static constructors are a way to initialize `static`
    // fields when they might require more logic than a single assignment statement (like reading a file).
    static Goblin()
    {
        // Get output directory of program and find goblin names.
        var dllPath = Path.GetDirectoryName(typeof(Goblin).Assembly.Location);
        var txtPath = Path.Join(dllPath, "Resources/Assn1-Goblins.txt");

        // Trim them as we read them into a list.
        var nameList = new List<string>();
        foreach (string line in File.ReadLines(txtPath))
            nameList.Add(line.Trim());

        // Save as an array, since it won't need to grow again.
        names = nameList.ToArray();
    }

    protected static string PickRandomSpriteName()
    {
        double rand = RNG.NextDouble();
        return rand switch
        {
            <= 0.50 => "Enemies/Goblin",            // 50% chance
            <= 0.80 => "Enemies/Goblin.Sword",      // 30% chance
            <= 0.95 => "Enemies/Goblin.Glaive",     // 15% chance
            <= 0.99 => "Enemies/Goblin.Scepter",    // 4% chance
            _ => "Enemies/Martian",                 // 1% chance :o
        };
    }

    #endregion
}
