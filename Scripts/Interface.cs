using Godot;

public class Interface : Control
{
	private Label _timeLabel;
	private Label _jumpsAmountLabel;

	private Control _dynamicRect;
	private Control _touchPanel;

	private GameState _gameState;

	public override void _Ready()
	{
		_gameState = GetNode<GameState>("/root/GameState");

		_timeLabel = GetNode<Label>("DynamicRect/TimeLabel");
		_jumpsAmountLabel = GetNode<Label>("DynamicRect/JumpsAmountLabel");

		_dynamicRect = GetNode<Control>("DynamicRect");
		_dynamicRect.RectSize = new Vector2(1024, 600);
	}

	private float TimeLabelValue
	{
		set 
		{
			if (_gameState.lang == "ru")
			{
				_timeLabel.Text = "Время: ";
			}
			else
			{
				_timeLabel.Text = "Time: ";
			}

			_timeLabel.Text = _timeLabel.Text + value.ToString();
		}
	}

	private int JumpsAmountLabelValue
	{
		set 
		{
			if (_gameState.lang == "ru")
			{
				_jumpsAmountLabel.Text = "Прыжки: ";
			}
			else
			{
				_jumpsAmountLabel.Text = "Jumps: ";
			}

			_jumpsAmountLabel.Text = _jumpsAmountLabel.Text + value.ToString();
		}
	}

	public override void _Process(float delta)
	{	
		_dynamicRect.RectScale = new Vector2(OS.WindowSize.x / 1024, OS.WindowSize.x / 1024);
	}

	private void RotateInterface(float rotationDegree)
	{
		Camera mainCamera = GetViewport().GetCamera();
		float rX = mainCamera.RotationDegrees.x;
		float rY = mainCamera.RotationDegrees.y;

		mainCamera.RotationDegrees = new Vector3(rX, rY, rotationDegree);
		_dynamicRect.RectRotation = rotationDegree;

		if (rotationDegree == 0)
		{
			_dynamicRect.RectPosition = new Vector2(0, 0);
		}
		else
		{
			_dynamicRect.RectPosition = new Vector2(0, OS.WindowSize.y);
		}
	}

	public void _on_Timer_time_changed(float time)
	{
		TimeLabelValue = Mathf.Ceil(time * 100) / 100;
	}

	public void _on_Player_jumps_amount_changed(int jumps)
	{
		JumpsAmountLabelValue = jumps;
	}


	public void _on_SensitivityButton_button_up()
	{
		_gameState.OpenSensitivityPanel();
	}
	
	public void _on_LicenceButton_button_up()
	{
		_gameState.OpenLicensePanel();
	}

}
