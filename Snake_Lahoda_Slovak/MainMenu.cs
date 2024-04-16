using Godot;
using System;

public partial class MainMenu : ColorRect
{
	public void PlayGame() {
		GetNode<SceneLoader>("/root/SceneLoader").ChangeToScene("game.tscn");
	}
	
	public void ExitGame() {
		GetTree().Quit();
	}
}
