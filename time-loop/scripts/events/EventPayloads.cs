// IDs identify participants; GameTime is elapsed simulation seconds.
public readonly record struct DoorOpenedEvent(
    string DoorId,
    string OpenedById,
    double GameTime
);

public readonly record struct ItemTakenEvent(
    string ItemId,
    string TakenById,
    string PreviousOwnerId,
    double GameTime
);
