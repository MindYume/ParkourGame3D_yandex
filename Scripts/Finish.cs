using Godot;

public class Finish : Spatial
{
    [Signal] public delegate void player_reached_finish();
    private Vector3 _teleportPosition = Vector3.Zero;
    private Sounds sounds;
    private Label3D _label3d;
    private GameState _gameState;

    public override void _Ready()
    {
        _gameState = GetNode<GameState>("/root/GameState");
        sounds = GetNode<Sounds>("/root/Sounds");
        _label3d = GetNode<Label3D>("Label3D");
    }

    public override void _Process(float delta)
    {
        if (_gameState.lang == "ru")
        {
            _label3d.Text = "Финиш";
        }
        else
        {
            _label3d.Text = "Finish";
        }
    }

    public void _on_WinPlatform_loaded(Vector3 teleportPosition)
    {
        _teleportPosition = teleportPosition;
    }

    public void _on_PlayerDetectArea_area_entered(Area area)
    {
        EmitSignal(nameof(player_reached_finish));

        switch(area.CollisionLayer)
        {
            case 2: // Teleport player to win platform
                Player player = area.GetParent<Player>();
                player.Translation = _teleportPosition;
                sounds.win.Play();
                break;
        }
    }
}
