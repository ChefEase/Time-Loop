using Godot;
using YarnSpinnerGodot;

// Yarn reads persistent knowledge through this bridge. KnowledgeManager remains the source of truth.
public static class YarnGameFunctions
{
    [YarnFunction("knows")]
    public static bool Knows(string knowledgeId)
    {
        KnowledgeManager? manager = KnowledgeManager.Instance;
        if (manager == null)
        {
            GD.PushError("Yarn function 'knows' could not find KnowledgeManager.");
            return false;
        }
        return manager.Knows(knowledgeId);
    }
}
