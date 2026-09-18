using Godot;

public interface IInteractable
{
    // Concrete nodes implement this; world facts belong to WorldState.
    void Interact(Node interactor);
}
