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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
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

        if (!any) RestartMatch();
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
            PlayerScore = 0;
            OpponentScore = 0;
        }

        RestartMatch();
    }

    public void RestartMatch()
    {
        foreach (var c in GetTree().CurrentScene.FindChildrenByType<Combatant>(0))
        {
            c.QueueFree();
        }

        var player = CombatantTypes[Util.RandInt(0, CombatantTypes.Count)].Instance<Combatant>();
        player.IsPlayerControlled = true;
        player.Name = "Player";
        GetTree().CurrentScene.AddChild(player);
        player.SetGlobalLocation(new Vector3(4, 2, 0));


        var opponent = CombatantTypes[Util.RandInt(0, CombatantTypes.Count)].Instance<Combatant>();
        opponent.Name = "Opponent";
        GetTree().CurrentScene.AddChild(opponent);
        opponent.SetGlobalLocation(new Vector3(-4, 2, 0));

        opponent.AddChild(MovementAIs[Util.RandInt(0, MovementAIs.Count)].Instance<Node>());
        opponent.AddChild(PunchingAIs[Util.RandInt(0, PunchingAIs.Count)].Instance<Node>());
    }
}
