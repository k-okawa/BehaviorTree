namespace BehaviorTree {
    public enum Status {
        Ready,
        Running,
        Success,
        Failure
    }

    public enum NodeResult {
        Success,
        Failure
    }

    public enum ActionResult {
        Running,
        Success,
        Failure
    }
}