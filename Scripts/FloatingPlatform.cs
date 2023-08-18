using Godot;
using System;

public class FloatingPlatform : Spatial
{
    [Export] private int _radius;
    [Export] float _rotation;
    [Export] private float _rotationSpeed;

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        _rotation += _rotationSpeed * delta;
        if (_rotation > Mathf.Pi * 2)
        {
            _rotation -= Mathf.Pi * 2;
        }

        float x = Translation.x;
        float y = Mathf.Sin(_rotation) * _radius;
        float z = Translation.z;
        Translation = new Vector3(x, y, z);
    }

}
