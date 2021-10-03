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
    public Array<PackedScene> Arenas;

    [Export]
    public int MatchesToWinTournament = 3;

    [Export]
    public int PointsToWinMatch = 3;

    [Export]
    public PackedScene BalanceAssistType;

    public int MatchNumber = 0;


    public static int PlayerCombantantType = 0;
    public static int Difficulty = 0;
    public int OpponentCombatantType = -1;

    public int OpponentPunchAIType = 0;
    public int OpponentBalanceAIType = 0;

    public int ArenaType = 0;

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

            if (armRootLocation.origin.y < 0f || Mathf.Abs(bodyRotation) > Mathf.Pi / 2)
            {
                Loses(c);
                break;
            }
        }

        if (!any) RestartPoint();
    }

    public void Loses(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
        {
            OpponentScore++;
            GetTree().CurrentScene.FindChildByType<InGameUI>(2)?.PlayerLoses();
        }
        else
        {
            PlayerScore++;
            GetTree().CurrentScene.FindChildByType<InGameUI>(2)?.PlayerWins();
        }

        if (OpponentScore >= PointsToWinMatch)
        {
            GetTree().ChangeScene("res://maps/LoseScreen.tscn");
            return;
        }

        if (PlayerScore >= PointsToWinMatch)
        {
            Console.WriteLine("Player has won the match");
            MatchNumber++;

            if (MatchNumber >= 3)
            {
                GetTree().ChangeScene("res://maps/WinScreen.tscn");
                return;
            }

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

        var oldArena = GetTree().CurrentScene.GetNode<Spatial>("Arena");
        if (oldArena != null)
        {
            oldArena.Name = "OldArena";
            oldArena.QueueFree();

            var env = oldArena.FindChildByType<WorldEnvironment>();
            if (env != null) env.Environment = null;
        }

        var arena = Arenas[ArenaType].Instance<Spatial>();
        arena.Name = "Arena";
        GetTree().CurrentScene.AddChild(arena);

        var player = CombatantTypes[PlayerCombantantType].Instance<Combatant>();
        player.IsPlayerControlled = true;
        player.Name = "Player";
        GetTree().CurrentScene.AddChild(player);
        player.SetGlobalLocation(new Vector3(4, 2, 0));

        if (Difficulty != 2)
        {
            player.AddChild(MovementAIs[2].Instance<Node>());
        }

        if (Difficulty == 0)
        {
            player.AddChild(BalanceAssistType.Instance());
        }


        var opponent = CombatantTypes[OpponentCombatantType].Instance<Combatant>();
        opponent.Name = "Opponent";
        GetTree().CurrentScene.AddChild(opponent);
        opponent.SetGlobalLocation(new Vector3(-4, 2, 0));

        opponent.AddChild(MovementAIs[OpponentBalanceAIType].Instance<Node>());
        opponent.AddChild(PunchingAIs[OpponentPunchAIType].Instance<Node>());

        GetTree().CurrentScene.FindChildByType<InGameUI>(2)?.MatchStart(PlayerScore + OpponentScore);
    }

    public void RestartMatch()
    {
        PlayerScore = 0;
        OpponentScore = 0;

        foreach (var c in GetTree().CurrentScene.FindChildrenByType<Combatant>(0))
        {
            c.QueueFree();
        }

        for (int i = 0; i < 1000; ++i)
        {
            OpponentCombatantType = Util.RandInt(0, CombatantTypes.Count);
            OpponentBalanceAIType = Util.RandInt(0, Math.Max(MovementAIs.Count - 2 + Difficulty, 1));
            OpponentPunchAIType = Util.RandInt(0, PunchingAIs.Count - (Difficulty == 0 ? 1 : 0));
            ArenaType = Util.RandInt(0, Arenas.Count);

            OpponentBalanceAIType = Math.Min(MatchNumber, OpponentBalanceAIType);
            OpponentPunchAIType = Math.Min(MatchNumber, OpponentPunchAIType);

            if (ArenaType == 0 && Difficulty == 0) continue; // no ice arena on easy
            if (ArenaType == 4 && Difficulty == 0) continue; // no debris arena on easy
            if (ArenaType == 3 && Difficulty != 2) continue; // seesaw only on hard

            break;
        }

        Console.WriteLine($"Restarting Match: OpponentCombatantType={OpponentCombatantType} OpponentBalanceAIType={OpponentBalanceAIType} OpponentPunchAIType={OpponentPunchAIType}");

        GetTree().CurrentScene.FindChildByType<InGameUI>(2)?.OverallMatchStart(MatchNumber);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (OS.IsDebugBuild())
        {
            if (@event.IsActionPressed("random_new_match")) RestartMatch();
        }


    }
}
