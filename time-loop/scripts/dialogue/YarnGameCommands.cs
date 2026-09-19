using Godot;
using YarnSpinnerGodot;

// Yarn can grant knowledge, but it cannot own or duplicate the knowledge store.
public static class YarnGameCommands
{
    [YarnCommand("learn")]
    public static void Learn(string knowledgeId)
    {
        KnowledgeManager? manager = KnowledgeManager.Instance;
        if (manager == null)
        {
            GD.PushError("Yarn command 'learn' could not find KnowledgeManager.");
            return;
        }
        manager.Learn(knowledgeId);
    }
}
