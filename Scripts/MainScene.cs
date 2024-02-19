using Godot;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace Main;
public partial class MainScene : Node2D
{
	const int SPAWN_OFFSET = 80;

	PathFollow2D spawnArea;
	Random rnd = new();
	List<Target> targets = new();
	int Score = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        spawnArea = GetNode<PathFollow2D>("Path2D/PathFollow2D");
        Path2D a = GetNode<Path2D>("Path2D");

        a.Curve = new Curve2D();
		a.Curve.AddPoint(new Vector2(-SPAWN_OFFSET, -SPAWN_OFFSET));
        a.Curve.AddPoint(new Vector2(-SPAWN_OFFSET, Statics.SCREEN_SIZE_Y+ SPAWN_OFFSET));
        a.Curve.AddPoint(new Vector2(Statics.SCREEN_SIZE_X + SPAWN_OFFSET, Statics.SCREEN_SIZE_Y + SPAWN_OFFSET));
        a.Curve.AddPoint(new Vector2(Statics.SCREEN_SIZE_X+ SPAWN_OFFSET, -SPAWN_OFFSET));


		NewTargetSpawn(3);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for(int i = 0; i < targets.Count; i++)
        {
			if (targets[i].IsReadyToDel)
			{
				Score++;
                targets.RemoveAt(i);
				i--;
			}
		}
	}
    private void NewTargetSpawn()
    {
		NewTargetSpawn(1);
    }
    private void NewTargetSpawn(int spawnCount)
	{
		GetNode<Timer>("Timer").WaitTime = rnd.Next(10,20)/10f;
		
		if(targets.Count < 2)
		{
			spawnCount = 3;
        }

        for(int i = 0; i < spawnCount; i++)
		{
            spawnArea.ProgressRatio = rnd.NextSingle();
            targets.Add(new Target()
            {
                Position = spawnArea.Position,
            });

            AddChild(targets[^1]);
        }
    }
}
