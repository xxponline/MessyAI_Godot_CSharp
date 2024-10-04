using Godot;

namespace MessyAIPlugin.MessyAI;

public static class NodeExtend
{
    public static void ClearChildren(this Node node)
    {
        foreach (var child in node.GetChildren())
        {
            node.RemoveChild(child);
        }
    }
}