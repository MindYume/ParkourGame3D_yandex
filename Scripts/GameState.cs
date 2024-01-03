using Godot;
public class GameState : Node
{
    public string lang = "en";
    private float language_last_check_time = 0;

    private GameStateEnum _state = GameStateEnum.Playing;
    private SensitivityPanel _sensitivityPanel;
    private LicenseAndCreditsPanel _licenseAndCreditsPanel;

    private enum GameStateEnum
    {
        Playing,
        Pause,
        SensitivityPanel,
        LicenseAndCreditsPanel
    }

    public override void _Ready()
    {
        /* if (OS.GetName() == "HTML5")
        {
            GDScript MyGDScript = (GDScript)GD.Load("res://Scripts/YandexGamesAPI.gd");
            lang = MyGDScript.Call("get_language").ToString();
        } */
    }

    public override void _Process(float delta)
    {
        switch(_state)
        {
            case GameStateEnum.Playing:
                PlayingStateProcess();
                break;

            case GameStateEnum.Pause:
                PauseStateProcess();
                break;
            
            case GameStateEnum.SensitivityPanel:
                SensitivityPanelStateProcess();
                break;

            case GameStateEnum.LicenseAndCreditsPanel:
                LicenseAndCreditsPanelProcess();
                break;
        }

        language_last_check_time += delta;
        if (language_last_check_time > 1)
        {
            language_last_check_time = 0;
            /* if (OS.GetName() == "HTML5")
            {
                GDScript MyGDScript = (GDScript)GD.Load("res://Scripts/YandexGamesAPI.gd");
                lang = MyGDScript.Call("get_language").ToString();
            } */
        }
    }

    private void PlayingStateProcess()
    {
        if (Input.IsActionPressed("ui_select"))
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if (Input.IsActionPressed("ui_cancel"))
        {
            GetTree().Paused = true;
            PauseMode = PauseModeEnum.Process;
            Input.MouseMode = Input.MouseModeEnum.Visible;
            _state = GameStateEnum.Pause;
        }

        if (Input.IsActionJustReleased("sensitivity_panel"))
        {
            OpenSensitivityPanel();
        }

        if (Input.IsActionJustReleased("license_and_credits_panel") && false)
        {
            OpenLicensePanel();
        }
    }

    private void PauseStateProcess()
    {
        if (Input.IsActionPressed("ui_select"))
        {
            GetTree().Paused = false;
            Input.MouseMode = Input.MouseModeEnum.Captured;
            _state = GameStateEnum.Playing;
        }
    }

    private void SensitivityPanelStateProcess()
    {
        if(!_sensitivityPanel.IsOpen)
        {
            GetTree().Paused = false;
            Input.MouseMode = Input.MouseModeEnum.Captured;
            _state = GameStateEnum.Playing;
        }
    }

    private void LicenseAndCreditsPanelProcess()
    {
        if(!_licenseAndCreditsPanel.IsOpen)
        {
            GetTree().Paused = false;
            Input.MouseMode = Input.MouseModeEnum.Captured;
            _state = GameStateEnum.Playing;
        }
    }

    public void GetSensitivityPanel(SensitivityPanel sensitivityPanel)
    {
        _sensitivityPanel = sensitivityPanel;
    }

    public void GetLicenseAndCreditsPanel(LicenseAndCreditsPanel licenseAndCreditsPanel)
    {
        _licenseAndCreditsPanel = licenseAndCreditsPanel;
    }

    public void OpenSensitivityPanel()
    {
        GetTree().Paused = true;
        PauseMode = PauseModeEnum.Process;
        _sensitivityPanel.PauseMode = PauseModeEnum.Process;
        _sensitivityPanel.Open();
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _state = GameStateEnum.SensitivityPanel;
    }

    public void OpenLicensePanel()
    {
        GetTree().Paused = true;
        PauseMode = PauseModeEnum.Process;
        _licenseAndCreditsPanel.PauseMode = PauseModeEnum.Process;
        _licenseAndCreditsPanel.Open();
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _state = GameStateEnum.LicenseAndCreditsPanel;
    }
}
