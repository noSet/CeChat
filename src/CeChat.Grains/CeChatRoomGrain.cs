using CeChat.Grains.Interfaces;
using CeChat.Grains.Interfaces.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CeChat.Grains
{
    public class CeChatRoomGrain : Grain, ICeChatRoomGrain
    {
        private readonly ILogger<CeChatRoomGrain> logger;
        private readonly IPersistentState<State> state;

        private Guid RoomKey => this.GetPrimaryKey();

        public CeChatRoomGrain(ILogger<CeChatRoomGrain> logger, [PersistentState("State")] IPersistentState<State> state)
        {
            this.logger = logger;
            this.state = state;
        }

        public override Task OnActivateAsync()
        {
            if (string.IsNullOrWhiteSpace(state.State.Name))
            {
                state.State.Name = RoomKey.ToString();
            }

            if (state.State.Users == null)
            {
                state.State.Users = new HashSet<Guid>();
            }

            if (state.State.Records == null)
            {
                state.State.Records = new List<Record>();
            }

            return base.OnActivateAsync();
        }

        public Task<ImmutableArray<Record>> GetAllAsync()
            => Task.FromResult(ImmutableArray.CreateRange(state.State.Records));

        public async Task SetName(string name)
        {
            state.State.Name = name;
            await state.WriteStateAsync();
        }

        public async Task Join(Guid userKey)
        {
            state.State.Users.Add(userKey);
            await state.WriteStateAsync();
        }

        public async Task Leave(Guid userKey)
        {
            state.State.Users.Remove(userKey);
            await state.WriteStateAsync();
        }

        public async Task Recording(Record record)
        {
            state.State.Records.Add(record);
            await state.WriteStateAsync();

            GetStreamProvider("SMS").GetStream<RecordNotification>(RoomKey, nameof(ICeChatRoomGrain))
                .OnNextAsync(new RecordNotification(record))
                .Ignore();
        }

        public class State
        {
            public string Name { get; set; }

            public HashSet<Guid> Users { get; set; }

            public List<Record> Records { get; set; }
        }
    }
}
