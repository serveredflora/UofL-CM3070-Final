public enum ActionTransitionStatus
{
    /// <summary>
    /// Not started yet
    /// </summary>
    Idle,

    /// <summary>
    /// Running, can transition before finished
    /// </summary>
    Interruptible,

    /// <summary>
    /// Running, cannot transition till finished
    /// </summary>
    Uninterruptible,

    /// <summary>
    /// Finished
    /// </summary>
    Completed
}
