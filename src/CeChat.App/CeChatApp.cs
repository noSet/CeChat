using CeChat.Grains.Interfaces;
using CeChat.Grains.Interfaces.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeChat.App
{
    public partial class CeChatApp : Form
    {
        public ILogger<CeChatApp> Logger { get; }

        public IClusterClient ClusterClient { get; }

        public CeChatApp(ILogger<CeChatApp> logger, IClusterClient clusterClient)
        {
            InitializeComponent();
            Logger = logger;
            ClusterClient = clusterClient;


            //txtRecords
        }

        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            while (!ClusterClient.IsInitialized)
            {
                await Task.Delay(100);
            }

            await ClusterClient
                .GetStreamProvider("SMS")
                .GetStream<RecordNotification>(Guid.Empty, nameof(ICeChatRoomGrain))
                .SubscribeAsync((notification, token) =>
                {
                    txtRecords.BeginInvoke(new Action<RecordNotification>(Update), notification);
                    return Task.FromResult(0);
                });
        }

        private void Update(RecordNotification recordNotification)
        {
            this.txtRecords.AppendText(recordNotification.Record.Content + Environment.NewLine);
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            string content = this.txtContent.Text;

            if (!string.IsNullOrWhiteSpace(content))
            {
                await ClusterClient
                    .GetGrain<ICeChatRoomGrain>(Guid.Empty)
                    .Recording(new Record(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, content));

                this.txtContent.Text = null;
            }
        }
    }
}
