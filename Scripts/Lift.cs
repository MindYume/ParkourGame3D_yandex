using Godot;
using System.Collections.Generic;

public class Lift : Spatial
{
    [Export] private bool _up = true;
    [Export] private int _speed = 10;
    private StaticBody[] _platforms;

    public override void _Ready()
    {
        var _children = GetChildren();
        _platforms = new StaticBody[_children.Count];
        int i = 0;
        foreach(StaticBody platform in _children)
        {
            _platforms[i] = platform;
            i++;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        for(int i = 0; i < _platforms.Length; i++)
        {
            if (_up)
            {
                _platforms[i].Translate(Vector3.Up * _speed * delta);
                if (_platforms[i].Translation.y >= 25 )
                {
                    _platforms[i].Translation = Vector3.Zero;
                }
            }
            else
            {
                //_platforms[i].Translate(Vector3.Down * _speed * delta);
                _platforms[i].Translation += (Vector3.Down * _speed * delta);
                if (_platforms[i].Translation.y <= 0 )
                {
                    _platforms[i].Translation = Vector3.Up * 25;
                }
            }
        }

        //GD.Print(_platforms[4].Translation.y);
    }
}
