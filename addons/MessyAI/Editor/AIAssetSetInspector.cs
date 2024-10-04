using Godot;

namespace MessyAIPlugin.MessyAI;

[Tool]
public partial class AIAssetSetInspector : EditorInspectorPlugin
{
    private Button _btnImportAIAssetSet;
    
    public override bool _CanHandle(GodotObject @object)
    {
        return @object is AIScriptAssetSet;
    }

    public override void _ParseBegin(GodotObject @object)
    {   
        base._ParseBegin(@object);
        
        _btnImportAIAssetSet = new Button();
        _btnImportAIAssetSet.Text = "Import AI";
        _btnImportAIAssetSet.Pressed += OnImportAIPress;
        AddCustomControl(_btnImportAIAssetSet);
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString,
        PropertyUsageFlags usageFlags, bool wide)
    {   
        return base._ParseProperty(@object, type, name, hintType, hintString, usageFlags, wide);
    }

    public override void _ParseEnd(GodotObject @object)
    {
        base._ParseEnd(@object);
    }

    private void OnImportAIPress()
    {
        GD.Print("11111");
    }
}