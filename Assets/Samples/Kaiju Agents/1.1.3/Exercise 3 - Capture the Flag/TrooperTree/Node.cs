using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree {
    //Define states nodes can be in
    public enum NodeState { 
        RUNNING,
        SUCCESS,
        FAILURE
    
    }
    public class Node
    {
        //We have references to our children and the parent
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Nodes have a dictionary of data they can update, it propogates to all other nodes
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();


        public Node()
        {
            parent = null;
        }
        //Constructor attaches our children and sets ourselves as parent
        public Node(List<Node> children) {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }
        private void _Attach(Node node) {
            node.parent = this;
            children.Add(node);
        }
        //Set default evaluate to fail, but can be overriden
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        //Setting data
        public void SetData(string key, object value) { 
            _dataContext[key] = value;
        }
        //Getting data looks through the tree
        public object GetData(string key) {
            object value = null;
            if (_dataContext.TryGetValue(key, out value)) 
                return value;
            Node node = parent;
            while (node != null)
            {
            value = node.GetData(key);
            if (value != null)
                return value;
            node = node.parent;
            }
            return null;
        }
        //Clearing data erases data from tree
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key)) {
                _dataContext.Remove(key);
                return true;
            }
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}

