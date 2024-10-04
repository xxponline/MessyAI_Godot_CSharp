using Godot;

namespace MessyAIPlugin.MessyAI;

[Tool]
public partial class AIAssetSetInspector : EditorInspectorPlugin
{
    private Button _btnImportAIAssetSet;
    private AIAssetsImportDialog _importDialog;
    
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
        switch (name)
        {
            case "ScriptAssetSetName":
            {
                var label = new Label();
                label.Text = $"ScriptAssetSetName : {((AIScriptAssetSet)@object).ScriptAssetSetName}";
                AddCustomControl(label);
            }
                return true;
            case "ScriptAssetSetId":
            {
                var label = new Label();
                label.Text = $"ScriptAssetSetId : {((AIScriptAssetSet)@object).ScriptAssetSetId}";
                AddCustomControl(label);
            }
                return true;
            case "AllBehaviourTreeAssets":
                return false;
        }
        return false;
    }

    public override void _ParseEnd(GodotObject @object)
    {
        base._ParseEnd(@object);
    }

    private void OnImportAIPress()
    {
        AIAssetsImportDialog.OpenDialog();
    }
}