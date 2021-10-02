using Godot;
using System;

public class CombatantSelector : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var lbl = new Label();
        lbl.Text = "TEST1";

        GetTree().CurrentScene.FindChildByType<CanvasLayer>(2).AddChild(lbl);

        lbl.RectPosition = GetTree().CurrentScene.FindChildByType<Camera>(2).UnprojectPosition(this.GetGlobalLocation());

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
