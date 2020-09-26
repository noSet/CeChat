using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeChat.Grains.Interfaces.Models
{
    [Immutable]
    public class RecordNotification
    {
        public Record Record { get; }

        public RecordNotification(Record record)
        {
            Record = record;
        }
    }
}
