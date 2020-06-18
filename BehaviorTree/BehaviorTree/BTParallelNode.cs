using System.Linq;

namespace BehaviorTree {
    public class BTParallelNode : BTNode {
        public BTParallelNode(BTNode parentNode) : base(parentNode) {
            
        }

        public override bool Execute() {
            if (!base.Execute()) {
                return false;
            }
            foreach (var child in _children) {
                child.Execute();
            }

            if (_children.Count <= 0) {
                status = Status.Failure;
            }
            return true;
        }

        bool _isOnceFailure = false;
        public override void SetResult(NodeResult result) {
            if (result == NodeResult.Failure) {
                _isOnceFailure = true;
            }
        }

        public override bool Update() {
            if (!base.Update()) {
                return false;
            }

            foreach (var child in _children) {
                child.Update();
            }

            bool isFinished = _children.All(itr => { return itr.status != Status.Running; });
            if (isFinished) {
                if (_isOnceFailure) {
                    status = Status.Failure;
                    parent?.SetResult(NodeResult.Failure);
                } else {
                    status = Status.Success;
                    parent?.SetResult(NodeResult.Success);
                }
            }
            
            return true;
        }
        
        public void AddNode(BTNode childNode) {
            _children.Add(childNode);
        }

        public override void ResetStatus() {
            base.ResetStatus();
            _isOnceFailure = false;
        }
    }
}