using UnityEngine;
using BehaviourTree;
public class HasDestinationNode : Node
{
    //Checks if we have a destination in our data, if we dont our selector will run the PickRandomPosition
    public override NodeState Evaluate()
    {
        
        if (GetData("patrolTarget") != null) {

            return NodeState.SUCCESS;

        }

        return NodeState.FAILURE;
    }
}
