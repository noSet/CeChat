using CeChat.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeChat
{
    public partial class CeChatApp : Form
    {
        public ICeChatRoomService CeChatRoomService { get; }

        public CeChatApp(ICeChatRoomService ceChatRoomService)
        {
            InitializeComponent();
            this.CeChatRoomService = ceChatRoomService;
        }
    }
}
