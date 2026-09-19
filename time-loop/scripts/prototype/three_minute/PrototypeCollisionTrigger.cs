using Godot;
using System;

public partial class PrototypeCollisionTrigger : Area2D
{
    public event Action? CourierCollision;
    private bool _danielInside, _jonahInside, _sent;
    public override void _Ready()
    {
        BodyEntered += body => { if (body.Name == "Daniel") _danielInside = true; if (body.Name == "Jonah") _jonahInside = true; Check(); };
        BodyExited += body => { if (body.Name == "Daniel") _danielInside = false; if (body.Name == "Jonah") _jonahInside = false; };
    }
    private void Check() { if (_danielInside && _jonahInside && !_sent) { _sent = true; CourierCollision?.Invoke(); } }
    public override void _ExitTree() { _sent = false; }
}
