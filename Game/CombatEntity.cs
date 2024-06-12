namespace COIS2020.DamonFernandez0813575.Assignments;

using Microsoft.Xna.Framework; // Needed for `Vector2`
using TrentCOIS.Tools.Visualization.EntityViz;


public class CombatEntity : Entity, IHasHP, IComparable<CombatEntity>, IEquatable<CombatEntity>
{
    // Instance fields
    // --------------------------------

    public int HP { get; set; }
    public int MaxHP { get; protected set; }

    /// <summary>
    /// How many frames/ticks this entity needs to wait before it can attack again.
    /// </summary>
    public uint AttackCooldown { get; private set; } = 10;

    /// <summary>
    /// The timestamp of the last time this entity attacked; used to compare against its <see cref="AttackCooldown"/>.
    /// </summary>
    protected uint LastAttackTime { get; set; } = 0;


    // Constructors
    // --------------------------------

    #region Constructors

    /// <summary>
    /// Creates a new combat entity at a random position.
    /// </summary>
    ///
    /// <remarks>
    /// By default, the random position will range between ±7.5 the x- and y-axes. Use one of the other constructors to
    /// supply another position.
    /// </remarks>
    ///
    /// <param name="name">This entity's name.</param>
    public CombatEntity(string name) : base(name)
    {
        float x = RNG.NextSingle() * 15f - 7.5f;
        float y = RNG.NextSingle() * 15f - 7.5f;
        Position = new Vector2(x, y);
    }

    /// <summary>
    /// Creates a new combat entity at the given position.
    /// </summary>
    public CombatEntity(string name, float x, float y) : base(name, x, y)
    { }

    /// <summary>
    /// Creates a new combat entity at the given position.
    /// </summary>
    public CombatEntity(string name, Vector2 position) : base(name, position)
    { }

    #endregion


    // Methods
    // --------------------------------

    public int CompareTo(CombatEntity? other)
    {

        if (other == null)
        {
            return 1;
        }
        else
        {
            return this.MaxHP.CompareTo(other.MaxHP);

        }
    }


    #region Provided methods

    /// <summary>
    /// Attacks another entity and deals the given amount of damage.
    /// </summary>
    ///
    /// <param name="other">The entity to attack.</param>
    /// <param name="damage">How much damage to deal.</param>
    /// <param name="currentTimestamp">
    /// The current game timestamp. Can be obtained from the <c>CurrentTimestamp</c> property in the <see
    /// cref="WizardFighterDX">Game class</see>.
    /// </param>
    public void Attack(CombatEntity other, int damage, uint currentTimestamp)
    {
        other.HP -= damage;
        if (other.HP < 0)
            other.HP = 0;
        LastAttackTime = currentTimestamp;
    }


    /// <summary>
    /// Checks whether the current entity is allowed to attack again yet.
    /// </summary>
    ///
    /// <param name="currentTimestamp">
    /// The current game timestamp. Can be obtained from the <c>CurrentTimestamp</c> property in the <see
    /// cref="WizardFighterDX">Game class</see>.
    /// </param>
    public bool CanAttack(uint currentTimestamp)
    {
        // They can attack either if their cooldown is done or if their cooldown is starting on this frame (allows them
        // to attack more than once per frame, to let wizards to area-of-effect attacks).
        return currentTimestamp > LastAttackTime + AttackCooldown || currentTimestamp == LastAttackTime;
    }


    /// <summary>
    /// Computes the distance between this entity and some other entity.
    /// </summary>
    public float DistanceTo(Entity other)
    {
        return (other.Position - Position).Length();
    }


    /// <summary>
    /// Updates this entity's position.
    /// </summary>
    ///
    /// <param name="dx">An offset to add to this entity's x-position.</param>
    /// <param name="dy">An offset to add to this entity's y-position.</param>
    public void Move(float dx, float dy)
    {
        Move(new Vector2(dx, dy));
    }

    /// <summary>
    /// Updates this entity's position.
    /// </summary>
    ///
    /// <param name="deltaPos">An offset vector to add to this entity's position.</param>
    public void Move(Vector2 deltaPos)
    {
        Position += deltaPos;
    }


    /// <summary>
    /// Moves this entity towards another entity by at most <paramref name="speed"/> units.
    /// </summary>
    ///
    /// <param name="other">The entity to move towards.</param>
    /// <param name="speed">The farthest this entity is allowed to move in a single frame.</param>
    public void MoveTowards(Entity other, float speed)
    {
        Vector2 ab = other.Position - Position;             // A to B = B - A.
        Move(ScaleVector(ab, size: speed, clamp: true));    // Scale before moving.
    }

    /// <summary>
    /// Moves this entity in the opposite direction from another.
    /// </summary>
    ///
    /// <param name="other">The entity to move away from.</param>
    /// <param name="amount">How far to move away from the given point.</param>
    public void PushAwayFrom(Entity other, float amount)
    {
        Vector2 ba = Position - other.Position;             // "B to A" is the same direction as "A away from B".
        Move(ScaleVector(ba, size: amount, clamp: false));  // Scale before moving.
    }


    /// <summary>
    /// Scales a movement vector.
    /// </summary>
    ///
    /// <remarks>
    /// If <paramref name="clamp"/> is true, then <paramref name="vec"/> is scaled to be no longer than
    /// <paramref name="size"/>. Otherwise, <paramref name="vec"/> is scaled to have a length of exactly
    /// <paramref name="size"/>.
    /// </remarks>
    ///
    /// <param name="vec">The vector to scale.</param>
    /// <param name="size">The desired size.</param>
    /// <param name="clamp">Which mode to scale with.</param>
    private static Vector2 ScaleVector(Vector2 vec, float size, bool clamp)
    {
        float len = vec.Length();                       // Check length from (A->B).
        if (len <= 0.0000001f) return Vector2.Zero;     // If zero(ish), quit to avoid NaN.
        else if (clamp && len < size) return vec;       // If clamping, and if already small enough, just return.
        else return vec / len * size;                   // Otherwise, normalize and re-scale to desired size.
    }


    /// <summary>
    /// Ensures that this entity's position does not fall outside of a given range.
    /// </summary>
    ///
    /// <remarks>
    /// There are two built-in <c>EntityXRange</c> and <c>EntityYRange</c> properties on the base
    /// <see cref="Visualizer"/> class that may be useful here.
    /// </remarks>
    ///
    /// <param name="xRange">A range of allowed x-values for this entity's position (min, max).</param>
    /// <param name="yRange">A range of allowed y-values for this entity's position (min, max).</param>
    public void ClampPosition((float, float) xRange, (float, float) yRange)
    {
        var (xMin, xMax) = xRange;
        var (yMin, yMax) = yRange;
        ClampPosition(xMin, xMax, yMin, yMax);
    }

    /// <summary>
    /// Ensures that this entity's position does not fall outside of a given range.
    /// </summary>
    ///
    /// <remarks>
    /// There are two built-in <c>EntityXRange</c> and <c>EntityYRange</c> properties on the base
    /// <see cref="Visualizer"/> class that may be useful here.
    /// </remarks>
    ///
    /// <param name="xMin">The minimum allowed x-value for this entity's position.</param>
    /// <param name="xMax">The maximum allowed x-value for this entity's position.</param>
    /// <param name="yMin">The minimum allowed y-value for this entity's position.</param>
    /// <param name="yMax">The maximum allowed y-value for this entity's position.</param>
    public void ClampPosition(float xMin, float xMax, float yMin, float yMax)
    {
        float newX = Math.Clamp(Position.X, xMin, xMax);
        float newY = Math.Clamp(Position.Y, yMin, yMax);
        Position = new Vector2(newX, newY);
    }


    // `=>` is modern-C# syntax for one-liner methods
    public bool Equals(CombatEntity? other) => Equals((Entity?)other); // Use base Entity equality comparison
    public override bool Equals(object? obj) => Equals(obj as CombatEntity);
    public override int GetHashCode() => base.GetHashCode();



    #endregion
}
