using Godot;
using System;

public class InGameUI : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    static string[] MATCH_NAMES = {
        "Quarterfinals",
        "Semifinals",
        "Finals",
        ""
    };

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var mr = GetTree().CurrentScene.FindChildByType<MatchRunner>(0);

        var player = GetTree().CurrentScene.GetNode<Combatant>("Player");
        var opponent = GetTree().CurrentScene.GetNode<Combatant>("Opponent");

        if (mr != null)
        {
            GetNode<Label>("GridContainer/LeftPoints").Text = mr.OpponentScore.ToString();
            GetNode<Label>("GridContainer/RightPoints").Text = mr.PlayerScore.ToString();

            GetNode<Label>("GridContainer/LeftCombatantName").Text = opponent?.CmbName ?? "???";
            GetNode<Label>("GridContainer/RightCombatantName").Text = player?.CmbName ?? "???";

            GetNode<Label>("MatchName").Text = MATCH_NAMES[mr.MatchNumber];
        }
    }
}
