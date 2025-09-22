public interface ITimer : ITimerReadOnly
{
    bool Paused { get; set; }
    bool OneShot { get; set; }

    void Begin(float waitTime, bool restart = false);
    void End();

    void Tick(float deltaTime);
}
