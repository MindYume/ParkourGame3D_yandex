using Godot;

public class SensitivityPanel : PopupPanel
{
    private HSlider _hSlider;
    private bool _isOpen;
    private Label _label;
    private GameState _gameState;
    [Signal] public delegate void sensitivity_changed(double sensitivity);

    public override void _Ready()
    {
        _gameState = GetNode<GameState>("/root/GameState");

        _hSlider = GetNode<HSlider>("AspectRatioContainer/HSlider");
        _label =  GetNode<Label>("AspectRatioContainer/Label");
        GetNode<GameState>("/root/GameState").GetSensitivityPanel(this);

        Close();
    }

    public override void _Process(float delta)
    {
        Camera mainCamera = GetViewport().GetCamera();
        float rZ = mainCamera.RotationDegrees.z;

        if (rZ == 0)
        {
            RectScale = new Vector2(OS.WindowSize.x / 1024, OS.WindowSize.x / 1024);
            RectPosition = new Vector2(OS.WindowSize.x / 4, OS.WindowSize.y / 4);
            RectRotation = 0;
        }
        else
        {
            RectScale = new Vector2(OS.WindowSize.y / 1024, OS.WindowSize.y / 1024);
            RectPosition = new Vector2(OS.WindowSize.x / 4, OS.WindowSize.y * 3 / 4);
            RectRotation = rZ;
        }


        if (_gameState.lang == "ru")
        {
            _label.Text = "Чувствительность";
        }
        else
        {
            _label.Text = "Sensitivity";
        }
    }

    public void _on_Button_button_up()
    {
        Close();
    }
    

    public void Open()
    {
        PauseMode = PauseModeEnum.Process;
        Visible = true;
        _hSlider.Editable = true;
        _isOpen = true;
    }

    public void Close()
    {
        EmitSignal(nameof(sensitivity_changed), _hSlider.Value);
        PauseMode = PauseModeEnum.Stop;
        Visible = false;
        _hSlider.Editable = false;
        _isOpen = false;
    }

    public bool IsOpen => _isOpen;
}
