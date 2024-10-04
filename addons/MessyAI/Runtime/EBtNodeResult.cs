namespace MessyAIPlugin.MessyAI;

public enum EBtNodeResult
{
    UnExecuted,
    LoopReset, // basically ,it's equal UnExecuted. it's used for loop some node by descriptor.
    Success,
    Failed,
    Aborted,
    InProgress,
    AbsolutelyAborted // this result is caused by parent Aborted. for avoid the Descriptor.AfterNodeExecuted to modify the result.
}