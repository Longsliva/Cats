using Godot;
using System;

namespace Main;
public partial class MainScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddChild(new Target()
		{
			Position = new Vector2(100, 100)
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
