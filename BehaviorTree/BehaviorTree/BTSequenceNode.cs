

using System;

namespace BehaviorTree {
    public class BTSequenceNode : BTNode {
        
        public BTSequenceNode(BTNode parentNode) : base(parentNode) {
            
        }
        
        public override bool Execute() {
            if (!base.Execute()) {
                return false;
            }
            if (_children.Count > 0) {
                _children[_execIndex].Execute();
            } else {
                status = Status.Failure;
            }
            return true;
        }

        public override void SetResult(NodeResult result) {
            switch (result) {
                case NodeResult.Success:
                    if (_execIndex + 1 < _children.Count) {
                        _execIndex++;
                        _children[_execIndex].Execute();
                    } else {
                        status = Status.Success;
                        parent?.SetResult(NodeResult.Success);
                    }
                    break;
                case NodeResult.Failure:
                    status = Status.Failure;
                    parent?.SetResult(NodeResult.Failure);
                    break;
            }
        }

        public override bool Update() {
            if (!base.Update()) {
                return false;
            }

            _children[_execIndex].Update();

            return true;
        }

        public void AddNode(BTNode childNode) {
            _children.Add(childNode);
        }
    }
}