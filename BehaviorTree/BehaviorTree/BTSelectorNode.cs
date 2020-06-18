namespace BehaviorTree {
    public class BTSelectorNode : BTNode{
        public BTSelectorNode(BTNode parent) : base(parent) {
            
        }
        
        public override void Execute() {
            base.Execute();
            if (_children.Count > 0) {
                _children[_execIndex].Execute();
            } else {
                status = Status.Failure;
            }
        }
        
        public override void SetResult(NodeResult result) {
            switch (result) {
                case NodeResult.Failure:
                    if (_execIndex + 1 < _children.Count) {
                        _execIndex++;
                        _children[_execIndex].Execute();
                    } else {
                        status = Status.Failure;
                        parent?.SetResult(NodeResult.Failure);
                    }
                    break;
                case NodeResult.Success:
                    status = Status.Success;
                    parent?.SetResult(NodeResult.Success);
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