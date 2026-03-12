using BehaviourTree;

public class ClearPatrolDestination : Node
{
    public override NodeState Evaluate()
    {
        ClearData("patrolTarget");
        return NodeState.SUCCESS;
    }
}