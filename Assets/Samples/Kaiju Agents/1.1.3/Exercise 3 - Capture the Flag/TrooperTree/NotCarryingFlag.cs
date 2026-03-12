using BehaviourTree;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;

public class NotCarryingFlag : Node
{
    private Trooper trooper;

    public NotCarryingFlag(TrooperController controller)
    {
        trooper = controller.GetComponent<Trooper>();
    }

    public override NodeState Evaluate()
    {
        //Fail if the trooper is carrying a flag
        if (trooper._flag != null)
        {
            return NodeState.FAILURE;
        }

        //Success if trooper is not carrying anything
        return NodeState.SUCCESS;
    }
}