using Godot;

public class CheckPoint : Spatial
{
    private Position3D _respawnPosition;
    public Vector3 RespawnPosition
    {
        get => _respawnPosition.GlobalTranslation;
    }

    public override void _Ready()
    {
        _respawnPosition = GetNode<Position3D>("RespawnPosition");
        
    }
}
