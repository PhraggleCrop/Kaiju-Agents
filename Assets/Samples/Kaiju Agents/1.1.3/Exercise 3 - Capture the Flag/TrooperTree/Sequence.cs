using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                NodeState result = node.Evaluate();

                switch (result)
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;

                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;

                    case NodeState.SUCCESS:
                        continue;
                }
            }

            state = NodeState.SUCCESS;
            return state;
        }
    }
}