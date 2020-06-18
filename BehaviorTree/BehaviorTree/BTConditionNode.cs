using System;

namespace BehaviorTree {
    public class BTConditionNode : BTNode {
        Func<bool> _compareFunc;
        
        public BTConditionNode(BTNode parentNode, Func<bool> compareFunc) : base(parentNode) {
            _compareFunc = compareFunc;
        }

        public override bool Execute() {
            if (!base.Execute()) {
                return false;
            }

            _children[_execIndex].Execute();
            return true;
        }

        public override void SetResult(NodeResult result) {
            switch (result) {
                case NodeResult.Success:
                    status = Status.Success;
                    parent?.SetResult(NodeResult.Success);
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
            
            if (_compareFunc()) {
                _children[_execIndex].Update();
            } else {
                status = Status.Failure;
                parent?.SetResult(NodeResult.Failure);
            }

            return true;
        }

        public void AppendNode(BTNode childNode) {
            if (_children.Count == 0) {
                _children.Add(childNode);
            }
        }
    }
}