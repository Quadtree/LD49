using Godot;
using System;

public class ExitGame : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Console.WriteLine("Exit game _Ready");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("exit_game"))
        {
            Console.WriteLine("Exiting via _Input");
            GetTree().Quit();
        }

        if (@event.IsActionPressed("restart"))
        {
            Console.WriteLine("Restarting via _Input");
            GetTree().ChangeScene("res://maps/SetupScreen.tscn");
        }
    }
}
