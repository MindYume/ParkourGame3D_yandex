using Godot;

public class WinPlatform : Spatial
{
    private Label3D _label;
    private string win_text_eng_file = "res://Texts/win_information_eng_simple.txt";
    private string win_text_rus_file = "res://Texts/win_information_rus_simple.txt";
    private string win_text = "";
    private float _timerTime;
    private float _timerBestTime;
    private GameState _gameState;
    
    [Signal] public delegate void loaded(Vector3 teleport_position);

    public override void _Ready()
    {
        _gameState = GetNode<GameState>("/root/GameState");

        File txt_file = new File();
        if (_gameState.lang == "ru")
        {
            txt_file.Open(win_text_rus_file, File.ModeFlags.Read);
        }
        else
        {
            txt_file.Open(win_text_eng_file, File.ModeFlags.Read);
        }
        win_text = txt_file.GetAsText();

        

        _label = GetNode<Label3D>("WinInformation/Text");

        Vector3 teleport_position = GetNode<Position3D>("TeleportPosition").GlobalTranslation;
        EmitSignal(nameof(loaded), teleport_position);
    }

    public void _on_Timer_time_changed(float time)
    {
        _timerTime = Mathf.Ceil(time*100)/100;
        _label.Text = string.Format(win_text, _timerTime, _timerBestTime);
    }

    public void _on_Timer_best_time_changed(float bestTime)
    {
        _timerBestTime = Mathf.Ceil(bestTime*100)/100;
        _label.Text = string.Format(win_text, _timerTime, _timerBestTime);
    }

    public void _on_RestartPlatform_area_entered(Area area)
    {
        Player player = area.GetParent<Player>();
        player.Translation = new Vector3(0, 2, 0);
        player.RotationDegrees = new Vector3(0, -45, 0);

        if (OS.GetName() == "HTML5")
		{
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GDScript MyGDScript = (GDScript)GD.Load("res://Scripts/YandexGamesAPI.gd");
            MyGDScript.Call("show_ad");
        }
    }
}
