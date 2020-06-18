using System;

namespace BehaviorTree {
    public class BTActionNode : BTNode {
        Func<ActionResult> action;
        
        public BTActionNode(BTNode parentNode, Func<ActionResult> actionFunc) : base(parentNode) {
            action = actionFunc;
        }

        public override bool Update() {
            if (!base.Update()) {
                return false;
            }

            var result = action();
            switch (result) {
                case ActionResult.Success:
                    status = Status.Success;
                    parent.SetResult(NodeResult.Success);
                    break;
                case ActionResult.Failure:
                    status = Status.Failure;
                    parent.SetResult(NodeResult.Failure);
                    break;
            }

            return true;
        }
    }
}