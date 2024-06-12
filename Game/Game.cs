namespace COIS2020.DamonFernandez0813575.Assignments;

using Microsoft.Xna.Framework;
using TrentCOIS.Tools.Visualization.EntityViz;
using System.Linq;

public class WizardFighterDX : Visualizer
{
    // Number of wizards and goblins
    private const int NUM_WIZARDS = 5;
    private const int NUM_GOBLINS = 10;

    // Constants for attack range and movement speed
    private const float WIZARD_ATTACK_RANGE = 1.25f;
    private const float WIZARD_MOVEMENT_SPEED = 0.50f;
    private const float GOBLIN_ATTACK_RANGE = 1.00f;
    private const float GOBLIN_MOVEMENT_SPEED = 0.60f;
    private const float KNOCK_BACK_AMOUNT = 1.50f;

    // Random number generator
    private readonly Random random = new Random();


    private ArrayList<CombatEntity> AllEntities { get; set; }
    private ArrayList<Wizard> AllWizards { get; set; }
    private ArrayList<Goblin> AllGoblins { get; set; }

    // Constructor to initialize lists and populate them with wizard and goblins
    public WizardFighterDX()
    {
        AllEntities = new ArrayList<CombatEntity>();
        AllWizards = new ArrayList<Wizard>();
        AllGoblins = new ArrayList<Goblin>();

        // Create and add wizards
        for (int i = 0; i < NUM_WIZARDS; i++)
        {
            Wizard newWizard = new Wizard();
            AllEntities.AddBack(newWizard);
            AllWizards.AddBack(newWizard);
        }
        // Create and add goblins
        for (int i = 0; i < NUM_GOBLINS; i++)
        {
            Goblin newGoblin = new Goblin();
            AllEntities.AddBack(newGoblin);
            AllGoblins.AddBack(newGoblin);
        }

        AllEntities.Sort(); // Sort all entities
    }


    protected override void Update()
    {
        updateWizards(); // Update wizard actions
        updateGoblins(); // Update goblin actions
        handleWiggleMovement(); // Handle random wiggle movement

        checkForCorpsesToRemove(); // Remove dead entities
        checkForWinCondition(); // Check for win condition

        // Spawn new goblin every 15 frames
        if (CurrentTimestamp % 15 == 0)
        {
            spawnNewGoblin();
        }

        // Spawn new wizard every 50 frames
        if (CurrentTimestamp % 50 == 0)
        {
            spawnNewWizard();
        }
    }

    // Check if either team has won
    private void checkForWinCondition()
    {
        if (AllGoblins.Count == 0 || AllWizards.Count == 0)
        {
            Stop(); // Stop the game if one team is eliminated
        }
    }

    // Remove entities that are dead (HP <= 0)
    private void checkForCorpsesToRemove()
    {
        // Remove dead entities from AllEntities
        for (int i = 0; i < AllEntities.Count; i++)
        {
            if (AllEntities.Get(i).HP <= 0)
            {
                AllEntities.RemoveAt(i);
            }
        }

        // Remove dead wizards from AllWizards
        for (int i = 0; i < AllWizards.Count; i++)
        {
            if (AllWizards.Get(i).HP <= 0)
            {
                AllWizards.RemoveAt(i);
            }
        }
        // Remove dead goblins from AllGoblins
        for (int i = 0; i < AllGoblins.Count; i++)
        {
            if (AllGoblins.Get(i).HP <= 0)
            {
                AllGoblins.RemoveAt(i);
            }
        }
    }

    // Get all wizards within the attack range of a goblin
    private ArrayList<Wizard> getAllWizardsInRange(Goblin goblin, ArrayList<Wizard> wizardsList)
    {
        ArrayList<Wizard> wizardsWithinRange = new ArrayList<Wizard>();
        foreach (Wizard wizard in wizardsList)
        {
            if (goblin.DistanceTo(wizard) <= GOBLIN_ATTACK_RANGE)
            {
                wizardsWithinRange.AddFront(wizard);
            }
        }
        return wizardsWithinRange;
    }

    // Get the weakest wizard from a list of wizards
    private Wizard getWeakestWizardFromList(ArrayList<Wizard> wizardsInRange)
    {
        Wizard currWeakestWizard = wizardsInRange.Get(0);
        foreach (Wizard wizard in wizardsInRange)
        {
            if (currWeakestWizard.HP > wizard.HP)
            {
                currWeakestWizard = wizard;
            }
        }
        return currWeakestWizard;
    }

    // Spawn a new goblin
    private void spawnNewGoblin()
    {
        Goblin newGoblin = new Goblin();
        AllGoblins.AddBack(newGoblin);
        // Insert goblin while preserving order
        AllEntities.InsertAt(getIndexToPreserveOrder(AllEntities, newGoblin), newGoblin);
        LogMessage($"The goblin {newGoblin} has arrived on the battlefield!");
    }

    // Get the index to insert a new entity while preserving order
    private int getIndexToPreserveOrder(ArrayList<CombatEntity> list, CombatEntity entity)
    {
        int leftPointer = 0;
        int rightPointer = list.Count - 1;
        int middlePointer;
        int middleValue;
        while (leftPointer <= rightPointer)
        {
            middlePointer = (leftPointer + rightPointer) / 2;
            middleValue = list.Get(middlePointer).MaxHP;
            if (entity.MaxHP >= middleValue)
            {
                rightPointer = middlePointer - 1;
            }
            else
            {
                leftPointer = middlePointer + 1;
            }
        }
        return leftPointer;
    }

    // Generate a random direction (-1 or 1)
    private int GenRandomDirection()
    {
        const int LEFT_OR_DOWN = -1;
        const int UP_OR_RIGHT = 1;

        // Randomly choose -1 or 1
        if (random.NextSingle() >= 0.50)
        {
            return LEFT_OR_DOWN;
        }
        else
        {
            return UP_OR_RIGHT;
        }
    }

    // Move a gobln randomly
    private void moveGoblinRandomly(Goblin goblin)
    {
        int directionToMoveVert = GenRandomDirection();
        int directionToMoveHoriz = GenRandomDirection();
        goblin.Move(GOBLIN_MOVEMENT_SPEED * directionToMoveHoriz, GOBLIN_MOVEMENT_SPEED * directionToMoveVert);
    }

    // Handle a goblin's attack on nearby wizards
    private void handleGoblinAttack(Goblin goblin)
    {
        ArrayList<Wizard> wizardsWithinRange = getAllWizardsInRange(goblin, AllWizards);

        // If the goblin can attack and there are wizards in range, attack the weakest one
        if (goblin.CanAttack(CurrentTimestamp) && wizardsWithinRange.Count != 0)
        {
            Wizard wizardToAttack = getWeakestWizardFromList(wizardsWithinRange);
            goblin.Attack(wizardToAttack, goblin.AttackPower, CurrentTimestamp);
            wizardToAttack.PushAwayFrom(goblin, KNOCK_BACK_AMOUNT);
            LogMessage($"{goblin} attacked {wizardToAttack} for {goblin.AttackPower}");
        }
    }

    // Update the state and actions of all goblins
    private void updateGoblins()
    {
        foreach (Goblin goblin in AllGoblins)
        {
            handleGoblinAttack(goblin); // Handle attacks on nearby wizards
            moveGoblinRandomly(goblin); // Move goblins randomly
        }
    }

    // Spawn a new wizard
    private void spawnNewWizard()
    {
        Wizard newWizard = new Wizard();
        AllWizards.AddFront(newWizard);
        // Insert wizard while preserving order
        AllEntities.InsertAt(getIndexToPreserveOrder(AllEntities, newWizard), newWizard);
        LogMessage($"The wizard {newWizard} has arrived on the battlefield!");
    }

    // Handle a wizard's attack on a goblin
    private void handleWizardAttack(Wizard wizard, Goblin goblin)
    {
        int wizardDamage = wizard.calculateWizardSpellDamage(wizard.DistanceTo(goblin));
        wizard.Attack(goblin, wizardDamage, CurrentTimestamp);
        LogMessage($"{wizard} attacked {goblin} for {wizardDamage}");
        goblin.PushAwayFrom(wizard, KNOCK_BACK_AMOUNT);
    }

    // Get the closest goblin to a given wizard
    private Goblin getClosestGoblinToWizard(Wizard wizard)
    {
        Goblin closestGoblin = AllGoblins.Get(0);
        float distanceToCurrClosestGoblin;
        float distanceToNewPotentialClosestGoblin;

        // Find the closest goblin
        foreach (Goblin goblin in AllGoblins)
        {
            distanceToCurrClosestGoblin = wizard.DistanceTo(closestGoblin);
            distanceToNewPotentialClosestGoblin = wizard.DistanceTo(goblin);
            if (distanceToCurrClosestGoblin > distanceToNewPotentialClosestGoblin)
            {
                closestGoblin = goblin;
            }
        }
        return closestGoblin;
    }

    // Update the state and actions of all wizards
    private void updateWizards()
    {
        foreach (Wizard wizard in AllWizards)
        {
            if (wizard.CanAttack(CurrentTimestamp))
            {
                // Area-of-effect atack for wizard
                foreach (Goblin goblin in AllGoblins)
                {
                    if (wizard.DistanceTo(goblin) <= WIZARD_ATTACK_RANGE)
                    {
                        handleWizardAttack(wizard, goblin);
                    }
                    else
                    {
                        handleWizardMovement(wizard);
                    }
                }
            }
            else
            {
                handleWizardMovement(wizard); // Move wizards towards goblins if it cant attack
            }
        }
    }

    // Handle the movement of a wizard towards the closest goblin
    private void handleWizardMovement(Wizard wizard)
    {
        Goblin closestGoblin = getClosestGoblinToWizard(wizard);
        wizard.MoveTowards(closestGoblin, WIZARD_MOVEMENT_SPEED);
    }

    // Handle random wiggle movement for all entities 
    private void handleWiggleMovement()
    {
        foreach (CombatEntity entity in AllEntities)
        {
            // Wiggle the entity around randomly
            float dx = random.NextSingle() - 0.5f;
            float dy = random.NextSingle() - 0.5f;
            entity.Move(dx, dy);

            // Ensure the entity stays within the bounds of the board
            entity.ClampPosition(EntityXRange, EntityYRange);
        }
    }

    // Return all entities in the game for visualization
    protected override IEnumerable<CombatEntity> GetEntities()
    {
        return AllEntities;
    }
}
