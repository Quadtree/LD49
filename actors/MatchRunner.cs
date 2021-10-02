using Godot;
using Godot.Collections;
using System;
using System.Linq;

public class MatchRunner : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public int PlayerScore = 0;
    public int OpponentScore = 0;

    [Export]
    public Array<PackedScene> CombatantTypes;

    [Export]
    public Array<PackedScene> MovementAIs;

    [Export]
    public Array<PackedScene> PunchingAIs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (GetTree().CurrentScene.FindChildrenByType<Combatant>(0).Count() == 0)
        {
            RestartMatch();
        }
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
