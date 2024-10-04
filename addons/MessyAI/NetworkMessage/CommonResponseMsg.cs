using Newtonsoft.Json;

namespace MessyAIPlugin.MessyAI;

public class CommonResponseMsg
{
    [JsonProperty("errCode")]
    public int ErrCode;
    [JsonProperty("errMessage")]
    public string ErrMessage;
}