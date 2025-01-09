using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GameOver()
	{
		GD.Print($"Game Over: {_score}");
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	public void NewGame()
	{
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	public void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	public void OnScoreTimerTimeout()
	{
		_score++;
	}

	public void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();

		var spawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		spawnLocation.ProgressRatio = GD.Randf();

		float dir = spawnLocation.Rotation + Mathf.Pi / 2;
		mob.Position = spawnLocation.Position;
		dir += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = dir;

		var velocity = new Vector2((float)GD.RandRange(150d, 250d), 0);
		mob.LinearVelocity = velocity.Rotated(dir);

		AddChild(mob);
	}
}
