using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessyAIPlugin.MessyAI;

public partial class AIAssetsImportDialog : Window
{   
    
    private List<AssetSetSummaryInfo> _remoteAllAssetSetInfos;
    private string _goalAssetSetPath;

    private string _backendUrl;
    private string _selectedSolutionId;

    private ItemList _assetSetsContent;
    private HttpRequest _httpListAssetSetsRequester;

    public override void _EnterTree()
    {
        _backendUrl = ProjectSettings.GetSetting("MessyAIConfigure/BackendUrl").AsString();
        _selectedSolutionId = ProjectSettings.GetSetting("MessyAIConfigure/SelectedSolutionId").AsString();

        _httpListAssetSetsRequester = new HttpRequest();
        _httpListAssetSetsRequester.Connect("request_completed", new Callable(this,nameof(OnListAssetSetsRequestCompleted)));
        AddChild(_httpListAssetSetsRequester);

        _assetSetsContent = GetNode<ItemList>("VBoxContainer/AssetSetsContent");
        _assetSetsContent.Connect("item_selected", new Callable(this, nameof(OnSelectAssetSet)));

        RequestAllAssetSets();
    }

    public override void _ExitTree()
    {
        _assetSetsContent.Clear();
        _assetSetsContent.Disconnect("item_selected");
        _httpListAssetSetsRequester.Disconnect("request_completed");
        RemoveChild(_httpListAssetSetsRequester);
    }
    
    private void RequestAllAssetSets()
    {
        if (!string.IsNullOrEmpty(_backendUrl) && !string.IsNullOrEmpty(_selectedSolutionId))
        {   
            JObject req = new JObject();
            req.Add("solutionId", _selectedSolutionId);
            
            _httpListAssetSetsRequester.Request(Path.Combine(_backendUrl, ServerAPI.ListAllAssetSets), new string[] { }, HttpClient.Method.Post, req.ToString());
        }
    }
    
    private void OnListAssetSetsRequestCompleted(int result, int responseCode, string[] headers, byte[] body)
    {
        if (responseCode == 200) // HTTP OK
        {
            string responseText = System.Text.Encoding.UTF8.GetString(body);
            var responseMsg =  JsonConvert.DeserializeObject<ListAssetSetsResponseMsg>(responseText);
            if (responseMsg.ErrCode == 0)
            { 
                _remoteAllAssetSetInfos = responseMsg.AssetSets;
                RefreshDisplay();
            }
        }
        else
        {
            GD.PrintErr($"Request failed with response code: {responseCode}");
        }
    }

    private void RefreshDisplay()
    {
        _assetSetsContent.ClearChildren();
        foreach (var assetSetItem in _remoteAllAssetSetInfos)
        {
            _assetSetsContent.AddItem(assetSetItem.AssetSetName);
        }
    }

    private void OnSelectAssetSet(int idx)
    {
        
    }
}