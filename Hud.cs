using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public Timer MessageTimer { get; set; }
	public Label Message { get; set; }
	public Button StartButton { get; set; }
	public Label ScoreLabel { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MessageTimer = GetNode<Timer>("MessageTimer");
		Message = GetNode<Label>("Message");
		ScoreLabel = GetNode<Label>("ScoreLabel");
		StartButton = GetNode<Button>("StartButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowMessage(string text, bool timeout = true)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		if (timeout)
			MessageTimer.Start();
	}

	public async void ShowGameOver()
	{
		ShowMessage("Game Over");

		await ToSignal(MessageTimer, Timer.SignalName.Timeout);

		ShowMessage("Dodge the Creeps!", false);

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		StartButton.Show();
	}

	public void UpdateScore(int score)
	{
		ScoreLabel.Text = score.ToString();
	}

	private void OnStartButtonPressed()
	{
		StartButton.Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout()
	{
		Message.Hide();
	}
}
