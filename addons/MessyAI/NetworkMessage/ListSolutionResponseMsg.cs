using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

public class ListSolutionResponseMsg : CommonResponseMsg
{
    [JsonProperty("solutions")] public SolutionSummaryInfo[] Solutions;
}