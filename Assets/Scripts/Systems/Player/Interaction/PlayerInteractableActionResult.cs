using System;

public readonly struct PlayerInteractableActionPerformResult
{
    public readonly bool Success;
    public readonly string FailMessage;

    /// <param name="success">Indicates if the action was performed successfully</param>
    /// <param name="failMessage">Can be null if <paramref name="success"/> is true</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="success"/> is false and <paramref name="failMessage"/> is null</exception>
    public PlayerInteractableActionPerformResult(bool success, string failMessage = null)
    {
        Success = success;
        FailMessage = success ? failMessage : (failMessage ?? throw new ArgumentNullException(nameof(failMessage)));
    }
}
