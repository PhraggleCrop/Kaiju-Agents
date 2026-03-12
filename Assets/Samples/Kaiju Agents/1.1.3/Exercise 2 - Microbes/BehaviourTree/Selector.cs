using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    //Selector evaluates children.
    //If any succeed or are running we return and abort so they finish their work first.
    //If we loop through all children and NON are running or have succeeded then we return a failure
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;

                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;

                    default:
                        continue;
                }

            }

            state = NodeState.FAILURE;
            return state;


        }
    }

}

