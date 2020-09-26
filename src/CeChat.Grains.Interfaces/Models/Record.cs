using Orleans.Concurrency;
using System;

namespace CeChat.Grains.Interfaces.Models
{
    [Immutable]
    public class Record : IEquatable<Record>
    {
        public Guid Key { get; }

        public Guid OwnerKey { get; }

        public Guid UserKey { get; }

        public string Content { get; }

        public DateTimeOffset Timestamp { get; }

        public Record(Guid key, Guid userKey, Guid ownerKey, string content)
        {
            this.Key = key;
            this.UserKey = userKey;
            this.OwnerKey = ownerKey;
            this.Content = content;
            this.Timestamp = DateTimeOffset.UtcNow;
        }

        public bool Equals(Record other)
        {
            if (other == null)
            {
                return false;
            }

            return Key == other.Key
                && UserKey == other.UserKey
                && OwnerKey == other.OwnerKey
                && Content == other.Content
                && Timestamp == other.Timestamp;
        }
    }
}
