using Godot;

namespace MessyAIPlugin.MessyAI;

public partial class MetaUploadDialog : Window
{
    private Control _dialog;
    
    public override void _EnterTree()
    {
        
        Size = new Vector2I(500, 500);
        _dialog = GD.Load<PackedScene>("res://addons/MessyAI/MetasUploadDialog.tscn").Instantiate<Control>();
        AddChild(_dialog);
    }

    public override void _ExitTree()
    {
        RemoveChild(_dialog);
    }
}