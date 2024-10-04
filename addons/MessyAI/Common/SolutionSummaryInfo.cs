using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

public class SolutionSummaryInfo
{
    [JsonProperty("solutionId")] public string SolutionId;
    [JsonProperty("solutionName")] public string SolutionName;
    [JsonProperty("solutionVersion")] public string SolutionVersion;
}