using Godot;
using System;

public partial class Mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var sprite = GetSprite();
		var mobTypes = sprite.SpriteFrames.GetAnimationNames();
		sprite.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	private AnimatedSprite2D GetSprite()
	{
		return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private CollisionShape2D GetCollision()
	{
		return GetNode<CollisionShape2D>("CollisionShape2D");
	}
}
