using System.Collections.Generic;
using System.Linq;

public interface IDependencyGraphNode
{
    IEnumerable<IDependencyGraphNode> Requirements { get; }
}

public class DependencyGraph<TNode> where TNode : IDependencyGraphNode
{
    public void ComputeExecutionOrder()
    {
        // Find node(s) that
    }
}

// Example usage below...

public class ProcGenCityDependencyGraph : DependencyGraph<ProcGenCityDependencyGraphNode> { }

public class ProcGenCityDependencyGraphNode : IDependencyGraphNode
{
    private readonly List<ProcGenCityDependencyGraphNode> _Requirements = new();

    public IEnumerable<IDependencyGraphNode> Requirements => _Requirements.Cast<IDependencyGraphNode>();
}
