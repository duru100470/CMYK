using System;

namespace BasicInjector
{
    public class ResolverNotFoundException : Exception
    {
        public ResolverNotFoundException() { }
        public ResolverNotFoundException(string message) : base(message) { }
        public ResolverNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        public ResolverNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}