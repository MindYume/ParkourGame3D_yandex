using Godot;

public class Timer: Node
{
    private float _time = 0;
    private float _bestTime = float.MaxValue;
    private bool _isTimerStarted;

    [Signal] public delegate void time_changed(float time);
    [Signal] public delegate void best_time_changed(float bestTime);
    [Signal] public delegate void loaded(Timer timer);


    public override void _Ready()
	{
		EmitSignal(nameof(loaded), this);
	}

    public override void _Process(float delta)
    {
        if (_isTimerStarted)
        {
            Time += delta;
        }
    }

    public float Time
    {
        set 
        {
            _time = value;
            EmitSignal(nameof(time_changed), _time);
        }

        get => _time;
    }

    public bool IsTimerStarted
    {
        set
        {
            _isTimerStarted = value;
        }

        get => _isTimerStarted;
    }

    public void _on_Player_entered_spawn()
    {
        _isTimerStarted = false;
        Time = 0;
    }

    public void _on_Player_exited_spawn()
    {
        _isTimerStarted = true;
    }

    public void _on_Finish_player_reached_finish()
    {
        _isTimerStarted = false;
        
        if (_time < _bestTime)
        {
            _bestTime = _time;
            EmitSignal(nameof(best_time_changed), _bestTime);
        }
    }

}