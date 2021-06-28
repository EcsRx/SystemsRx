using SystemsRx.Attributes;
using SystemsRx.Systems;
using SystemsRx.Types;

namespace SystemsRx.Tests.Systems
{
    [Priority(PriorityTypes.High)]
    public class HighPrioritySystem : ISystem
    {
    }

    [Priority(PriorityTypes.Higher)]
    public class HighestPrioritySystem : ISystem
    {
    }
    
    [Priority(PriorityTypes.Lower)]
    public class LowerThanDefaultPrioritySystem : ISystem
    {
    }
}