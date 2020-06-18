using System.Collections.Generic;

namespace BehaviorTree {
    public abstract class BTNode {
        public Status status { get; protected set; } = Status.Ready;
        public BTNode parent { get; protected set; } = null;
        protected List<BTNode> _children = new List<BTNode>();
        protected int _execIndex = 0;

        public BTNode(BTNode parentNode) {
            parent = parentNode;
        }

        public virtual bool Execute() {
            if (status != Status.Ready) {
                return false;
            }

            status = Status.Running;
            return true;
        }

        public virtual void SetResult(NodeResult result) {

        }

        public virtual bool Update() {
            if (this.status != Status.Running) {
                return false;
            }

            return true;
        }

        public virtual void ResetStatus() {
            status = Status.Ready;
            _execIndex = 0;
            foreach (var child in _children) {
                child.ResetStatus();
            }
        }
    }
}