public interface IAction
{
    bool IsActive { get; }
    ActionTransitionStatus CurrentStatus { get; }

    void Start();
    void Stop(ActionStopReason reason);

    ActionResult Update(float deltaTime);
}
