using Godot;

public class Player : KinematicBody
{
	[Export] private float _mouseSensitivity = 0.1f;
	private int _jumpsAmount = 2;
	private int _jumpStrength = 15;
	private float _verticalVelocity = 0;
	private bool _canJump = false;
	private bool _isOnFloor = false;
	private float _gravitation = 30;
	private float _lastStepTime = 0;
	private float _lastSaveTime = 0;
	private Vector3 _respawnPosition = new Vector3(0, 5, 0);
	private float _moveAcceleration= 80;
	private float _maxMoveVelocity = 10;
	private Vector3 _moveVelocity = Vector3.Zero;
	private Vector2 _stickDirection;
	private Timer _timer;
	private Camera _camera;
	private Area _area;
	private Sounds sounds;
	private bool _isUnderPlatform = false;

	[Signal] public delegate void exited_spawn();
	[Signal] public delegate void entered_spawn();
	[Signal] public delegate void jumps_amount_changed(int jumpsAmount);

	private JavaScriptObject _loadPlayerDataCallback;
	private JavaScriptObject javascriptWidnow;

	private int JumpsAmount
	{
		set 
		{
			_jumpsAmount = value;
			EmitSignal(nameof(jumps_amount_changed), _jumpsAmount);
		}

		get => _jumpsAmount;
	}
	
	private void loadPlayerData(JavaScriptObject[] arr)
	{
		if (arr[0] != null)
		{
			string player_data = arr[0].ToString();
			string[] components = player_data.Split(" ");

			float x = float.Parse(components[0]);
			float y = float.Parse(components[1]);
			float z = float.Parse(components[2]);
			Translation = new Vector3(x, y, z); 

			x = float.Parse(components[3]);
			y = float.Parse(components[4]);
			z = float.Parse(components[5]);
			_respawnPosition = new Vector3(x, y, z);

			_timer.Time = float.Parse(components[6]);
			_timer.IsTimerStarted = bool.Parse(components[7]);
		}
		_area.Monitoring = true;
	}

	private void savePlayerData()
	{
		string dataString = "";
		dataString = $"{Translation.x} {Translation.y} {Translation.z} ";
		dataString += $"{_respawnPosition.x} {_respawnPosition.y} {_respawnPosition.z} ";
		dataString += $"{_timer.Time} ";
		dataString += $"{_timer.IsTimerStarted}";

		javascriptWidnow.Call("localStorageSetItem", "player_data", dataString);
	}

	public override void _Ready()
	{
		sounds = GetNode<Sounds>("/root/Sounds");
		_camera = GetNode<Camera>("Camera");
		_area = GetNode<Area>("Area");

		_area.Connect("area_entered", this, "_on_area_entered");
		_area.Connect("area_exited", this, "_on_area_exited");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;

		if (OS.GetName() == "HTML5")
		{
			_area.Monitoring = false;

			_loadPlayerDataCallback = JavaScript.CreateCallback(this, nameof(loadPlayerData));
			javascriptWidnow = JavaScript.GetInterface("window");
			javascriptWidnow.Call("localStorageGetItem", "player_data", _loadPlayerDataCallback);
		}

	}
	
	
	public override void _PhysicsProcess(float delta)
	{
		HandleMoveControl(delta);
		HandleJumpsControl(delta);

		if (_isUnderPlatform)
		{
			Translation += Vector3.Down * 0.05f;
		}
		
		_lastSaveTime += delta;
		if (OS.GetName() == "HTML5" && _lastSaveTime > 1 && IsOnFloor())
		{
			_lastSaveTime = 0;
			savePlayerData();
		}

		// Remove on release
		if(Input.IsKeyPressed(((int)KeyList.R)))
		{
			Translation = new Vector3(0, 2.5f, 0);
		}
	}

	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{	
			Vector2 mouseMotionRotated = mouseMotion.Relative.Rotated(-Mathf.Deg2Rad(_camera.RotationDegrees.z));

			_camera.RotationDegrees += (new Vector3(-mouseMotionRotated.y, 0, 0))* _mouseSensitivity;
			float rX = _camera.RotationDegrees.x;
			float rY = _camera.RotationDegrees.y;
			float rZ = _camera.RotationDegrees.z;
			_camera.RotationDegrees = new Vector3(Mathf.Clamp(rX, -90, 90), rY, rZ);

			RotationDegrees += (new Vector3(0, -mouseMotionRotated.x, 0)) * _mouseSensitivity;
		}
	}

	private void HandleMoveControl(float delta)
	{
		Vector2 inputVector = Vector2.Zero;
		inputVector = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"), Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")).LimitLength(1);

		if (_stickDirection != Vector2.Zero)
		{
			inputVector = _stickDirection;
		}

		inputVector = inputVector.Rotated(-Mathf.Deg2Rad(RotationDegrees.y));

		_moveVelocity = MoveAndSlide(_moveVelocity, Vector3.Up);
		_moveVelocity.y = 0;

		_lastStepTime += delta;
		if (inputVector != Vector2.Zero )
		{
			_moveVelocity += new Vector3(inputVector.x, 0 ,inputVector.y) * _moveAcceleration * delta;
			_moveVelocity = _moveVelocity.LimitLength(_maxMoveVelocity);
			if (_lastStepTime > 0.3 && _isOnFloor && _verticalVelocity <= 0)
			{
				sounds.step.Play();
			 	_lastStepTime = 0;
			}
		}
		else
		{
			_moveVelocity = _moveVelocity.MoveToward(Vector3.Zero, _moveAcceleration * delta);
		}
	}

	private void HandleJumpsControl(float delta)
	{
		Vector3 linearVelocity = MoveAndSlide(new Vector3(0, _verticalVelocity, 0), Vector3.Up);
		_verticalVelocity  = linearVelocity.y -_gravitation * delta;
		_isOnFloor = IsOnFloor();

		if (IsOnFloor() && _verticalVelocity <= 0)
		{
			if (JumpsAmount != 2)
			{
				sounds.step.Play();
				_lastStepTime = 0;
			}

			_canJump = true;
			JumpsAmount = 2;
		}

		if (Input.IsActionJustReleased("jump"))
		{
			if (JumpsAmount > 0 )
			{
				_canJump = true;
			}
			else
			{
				_canJump = false;
			}
		}

		if (Input.IsActionPressed("jump"))
		{
			if (_canJump)
			{
				_verticalVelocity = _jumpStrength;
				JumpsAmount--;
				_canJump = false;
				if (JumpsAmount == 1)
				{
					sounds.jump1.Play();
				}
				else
				{
					sounds.jump2.Play();
				}
			}
		}
	}

	private void Respawn()
	{
		Translation = _respawnPosition;
	}

	private void SetRespawnPosition(Area _CheckpointArea)
	{
		CheckPoint checkpoint = (CheckPoint)_CheckpointArea.GetParent();

		if (_respawnPosition != checkpoint.RespawnPosition)
		{
			sounds.checkpoint.Play();
		}

		_respawnPosition = checkpoint.RespawnPosition;

		if (OS.GetName() == "HTML5")
		{
			_lastSaveTime = 0;
		}
	}

	public void _on_Timer_loaded(Timer timer)
	{
		_timer = timer;
	}

	public void _on_area_entered(Area area)
	{
		switch(area.CollisionLayer)
		{
			case 4: // Checkpoint
				SetRespawnPosition(area);
				break;
			
			case 8: // Lava
				Respawn();
				break;
			
			case 16: // PlatformBotttom
				_isUnderPlatform = true;
				break;
			
			case 32: // StartDetectArea
				EmitSignal(nameof(entered_spawn));
				break;
		}
	}

	public void _on_area_exited(Area area)
	{
		switch(area.CollisionLayer)
		{      
			case 16: // PlatformBotttom
				_isUnderPlatform = false;
				break;
			
			case 32: // StartDetectArea
				EmitSignal(nameof(exited_spawn));
				break;
		}
	}

	public void _on_SensitivityPanel_sensitivity_changed(double sensitivity)
	{
		_mouseSensitivity = (float)sensitivity;
	}

	public void _on_JumpButton_button_up()
	{
		if (JumpsAmount > 0 )
		{
			_canJump = true;
		}
		else
		{
			_canJump = false;
		}
	}

	public void _on_JumpButton_button_down()
	{
		if (_canJump)
		{
			_verticalVelocity = _jumpStrength;
			JumpsAmount--;
			_canJump = false;
			if (JumpsAmount == 1)
			{
				sounds.jump1.Play();
			}
			else
			{
				sounds.jump2.Play();
			}
		}

		GD.Print("Pressed");
	}

	public void _on_TouchGamepadControl_screen_drag(Vector2 dragDirection)
	{
		_stickDirection = dragDirection;
	}
}
