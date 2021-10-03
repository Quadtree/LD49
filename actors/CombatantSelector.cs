using Godot;
using System;

public class CombatantSelector : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    int CmbId = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //var lbl = new Label();
        //lbl.Text = this.FindChildByType<Combatant>(2).CmbName;

        var btn = new Button();
        btn.Text = this.FindChildByType<Combatant>(2).CmbName;

        var easyButton = GetTree().CurrentScene.FindChildByName<Button>("EasyButton", 10);
        btn.AddStyleboxOverride("normal", easyButton.GetStylebox("normal"));
        btn.AddStyleboxOverride("hover", easyButton.GetStylebox("hover"));
        btn.AddStyleboxOverride("pressed", easyButton.GetStylebox("pressed"));

        var con = new CenterContainer();
        con.RectPosition = GetTree().CurrentScene.FindChildByType<Camera>(2).UnprojectPosition(this.GetGlobalLocation()) - new Vector2(100, 100) + new Vector2(0, -300);
        con.RectSize = new Vector2(200, 200);
        con.AddChild(btn);

        //lbl.RectPosition = GetTree().CurrentScene.FindChildByType<Camera>(2).UnprojectPosition(this.GetGlobalLocation());
        //btn.RectPosition = GetTree().CurrentScene.FindChildByType<Camera>(2).UnprojectPosition(this.GetGlobalLocation());

        //GetTree().CurrentScene.FindChildByType<CanvasLayer>(2).AddChild(lbl);
        GetTree().CurrentScene.FindChildByType<CanvasLayer>(2).AddChild(con);

        btn.Connect("pressed", this, nameof(ButtonPressed));
    }

    private void ButtonPressed()
    {
        GetTree().ChangeScene("res://maps/default.tscn");

        MatchRunner.PlayerCombantantType = CmbId;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
