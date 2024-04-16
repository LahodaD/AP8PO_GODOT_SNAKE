using Godot;
using System;

public partial class GameOverMenu : CanvasLayer
{	
	private void _on_restart_button_pressed()
	{
		GetNode<SceneLoader>("/root/SceneLoader").ChangeToScene("game.tscn");
	}
	
	// Definice události (signalu)
	[Signal]
	public delegate void RestartEventHandler();

	// Metoda pro obsluhu události
	private void _OnRestartButtonPressed()
	{
		// Emitování události
		EmitSignal(nameof(RestartEventHandler));
	}
}
