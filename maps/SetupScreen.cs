using Godot;
using System;

public class SetupScreen : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    static string[] DIFFICULTY_BUTTONS = {
        "EasyButton",
        "MediumButton",
        "HardButton",
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < DIFFICULTY_BUTTONS.Length; ++i)
        {
            this.FindChildByName<BaseButton>(DIFFICULTY_BUTTONS[i], 10).Connect("pressed", this, $"SetDifficultyTo{i}");
        }

        SetDifficulty(1);
    }

    private void SetDifficulty(int level)
    {
        MatchRunner.Difficulty = level;

        for (int i = 0; i < DIFFICULTY_BUTTONS.Length; ++i)
        {
            this.FindChildByName<BaseButton>(DIFFICULTY_BUTTONS[i], 10).Pressed = i == level;
        }
    }

    public void SetDifficultyTo0()
    {
        SetDifficulty(0);
    }

    public void SetDifficultyTo1()
    {
        SetDifficulty(1);
    }

    public void SetDifficultyTo2()
    {
        SetDifficulty(2);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
