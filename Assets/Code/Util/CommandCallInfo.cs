using FishNet.Connection;
using JetBrains.Annotations;

namespace Code.Console
{
    public record CommandCallInfo([CanBeNull] NetworkConnection conn = null);
    
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit{}
}