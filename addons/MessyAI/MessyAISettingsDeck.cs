using System.IO;
using Godot;
using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

[Tool]
[GlobalClass]
public partial class MessyAISettingsDeck : Panel
{
    private HttpRequest _httpRequest;
    private OptionButton _solutionOptions;
    private Button _btnSubmitMetas;
    private Button _btnSynchronousAIs;

    private MetaUploadDialog _metaUploadDialog;
    private SolutionSummaryInfo[] _optionalSolutionSummaryInfos;

    private string _persistenceBackendUrl;
    private string _currentInputBackendUrl;
    private bool _urlChecked;

    private LineEdit _urlTextBox;
    
    public override void _EnterTree()
    {
        GD.Print("MessyAISettingsDeck EnterTree");
        
        _urlChecked = false;
        
        //PersistenceBackendUrl
        _persistenceBackendUrl = ProjectSettings.GetSetting("MessyAIConfigure/BackendUrl").AsString();
        _currentInputBackendUrl = _persistenceBackendUrl;
        
        // Remote Requester
        _httpRequest = new HttpRequest();
        _httpRequest.Connect("request_completed", new Callable(this,nameof(OnRequestCompleted)));
        AddChild(_httpRequest);
        
        _urlTextBox = GetNode<LineEdit>("VerticalBox/ServerUrlContainer/ServerUrlText");
        _urlTextBox.Text = _currentInputBackendUrl;
        _urlTextBox.Connect("text_submitted", new Callable(this, nameof(OnNewServerAddressEntered)));
        _urlTextBox.Connect("text_changed", new Callable(this, nameof(OnNewServerAddressChanged)));

        _solutionOptions = GetNode<OptionButton>("VerticalBox/SolutionOptionsContainer/SolutionOptions");
        _solutionOptions.Connect("item_selected", new Callable(this, nameof(OnSolutionSelectChanged)));

        _btnSubmitMetas = GetNode<Button>("VerticalBox/AIOperatorContainer/Btn_SubmitMetas");
        _btnSubmitMetas.Pressed += MetaUploadButtonPressed;
        
        _btnSynchronousAIs = GetNode<Button>("VerticalBox/AIOperatorContainer/Btn_SynchronousAIs");
        _btnSynchronousAIs.Pressed += SynchronousAIButtonPressed;
        
        if (!string.IsNullOrEmpty(_persistenceBackendUrl))
        {
            GD.Print($"Request AI Editor End {_persistenceBackendUrl}");
            _httpRequest.Request(Path.Combine(_persistenceBackendUrl, ServerAPI.ListSolutionAPI), new string[] { });
        }
    }

    public override void _ExitTree()
    {
        GD.Print("MessyAISettingsDeck ExitTree");

        _urlTextBox.Disconnect("text_submitted");
        _urlTextBox.Disconnect("text_changed");
        
        _solutionOptions.Disconnect("item_selected");

        _btnSubmitMetas.Pressed -= MetaUploadButtonPressed;
        _btnSynchronousAIs.Pressed -= SynchronousAIButtonPressed;
        
        _httpRequest.Disconnect("request_completed");
        
        RemoveChild(_httpRequest);
    }

    // Handle the response from the request
    private void OnRequestCompleted(int result, int responseCode, string[] headers, byte[] body)
    {
        if (responseCode == 200) // HTTP OK
        {
            string responseText = System.Text.Encoding.UTF8.GetString(body);
            var responseMsg =  JsonConvert.DeserializeObject<ListSolutionResponseMsg>(responseText);
            if (responseMsg.ErrCode == 0)
            {
                _persistenceBackendUrl = _currentInputBackendUrl;
                ProjectSettings.SetSetting("MessyAIConfigure/BackendUrl", _persistenceBackendUrl);
                
                _optionalSolutionSummaryInfos = responseMsg.Solutions;

                _urlChecked = true;
                RefreshServerUrlTextBox();
                RefreshOptionalSolutions();
            }
        }
        else
        {
            GD.PrintErr($"Request failed with response code: {responseCode}");
        }
    }
    
    private void OnNewServerAddressEntered(string newUrl)
    {
        _currentInputBackendUrl = newUrl;
        _httpRequest.Request(Path.Combine(newUrl, ServerAPI.ListSolutionAPI), new string[] { });
    }

    private void OnNewServerAddressChanged(string newUrl)
    {
        _currentInputBackendUrl = newUrl;
        RefreshServerUrlTextBox();
    }

    private void OnSolutionSelectChanged(int selectedIdx)
    {
        if (selectedIdx == 0)
        {
            ProjectSettings.SetSetting("MessyAIConfigure/SelectedSolutionId", "");
        }
        else
        {
            ProjectSettings.SetSetting("MessyAIConfigure/SelectedSolutionId", _optionalSolutionSummaryInfos[selectedIdx - 1].SolutionId);
        }

        _solutionOptions.Selected = selectedIdx;
    }
    
    private void MetaUploadButtonPressed()
    {
        _metaUploadDialog = new MetaUploadDialog();
        _metaUploadDialog.Connect("close_requested", new Callable(this, nameof(OnMetaUploadDialogClosed)));
        AddChild(_metaUploadDialog);
        _metaUploadDialog.Show();
    }

    private void SynchronousAIButtonPressed()
    {
        
    }

    private void OnMetaUploadDialogClosed()
    {
        _metaUploadDialog.Hide();
        RemoveChild(_metaUploadDialog);
        _metaUploadDialog = null;
    }

    private void OnSynchronousAIDialogClosed()
    {
        
    }

    private void RefreshServerUrlTextBox()
    {
        if (_urlChecked && _persistenceBackendUrl == _currentInputBackendUrl)
        {
            _urlTextBox.AddThemeColorOverride("font_color", new Color(0,1,0));
        }
        else
        {
            _urlTextBox.AddThemeColorOverride("font_color", new Color(1,0,0));
        }
    }

    private void RefreshOptionalSolutions()
    {
        string currentSelectedSolutionId = ProjectSettings.GetSetting("MessyAIConfigure/SelectedSolutionId").AsString();
        
        _solutionOptions.Clear();
        _solutionOptions.AddItem("Empty");
        var selectIdx = 0;
        for (var i = 0; i < _optionalSolutionSummaryInfos.Length; ++i)
        {
            if (currentSelectedSolutionId == _optionalSolutionSummaryInfos[i].SolutionId)
            {
                selectIdx = i + 1;
            }
            _solutionOptions.AddItem(_optionalSolutionSummaryInfos[i].SolutionName);
        }
        _solutionOptions.Selected = selectIdx;
    }
}