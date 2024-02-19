using Godot;
using System;
using System.Collections.Generic;

namespace Main;
public partial class MainScene : Node2D
{
	List<Target> targets = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Path2D a = GetNode<Path2D>("Path2D");

        a.Curve = new Curve2D();
		a.Curve.AddPoint(new Vector2(-40, -40));
        a.Curve.AddPoint(new Vector2(-40, Statics.SCREEN_SIZE_Y+40));
        a.Curve.AddPoint(new Vector2(Statics.SCREEN_SIZE_X + 40, Statics.SCREEN_SIZE_Y + 40));
        a.Curve.AddPoint(new Vector2(Statics.SCREEN_SIZE_X+40, -40));


        targets.Add(new Target());

		AddChild(targets[^1]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void NewTargetSpawn()
	{

	}
}
