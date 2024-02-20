using Godot;
using System;

namespace Main;
public abstract partial class Target : Sprite2D
{
    const int MAX_SPEED = 360;
    const int MIN_SPEED = 120;

    readonly protected Random RndGen = new();

    Vector2I MoveToThis;
	float CurrentSpeed;
	float targetSpeed;

	sbyte IsRotationToRight = 1;

	bool IsFliping = false;
	float waitTime = 0f;

	public bool IsRunAway = false;
	protected bool IsFalling = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		YSortEnabled = true;
		Button clickHandler = new()
		{
			Size = Texture.GetSize(),
			Position = -Texture.GetSize() / 2,
			Flat = true,
			FocusMode = Control.FocusModeEnum.None
        };
		clickHandler.Pressed += Clicked;
		AddChild(clickHandler);

		GetNewRandomTargetPosition();

		CurrentSpeed = RndGen.Next(MIN_SPEED, MAX_SPEED);
        targetSpeed = RndGen.Next(MIN_SPEED, MAX_SPEED);
    }

	public override void _Process(double delta)
	{
        if(!IsRunAway)
		{
            if (waitTime > 0)
            {
                waitTime -= (float)delta;
                Rotation = Mathf.MoveToward(Rotation, 0.1f * IsRotationToRight, (float)delta / 5);
                if (Rotation == 0.1f * IsRotationToRight) IsRotationToRight *= -1;
                return;
            }

            if (IsFliping) return;

            if (CurrentSpeed == targetSpeed) targetSpeed = RndGen.Next(MIN_SPEED, MAX_SPEED);
            else CurrentSpeed = Mathf.MoveToward(CurrentSpeed, targetSpeed, (float)delta);

            Rotation = Mathf.MoveToward(Rotation, 0.1f * IsRotationToRight, (float)delta);
            if (Rotation == 0.1f * IsRotationToRight) IsRotationToRight *= -1;

            if (Position == MoveToThis)
            {
                waitTime = RndGen.Next(50, 150) / 100f;
                GetNewRandomTargetPosition();
            }
            else
            {
                Position = Position.MoveToward(MoveToThis, (float)delta * CurrentSpeed);
            }
        }
		else
		{
            Rotation = Mathf.MoveToward(Rotation, 0.1f * IsRotationToRight, (float)delta);
            if (Rotation == 0.1f * IsRotationToRight) IsRotationToRight *= -1;

            if(!IsFalling)
			{
                Position = Position.MoveToward(MoveToThis, (float)delta * MAX_SPEED);
                if (Position == MoveToThis)
                {
                    MoveToThis = new Vector2I((int)Position.X, Statics.SCREEN_SIZE_Y + 100);
                    IsFalling = true;
                }
            }
            else
            {
                Position = Position.MoveToward(MoveToThis, (float)delta * MAX_SPEED*5);
                if (Position == MoveToThis) QueueFree();
                
            }
        }
    }
	private void GetNewRandomTargetPosition()
	{
        MoveToThis = new Vector2I(RndGen.Next(50, Statics.SCREEN_SIZE_X - 50), RndGen.Next(50, Statics.SCREEN_SIZE_Y - 50));
		Vector2 forDraw = Position - MoveToThis;

		if (forDraw.X < 0 && FlipH) Flip();
		if (forDraw.X > 0 && !FlipH) Flip();
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

	public void Clicked()
	{
		AudioStreamPlayer temp = new()
		{
			Stream = GD.Load<AudioStream>("res://Media/Audio/SFX/Clicked.mp3"),
			Autoplay = true
		};
		AddChild(temp);
		IsRunAway = true;
        MoveToThis = new Vector2I((int)Position.X, (int)Position.Y-100);
    }
}
