using Godot;

public class Instructions : Spatial
{
    private Label3D _label;
    private string instructions_text_eng_file = "res://Texts/instructions_eng.txt";
    private string instructions_text_rus_file = "res://Texts/instructions_rus.txt";
    private GameState _gameState;
    

    public override void _Ready()
    {
        _gameState = GetNode<GameState>("/root/GameState");
        _label = GetNode<Label3D>("Text");

        File txt_file = new File();
        if (_gameState.lang == "ru")
        {
            txt_file.Open(instructions_text_rus_file, File.ModeFlags.Read);
        }
        else
        {
            txt_file.Open(instructions_text_eng_file, File.ModeFlags.Read);
        }
        _label.Text = txt_file.GetAsText();
    }

}
