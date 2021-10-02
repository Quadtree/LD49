using Godot;
using Godot.Collections;
using System;
using System.Linq;

public class MatchRunner : Spatial
{
    // Scores in the current match
    public int PlayerScore = 0;
    public int OpponentScore = 0;

    [Export]
    public Array<PackedScene> CombatantTypes;

    [Export]
    public Array<PackedScene> MovementAIs;

    [Export]
    public Array<PackedScene> PunchingAIs;

    [Export]
    public int MatchesToWinTournament = 3;

    [Export]
    public int PointsToWinMatch = 3;

    public int MatchNumber = 0;


    public int PlayerCombantantType = 0;
    public int OpponentCombatantType = -1;

    public int OpponentPunchAIType = 0;
    public int OpponentBalanceAIType = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (OpponentCombatantType == -1)
        {
            RestartMatch();
        }

        var combatants = GetTree().CurrentScene.FindChildrenByType<Combatant>(0);

        var any = false;

        foreach (var c in combatants)
        {
            any = true;
            var body = c.FindChildByName<RigidBody>("Body");

            var armRootLocation = c.FindChildByName<Spatial>("ArmJointCenter").GlobalTransform;
            var bodyRotation = body.Rotation.z;

            if (armRootLocation.origin.y < 1f || Mathf.Abs(bodyRotation) > Mathf.Pi / 2)
            {
                Loses(c);
            }
        }

        if (!any) RestartPoint();
    }

    public void Loses(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
            OpponentScore++;
        else
            PlayerScore++;

        if (OpponentScore >= PointsToWinMatch)
        {
            // @TODO: Show "you lose the tournament" screen
        }

        if (PlayerScore >= PointsToWinMatch)
        {
            Console.WriteLine("Player has won the match");
            MatchNumber++;
            RestartMatch();
        }

        RestartPoint();
    }

    public void RestartPoint()
    {
        Console.WriteLine($"Starting Point: PlayerScore={PlayerScore} OpponentScore={OpponentScore}");

        foreach (var c in GetTree().CurrentScene.FindChildrenByType<Combatant>(0))
        {
            c.QueueFree();
            c.Name += "_X";
        }

        var player = CombatantTypes[PlayerCombantantType].Instance<Combatant>();
        player.IsPlayerControlled = true;
        player.Name = "Player";
        GetTree().CurrentScene.AddChild(player);
        player.SetGlobalLocation(new Vector3(4, 2, 0));


        var opponent = CombatantTypes[OpponentCombatantType].Instance<Combatant>();
        opponent.Name = "Opponent";
        GetTree().CurrentScene.AddChild(opponent);
        opponent.SetGlobalLocation(new Vector3(-4, 2, 0));

        opponent.AddChild(MovementAIs[OpponentBalanceAIType].Instance<Node>());
        opponent.AddChild(PunchingAIs[OpponentPunchAIType].Instance<Node>());
    }

    public void RestartMatch()
    {
        PlayerScore = 0;
        OpponentScore = 0;

        foreach (var c in GetTree().CurrentScene.FindChildrenByType<Combatant>(0))
        {
            c.QueueFree();
        }

        OpponentCombatantType = Util.RandInt(0, CombatantTypes.Count);
        OpponentBalanceAIType = Util.RandInt(0, MovementAIs.Count);
        OpponentPunchAIType = Util.RandInt(0, PunchingAIs.Count);

        Console.WriteLine($"Restarting Match: OpponentCombatantType={OpponentCombatantType} OpponentBalanceAIType={OpponentBalanceAIType} OpponentPunchAIType={OpponentPunchAIType}");
    }
}
