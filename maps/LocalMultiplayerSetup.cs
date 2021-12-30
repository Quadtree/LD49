using System;
using Godot;

public class LocalMultiplayerSetup : Control
{
    public static Combatant.ControllerType LeftPlayerController;
    public static Combatant.ControllerType RightPlayerController;
    public static int LeftPlayerCombatantType;
    public static int RightPlayerCombatantType;
    public static int MapIndex;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.FindChildByName<Button>("StartGame").Connect("pressed", this, nameof(StartGamePressed));
    }

    void StartGamePressed()
    {
        LeftPlayerController = (Combatant.ControllerType)this.FindChildByName<ControllerSelector>("LPController", 10).Selection;
        RightPlayerController = (Combatant.ControllerType)this.FindChildByName<ControllerSelector>("RPController", 10).Selection;
        LeftPlayerCombatantType = this.FindChildByName<ControllerSelector>("LPCombatant", 10).Selection;
        RightPlayerCombatantType = this.FindChildByName<ControllerSelector>("RPCombatant", 10).Selection;
        MapIndex = this.FindChildByName<ControllerSelector>("MapSelection", 10).Selection;

        GetTree().ChangeScene("res://maps/LocalMultiplayerDefault.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
