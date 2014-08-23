using System;
using okbrain.Models;

namespace okbrain.Domain.Messages
{
    public interface IMessage
    {
    }

    public abstract class Command : IMessage
    {
        public Guid AgId { get; protected set; }
    }


    public class CreateCommand : Command
    {
        public CreateCommand()
        {
            AgId = Guid.NewGuid();
        }
    }

    public abstract class Event : IMessage
    {
        public readonly Guid AgId;
        public readonly int Version;
        public readonly DateTime Generated;
        
        protected Event(Guid agId, int version, DateTime generated)
        {
            AgId = agId;
            Version = version;
            Generated = generated;
        }
    }
}
