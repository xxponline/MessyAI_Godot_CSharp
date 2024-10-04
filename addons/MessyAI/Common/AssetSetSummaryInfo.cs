using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

public class AssetSetSummaryInfo
{
    [JsonProperty("assetSetId")] public string AssetSetId;
    [JsonProperty("assetSetName")] public string AssetSetName;
}