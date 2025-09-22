public class ActionTransition
{
    public enum Condition
    {
        Always,
        OnlyOnComplete,
    }

    public IAction targetAction;
    public Condition condition;

    public ActionTransition(IAction targetAction, Condition condition)
    {
        this.targetAction = targetAction;
        this.condition = condition;
    }

    public bool IsAvailable(IAction currentAction)
    {
        return currentAction != targetAction && (condition == Condition.Always || currentAction.CurrentStatus == ActionTransitionStatus.Interruptible);
    }
}