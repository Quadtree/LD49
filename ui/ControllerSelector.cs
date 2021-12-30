using System;
using Godot;

public class ControllerSelector : VBoxContainer
{
    [Export]
    public int Selection = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Selection != -1)
        {
            GetChild<Button>(Selection).Pressed = true;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var nc = GetChildCount();
        Selection = -1;
        for (int i = 0; i < nc; ++i)
        {
            var b = GetChild<Button>(i);
            if (b.Pressed)
            {
                Selection = i;
            }
        }
    }
}
