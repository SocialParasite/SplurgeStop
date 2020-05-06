using System;

namespace SplurgeStop.Domain.Exceptions
{
    public sealed class InvalidEntityStateException : Exception
    {
            public InvalidEntityStateException(object entity, string message)
                : base($"Entity {entity.GetType().Name} is in invalid state, {message}")
            {
            }
    }
}
