using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree {
    public class Sequence : Node
    {
        //Sequences evaluate their children, if a children returns a failure we abort the sequence,
        //Else if they return success or arerunning, we continuing propogating through them
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children) { 
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;

                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;

                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;


        }
    }

}

