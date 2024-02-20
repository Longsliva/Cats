using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main;
public partial class MainScene : Control
{
    string scoreLabelBase;

	const int SPAWN_OFFSET = 80;

	Label scoreLabel;

    Node TargetParent;
	PathFollow2D spawnArea;
    readonly Random rnd = new();
    readonly List<Target> targets = new();
	int Score = 0;
    // Called when the node enters the scene tree for the first time.
    readonly AudioStreamPlayer great = new();
    List<string> greatStreams;

	public override void _Ready()
	{
        greatStreams = DirAccess.GetFilesAt("res://Media/Audio/Voice/Great/").ToList();
        for (int i = 0; i < greatStreams.Count; i++)
        {
            if (!greatStreams[i].EndsWith(".import"))
            {
                greatStreams.RemoveAt(i);
                i--;
            }
            else
            {
                greatStreams[i] = greatStreams[i].Replace(".import", string.Empty);
                greatStreams[i] = "res://Media/Audio/Voice/Great/" + greatStreams[i];
            }
        }

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

        AddChild(great);

		NewTargetSpawn(3);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        for (int i = 0; i < targets.Count; i++)
        {
			if (targets[i].IsRunAway)
			{
				if (targets[i] is Cats)
                {
                    Score++;
                    if(Score%10 == 0)
                    {
                        great.Stream = GD.Load<AudioStream>(greatStreams[rnd.Next(greatStreams.Count)]);
                        great.Play();
                    }
                }
				else
                {
                    if(!(targets[i] as Dogs).IsSelfDistract) Score--;
                }
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
