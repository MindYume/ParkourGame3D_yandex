using Godot;

public class Sounds : Node
{
    public AudioStreamPlayer jump1;
    public AudioStreamPlayer jump2;
    public AudioStreamPlayer checkpoint;
    public AudioStreamPlayer win;
    public AudioStreamPlayer step;

    public override void _Ready()
    {
        jump1 = GetNode<AudioStreamPlayer>("Jump1");
        jump2 = GetNode<AudioStreamPlayer>("Jump2");
        checkpoint = GetNode<AudioStreamPlayer>("Checkpoint");
        win = GetNode<AudioStreamPlayer>("Win");
        step = GetNode<AudioStreamPlayer>("Step");
    }

}
