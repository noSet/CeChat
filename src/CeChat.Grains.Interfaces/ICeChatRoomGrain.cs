using CeChat.Grains.Interfaces.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;

namespace CeChat.Grains.Interfaces
{
    public interface ICeChatRoomGrain : IGrainWithGuidKey
    {
        Task<ImmutableArray<Record>> GetAllAsync();

        Task SetName(string name);

        Task Join(Guid userKey);

        Task Leave(Guid userKey);

        Task Recording(Record record);
    }
}
