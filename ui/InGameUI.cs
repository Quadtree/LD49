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

    static string[] MESSAGES = {
        "YouWinMessage",
        "YouLoseMessage",
        "MatchStartMessage",
        "MatchStartMessage2",
    };

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var mr = GetTree().CurrentScene.FindChildByType<MatchRunner>(0);

        var player = GetTree().CurrentScene.GetNode<Combatant>("Player");
        var opponent = GetTree().CurrentScene.GetNode<Combatant>("Opponent");

        if (mr != null)
        {
            GetNode<Label>("GridContainer/LeftPoints").Text = mr.PlayerScore.ToString();
            GetNode<Label>("GridContainer/RightPoints").Text = mr.OpponentScore.ToString();

            GetNode<Label>("GridContainer/LeftCombatantName").Text = player?.CmbName ?? "???";
            GetNode<Label>("GridContainer/RightCombatantName").Text = opponent?.CmbName ?? "???";

            GetNode<Label>("MatchName").Text = MATCH_NAMES[mr.MatchNumber];
        }

        foreach (var v in MESSAGES)
        {
            var decayRate = 0.65f;

            if (v == "MatchStartMessage2") decayRate = 0.35f;

            GetNode<Label>(v).Modulate = new Color(1, 1, 1, Mathf.Max(0, GetNode<Label>(v).Modulate.a - delta * decayRate));
        }
    }

    public void PlayerWins()
    {
        GetNode<Label>("YouWinMessage").Modulate = new Color(1, 1, 1, 1);
    }

    public void PlayerLoses()
    {
        GetNode<Label>("YouLoseMessage").Modulate = new Color(1, 1, 1, 1);
    }

    public void MatchStart(int round)
    {
        GetNode<Label>("MatchStartMessage").Modulate = new Color(1, 1, 1, 1);
        GetNode<Label>("MatchStartMessage").Text = $"Round {round + 1}. Fight!";
    }

    public void OverallMatchStart(int id)
    {
        GetNode<Label>("MatchStartMessage2").Modulate = new Color(1, 1, 1, 1);
        GetNode<Label>("MatchStartMessage2").Text = $"{MATCH_NAMES[id]} starting!";
    }
}
