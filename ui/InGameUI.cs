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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var mr = GetTree().CurrentScene.FindChildByType<MatchRunner>(0);

        if (mr != null)
        {
            GetNode<Label>("GridContainer/LeftPoints").Text = mr.OpponentScore.ToString();
            GetNode<Label>("GridContainer/RightPoints").Text = mr.PlayerScore.ToString();
        }
    }
}
