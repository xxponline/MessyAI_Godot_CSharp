using Godot;

namespace MessyAIPlugin.MessyAI;

public static class GodotObjectExtend
{
    public static void Disconnect(this GodotObject godotObject, StringName signal)
    {
        foreach (var s in godotObject.GetSignalConnectionList(signal))
        {
            godotObject.Disconnect(signal, s["callable"].As<Callable>());
        }
    }
}