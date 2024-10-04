using Godot;

namespace MessyAIPlugin.MessyAI;

[Tool]
public partial class BehaviourTreeScriptAsset: GodotObject
{
    [Export] public string AssetName;
    [Export] public string AssetId;
    [Export] public string AssetVersion;
            
    [Export] public string BehaviourTreeNodes;
    [Export] public string BehaviourTreeDescriptors;
    [Export] public string BehaviourTreeServices;
}

[Tool]
[GlobalClass]
public partial class AIScriptAssetSet : Resource
{
    [Export] public string ScriptAssetSetName;
    [Export] public string ScriptAssetSetId;
    [Export] public BehaviourTreeScriptAsset[] AllBehaviourTreeAssets { get; set; }
}