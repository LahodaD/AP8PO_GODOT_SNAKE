using Godot;
using System;

public partial class Game : Sprite2D
{
	private int cells = 50;
	private int snakeCellSize = 50;
	
	private System.Collections.Generic.List<Vector2> oldData = new System.Collections.Generic.List<Vector2>();
	private System.Collections.Generic.List<Vector2> snakeData = new System.Collections.Generic.List<Vector2>();
	private System.Collections.Generic.List<Panel> snake = new System.Collections.Generic.List<Panel>();
	
	private Vector2 foodPos;
	private bool regenFood = true;
	private Panel foodItem;
	
	private Vector2 startPos = new Vector2(25, 13);
	private Vector2 up = new Vector2(0, -1);
	private Vector2 down = new Vector2(0, 1);
	private Vector2 left = new Vector2(-1, 0);
	private Vector2 right = new Vector2(1, 0);
	private Vector2 moveDirection;
	
	private bool canMove;
	private bool gameStarted = false;
	
	
	public override void _Ready()
	{
		GetNode<CanvasLayer>("GameOverMenu").Hide();
		GetTree().Paused = false;
		moveDirection = up;
		canMove = true;
		GenerateSnake();
		MoveFood();
		//createFood();
	}
	
	public override void _Process(double delta)
	{
		MoveSnake();
	}
	
	private void GenerateSnake() 
	{
		for(int i = 1; i <= 3; i++) 
		{ 
			AddSegment(startPos + new Vector2(0, i));
			GD.Print(startPos + new Vector2(0, i));
		}
	}
	
	private void AddSegment(Vector2 pos)
	{
		PackedScene snakeScene = (PackedScene)ResourceLoader.Load("res://snake_body.tscn");
		snakeData.Add(pos);
		Panel snakeBody = (Panel)snakeScene.Instantiate();
		snakeBody.Position = (pos * snakeCellSize) + new Vector2(0, snakeCellSize);
		AddChild(snakeBody);
		snake.Add(snakeBody);
	}
	
	private void StartGame() 
	{
		gameStarted = true;
		GetNode<Timer>("MoveTimer").Start();
	}
	
	private void MoveSnake()
	{
		if (canMove)
		{
			// Update movement from keypresses
			if (Input.IsActionJustPressed("move_down") && moveDirection != up)
			{
				GD.Print("MOVE_DOWN");
				moveDirection = down;
				canMove = false;
				if (!gameStarted)
					StartGame();
			}
			if (Input.IsActionJustPressed("move_up") && moveDirection != down)
			{
				GD.Print("MOVE_UP");
				moveDirection = up;
				canMove = false;
				if (!gameStarted)
					StartGame();
			}
			if (Input.IsActionJustPressed("move_left") && moveDirection != right)
			{
				GD.Print("MOVE_LEFT");
				moveDirection = left;
				canMove = false;
				if (!gameStarted)
					StartGame();
			}
			if (Input.IsActionJustPressed("move_right") && moveDirection != left)
			{
				GD.Print("MOVE_RIGHT");
				moveDirection = right;
				canMove = false;
				if (!gameStarted)
					StartGame();
			}
		} else {
			GD.Print("blablablablablabla");
		}
	}
	
	private void OnMoveTimerTimeout()
	{
		// Allow snake movement
		GD.Print("ON_MOVE_TIMER_TIMEOUT");
		canMove = true;
		// Use the snake's previous position to move the segments
		oldData = new System.Collections.Generic.List<Vector2>(snakeData);
		snakeData[0] += moveDirection;
		for (int i = 0; i < snakeData.Count; i++)
		{
			GD.Print(snakeData[i]);
			// Move all the segments along by one
			if (i > 0)
				snakeData[i] = oldData[i - 1];
			snake[i].Position = (snakeData[i] * snakeCellSize) + new Vector2(0, snakeCellSize);
		}
		CheckFoodEaten();
		CheckOutOfBounds();
		CheckSelfEaten();
	}
	
	private void CheckFoodEaten()
	{
		GD.Print("----------------- fooooood --------------");
		GD.Print(foodPos);
		if (snakeData[0] == foodPos)
		{
			//_score++;
			//GetNode<Label>("Hud/ScoreLabel").Text = "SCORE: " + _score;
			AddSegment(oldData[oldData.Count - 1]);
			MoveFood();
		}
	}
	
	private void CheckSelfEaten()
	{
		for (int i = 1; i < snakeData.Count; i++)
		{
			if (snakeData[0] == snakeData[i])
			{
				EndGame();
			}
		}
	}
	
	private void CheckOutOfBounds()
	{
		if (snakeData[0].X < 0 || snakeData[0].X > cells - 1 || snakeData[0].Y < 0 || snakeData[0].Y > 27 - 1)
		{
			EndGame();
		}
	}
	
	private void MoveFood()
	{
		while (regenFood)
		{
			regenFood = false;
			
			Random random = new Random();
			foodPos = new Vector2(random.Next(0, cells - 1), random.Next(0, 27 - 1));
			
			foreach (Vector2 pos in snakeData)
			{
				if (foodPos == pos)
				{
					regenFood = true;
				}
			}
		}
		GetNode<Panel>("Food").Position = (foodPos * snakeCellSize) + new Vector2(0, snakeCellSize);
		regenFood = true;
	}
	
	public void EndGame()
	{
		GetNode<CanvasLayer>("GameOverMenu").Show();
		GetNode<Timer>("MoveTimer").Stop();
		gameStarted = false;
		GetTree().Paused = true;
	}
	
	public void OnGameOverMenuRestart()
	{
		GetNode<CanvasLayer>("GameOverMenu").Hide();
		GetTree().Paused = false;
		moveDirection = up;
		canMove = true;
		GenerateSnake();
		MoveFood();
	}
}
