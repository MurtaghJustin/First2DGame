using Godot;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400;
	public Vector2 ScreenSize;

	private int BorderX { get; set; } = 0;
	private int BorderY { get; set; } = 0;

	[Signal]
	public delegate void HitEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		var sprite = GetSprite();
		var frameNames = sprite.SpriteFrames.GetAnimationNames();

		int width = 0;
		int height = 0;
		
		for (int i = 0; i < frameNames.Length; i++)
		{
			var fn = frameNames[i];
			var texture = sprite.SpriteFrames.GetFrameTexture(fn, 0);
			var textureWidth = (int)Math.Ceiling(texture.GetWidth() * sprite.Scale.X);
			var textureHeight = (int)Math.Ceiling(texture.GetHeight() * sprite.Scale.Y);

			if (textureWidth > width)
				width = textureWidth;
			if (textureHeight > height)
				height = textureHeight;	
		}

		BorderX = (int)Math.Ceiling(width / 2d);
		BorderY = (int)Math.Ceiling(height / 2d);
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var d = (float)delta;
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		var sprite = GetSprite();

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			sprite.Play();
		}
		else
		{
			sprite.Stop();
		}

		if (velocity.X != 0)
		{
			sprite.Animation = "walk";
			sprite.FlipV = false;
			sprite.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			sprite.Animation = "up";
			sprite.FlipH = velocity.Y > 0;
		}
		else
		{
			sprite.Animation = "walk";
		}

		Position += velocity * d;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0 + BorderX, ScreenSize.X - BorderX),
			y: Mathf.Clamp(Position.Y, 0 + BorderY, ScreenSize.Y - BorderY)
		);
	}

	private void OnBodyEntered(NoiseTexture2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetCollision().SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetCollision().Disabled = false;
	}

	private CollisionShape2D GetCollision()
	{
		return GetNode<CollisionShape2D>("CollisionShape2D");
	}

	private AnimatedSprite2D GetSprite()
	{
		return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
}
