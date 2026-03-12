using UnityEngine;
using System.Collections;

namespace BehaviourTree {
    public abstract class Tree
    {
        //Setup tree with a root
        private Node _root = null;
        protected void Start() {
            _root = SetupTree();
        }
        //Update roots 
        protected void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }
        //Abstract setup to override
        protected abstract Node SetupTree();
    }

}
