using Godot;
using System;

namespace Main;
public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Statics.SCREEN_SIZE_X = GetParent<Window>().Size.X;
        Statics.SCREEN_SIZE_Y = GetParent<Window>().Size.Y;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartCats()
	{
		GetTree().ChangeSceneToFile("res://Game.tscn");
	}
}
