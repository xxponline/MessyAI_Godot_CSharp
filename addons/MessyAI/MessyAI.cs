#if TOOLS
using System.IO;
using Godot;
using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

[Tool]
public partial class MessyAI : EditorPlugin
{
	private MessyAISettingsDeck _customDock;

	private AIAssetSetInspector _assetSetInspector;
	
	public override void _EnterTree()
	{
		GD.Print("My Plugin has been loaded!!");

		if (!ProjectSettings.HasSetting("MessyAIConfigure/BackendUrl"))
		{
			ProjectSettings.SetSetting("MessyAIConfigure/BackendUrl", "http://localhost:8080");
		}

		if (!ProjectSettings.HasSetting("MessyAIConfigure/SelectedSolutionId"))
		{
			ProjectSettings.SetSetting("MessyAIConfigure/SelectedSolutionId", "");
		}
		
		
		_assetSetInspector = new AIAssetSetInspector();
		AddInspectorPlugin(_assetSetInspector);

		//_customDock = new MessyAISettingsDeck();
		//_customDock = GD.Load<PackedScene>("res://addons/MessyAI/MessyAISettingsDeck.tscn")
		//	.Instantiate<MessyAISettingsDeck>();
		//_customDock.Name = "MessyAIDeck";
		//_customDock.AddChild(GD.Load<PackedScene>("res://addons/MessyAI/MessyAISettingsDeck.tscn").Instantiate<MessyAISettingsDeck>());
		//_customDock.Initialize();
		
		
		_customDock = GD.Load<PackedScene>("res://addons/MessyAI/MessyAISettingsDeck.tscn")
			.Instantiate<MessyAISettingsDeck>();
		_customDock.Name = "MessyAIDeck";
		AddControlToDock(DockSlot.LeftBr, GD.Load<PackedScene>("res://addons/MessyAI/MessyAISettingsDeck.tscn").Instantiate<MessyAISettingsDeck>());
	}

	public override void _ExitTree()
	{
		GD.Print("My Plugin has been unloaded!!");
		
		RemoveInspectorPlugin(_assetSetInspector);
		
		//_customDock.UnInitialize();
		RemoveControlFromDocks(_customDock);
		_customDock.Free();
		_customDock = null;
	}
}
#endif
