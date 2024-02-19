using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main;
public partial class Target : Sprite2D
{
    Random rnd = new Random();
    const float MAX_SPEED = 120;

	Vector2I MoveToThis;
	float CurrentSpeed;

	int counter = 0;
	float needAngle = 0f;

	bool IsFliping = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		List<string> allCats = DirAccess.GetFilesAt("res://Cats").ToList();
		for(int i = 0; i < allCats.Count; i++)
		{
			if (!allCats[i].EndsWith(".import"))
			{
				allCats.RemoveAt(i);
				i--;
			}
			else
			{
				allCats[i] = allCats[i].Replace(".import", string.Empty);
			}
		}
        
        Texture = GD.Load<Texture2D>("Cats/" + allCats[rnd.Next(allCats.Count)]);
	}

	public override void _Process(double delta)
	{
		if(Position == MoveToThis)
		{
			counter = rnd.Next(5);
			GetNewRandomTargetPosition();
		}
		else
		{
			if(Rotation == needAngle)
			{
                if (counter > 0)
                {
                    counter--;
                }
                else
                {
					Position = Position.MoveToward(MoveToThis, (float)delta);
                }
            }
			else
			{
				Rotation = Mathf.MoveToward(Rotation, needAngle, (float)delta);
			}

			
		}
	}
	private void GetNewRandomTargetPosition()
	{
        MoveToThis = new Vector2I(rnd.Next(20, Statics.SCREEN_SIZE_X - 20), rnd.Next(20, Statics.SCREEN_SIZE_Y - 20));
		Vector2 forDraw = Position - MoveToThis;
        float needRot;
        if (forDraw.X >= 0 && forDraw.Y >= 0 || forDraw.X < 0 && forDraw.Y < 0)
        {
            needRot = MathF.Abs(MathF.Atan(forDraw.Y / forDraw.X));
        }
        else
        {
            needRot = MathF.Abs(MathF.Atan(forDraw.X / forDraw.Y));
        }

        if (forDraw.X >= 0 && forDraw.Y >= 0) needRot += MathF.PI / 2;
        if (forDraw.X < 0 && forDraw.Y < 0) needRot += MathF.PI * 1.5f;
        if (forDraw.X < 0 && forDraw.Y >= 0) needRot += MathF.PI;
        needRot -= MathF.PI / 2;
		needAngle = needRot/15;
    }
	private async void Flip()
	{
		if (IsFliping) return;
		IsFliping = true;

		for(int i = 0; i < 9; i++)
		{
			Scale = new Vector2(Scale.X - 0.1f, Scale.Y);
			await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
		}
		FlipH = !FlipH; 
        for (int i = 0; i < 9; i++)
        {
            Scale = new Vector2(Scale.X + 0.1f, Scale.Y);
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
        }

        IsFliping = false;
	}
}
