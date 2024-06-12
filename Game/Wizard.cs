namespace COIS2020.DamonFernandez0813575.Assignments;

using Microsoft.Xna.Framework; // Needed for `Vector2`
using TrentCOIS.Tools.Visualization.EntityViz;


public enum Element
{
    Fire,
    Water,
    Earth,
    Dark,
    Light,
    Wind,
    Ice,
    Electric,
}

public class Wizard : CombatEntity, IComparable<Wizard>, IEquatable<Wizard>
{
    private static readonly string[] names; // Names get filled from txt file automatically


    // Instance fields
    // --------------------------------

    public int SpellLevel { get; set; }
    public Element SpellElement { get; set; }


    // Constructors
    // --------------------------------

    #region Constructors

    /// <summary>
    /// Creates a new wizard with a random name and position.
    /// </summary>
    public Wizard() : this(names[RNG.Next(names.Length)])
    { }

    /// <summary>
    /// Creates a new wizard with a random position.
    /// </summary>
    public Wizard(string name) : base(name)
    {
        var elements = Enum.GetValues<Element>();
        SpellElement = elements[RNG.Next(elements.Length)];
        SpriteName = PickRandomSpriteName();

        SpellLevel = RNG.Next(1, 5);    // Wizards start at levels 1 to 4.
        MaxHP = HP = RNG.NextDouble() <= 0.70
            ? RNG.Next(15, 21)          // 70% chance of 15-20 health
            : RNG.Next(10, 16);         // 30% chance of being a tad weaker, with 10-15 health.
    }

    /// <summary>
    /// Creates a new wizard at the given position.
    /// </summary>
    public Wizard(string name, float x, float y) : this(name, new Vector2(x, y))
    { }

    /// <summary>
    /// Creates a new wizard at the given position.
    /// </summary>
    public Wizard(string name, Vector2 position) : this(name)
    {
        Position = position;
    }

    #endregion


    // Methods
    // --------------------------------

    public int CompareTo(Wizard? other)
    {
        // You shouldn't *need* to implement this version of CompareTo to do the assignment (to sort your AllEntities
        // list, you want CombatEntity.CompareTo). If you *want* to implement this one for fun, though, make it so that
        // they get sorted by SpellLevel instead of MaxHP. Wizards, while vain, value their magical intellect. :)
        throw new NotImplementedException();
    }


    #region Provided methods

    public bool Equals(Wizard? other) => Equals((CombatEntity?)other); // Use base CombatEntity equality comparison
    public override bool Equals(object? obj) => Equals(obj as Wizard);
    public override int GetHashCode() => base.GetHashCode();




    public override string ToString()
    {
        string element;

        #pragma warning disable format // Force VS not to mess up my indenting!
        switch (SpellElement)
        {
            // (find Unicode code-points for emojis on emojipedia.org under the Technical Information tab)
            case Element.Fire:      element = "\U0001F525"; break;      // "Fire" 🔥
            case Element.Water:     element = "\U0001F4A7"; break;      // "Droplet" 💧
            case Element.Earth:     element = "\U0001FAA8"; break;      // "Rock" 🪨
            case Element.Dark:      element = "\U0001F31C"; break;      // "Last Quarter Moon Face" 🌜
            case Element.Light:     element = "\U0001F31E"; break;      // "Sun with Face" 🌞
            case Element.Wind:      element = "\U0001F4A8"; break;      // "Dash Symbol" 💨
            case Element.Ice:       element = "\u2744\uFE0F"; break;    // "Snowflake" ❄
            case Element.Electric:  element = "\u26A1"; break;          // "High Voltage Sign" ⚡
            default:                element = "\u2753"; break;          // "Black Question Mark Ornament" ❓
        }
        #pragma warning restore format

        return $"{element} {Name} (lvl. {SpellLevel})";
    }

    // Static stuff
    // --------------------------------

    // Static constructor; runs once when this class is first used. Static constructors are a way to initialize `static`
    // fields when they might require more logic than a single assignment statement (like reading a file).
    static Wizard()
    {
        // Get output directory of program and find goblin names.
        var dllPath = Path.GetDirectoryName(typeof(Wizard).Assembly.Location);
        var txtPath = Path.Join(dllPath, "Resources/Assn1-Wizards.txt");

        // Add to a list, splitting at the 1st space and grabbing the second half (plus trimming extra whitespace).
        var nameList = new List<string>();
        foreach (string line in File.ReadLines(txtPath))
            nameList.Add(line.Split(null, 2)[1].Trim());

        // Save as an array, since it won't need to grow again.
        names = nameList.ToArray();
    }

    protected static string PickRandomSpriteName()
    {
        var person = RNG.Next(1, 14 + 1);                               // 14 skins to choose from; 1 to 14 inclusive
        var weapon = (RNG.NextDouble() >= 0.01) ? "Scepter" : "Gun";    // 1% chance for wizard to spawn with a gun lol
        return $"People/Skin{person}.{weapon}";
    }

    public int calculateWizardSpellDamage(float distance)
    {
        if (distance == 0)
        {
            return this.SpellLevel;
        }
        else
        {
            return (int)Math.Ceiling(this.SpellLevel / distance);

        }

    }


    #endregion
}
