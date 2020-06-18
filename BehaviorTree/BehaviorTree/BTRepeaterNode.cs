namespace BehaviorTree {
    public class BTRepeaterNode : BTNode {
        int _loopCount = 0;
        int _counter = 0;
        
        public BTRepeaterNode(BTNode parentNode,int loopCount) : base(parentNode) {
            _loopCount = loopCount;
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
                    _counter++;
                    _children[0].ResetStatus();
                    _children[0].Execute();
                    if (_counter >= _loopCount) {
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

        public override void ResetStatus() {
            base.ResetStatus();
            _counter = 0;
        }

        public void AppendNode(BTNode childNode) {
            if (_children.Count == 0) {
                _children.Add(childNode);
            }
        }
    }
}