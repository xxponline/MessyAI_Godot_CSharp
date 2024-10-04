using System.Collections.Generic;
using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

public class ListAssetSetsResponseMsg : CommonResponseMsg
{
    [JsonProperty("assetSets")] public List<AssetSetSummaryInfo> AssetSets;
}