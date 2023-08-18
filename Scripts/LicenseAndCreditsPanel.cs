using Godot;

public class LicenseAndCreditsPanel : PopupPanel
{
    private RichTextLabel _richTextLabel;
    private bool _isOpen;
    private string license_text_file = "res://LICENSE.txt";
    private string credts_text_file = "res://Credits.txt";
    private string license_text = "";
    private string credits_text = "";

    public override void _Ready()
    {   
        File txt_file = new File();

        txt_file.Open(license_text_file, File.ModeFlags.Read);
        license_text = txt_file.GetAsText();

        txt_file.Open(credts_text_file, File.ModeFlags.Read);
        credits_text = txt_file.GetAsText();


        _richTextLabel = GetNode<RichTextLabel>("VSplitContainer/RichTextLabel");
        _richTextLabel.Text = "LICENCE\n\n" + license_text + "\n\n\nCREDITS\n\n" + credits_text;

        GetNode<GameState>("/root/GameState").GetLicenseAndCreditsPanel(this);
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
    }

    public void _on_Button_button_up()
    {
        Close();
    }

    public void Open()
    {
        PauseMode = PauseModeEnum.Process;
        Visible = true;
        _isOpen = true;
    }

    public void Close()
    {
        PauseMode = PauseModeEnum.Stop;
        Visible = false;
        _isOpen = false;
    }

    public bool IsOpen => _isOpen;
}
