using Godot;
using System;
using System.Collections.Generic;

namespace Main;
public partial class MainScene : Control
{
    string scoreLabelBase;

	const int SPAWN_OFFSET = 80;

	Label scoreLabel;

    Node TargetParent;
	PathFollow2D spawnArea;
	Random rnd = new();
	List<Target> targets = new();
	int Score = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        TargetParent = GetNode("TargetParent");
        scoreLabel = GetNode<Label>("Label");
        scoreLabelBase = scoreLabel.Text;
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
        for (int i = 0; i < targets.Count; i++)
        {
			if (targets[i].IsRunAway)
			{
				if (targets[i] is Cats) Score++;
				else Score--;
                targets.RemoveAt(i);
				i--;
                scoreLabel.Text = scoreLabelBase + Score;

            }
		}
    }
    public void NewTargetSpawn()
    {
		NewTargetSpawn(1);
    }
    private void NewTargetSpawn(int spawnCount)
	{
		GetNode<Timer>("Timer").WaitTime = rnd.Next(10,20)/10f;
		
		if(targets.Count == 0)
		{
			spawnCount = 3;
        }

        for(int i = 0; i < spawnCount; i++)
		{
            spawnArea.ProgressRatio = rnd.NextSingle();
            
			if(Score>15)
			{
                if(rnd.Next(4) == 0)
				{
                    targets.Add(new Dogs()
                    {
                        Position = spawnArea.Position,
                    });
                }
				else
				{
                    targets.Add(new Cats()
                    {
                        Position = spawnArea.Position,
                    });
                }
            }
			else
			{
                targets.Add(new Cats()
                {
                    Position = spawnArea.Position,
                });
            }

            TargetParent.AddChild(targets[^1]);
        }
    }
}
