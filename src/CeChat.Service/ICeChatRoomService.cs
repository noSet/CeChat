using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeChat.Service
{
    public interface ICeChatRoomService
    {
        void Join(string userName);

        void Leave(string userName);

        void SendMessage(string message);

        IEnumerable<string> GetMessages();
    }
}
