#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Screens
{
    public class Import : BeatmapScreen
    {
        private BeatmapManager manager;

        private Storage storage;

        private Storage temp;

        public Import()
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Import from Host's Stable Install",
                    Action = import
                },
                new SymcolButton
                {
                    ButtonText = "Back",
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = 50,
                    Action = this.Exit,
                    Position = new Vector2(0 , -10),
                },
                new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Toggle TCP",
                    Action = () => { OnlineModset.OsuNetworkingHandler.Tcp = !OnlineModset.OsuNetworkingHandler.Tcp; }
                },
            };

            OnlineModset.OsuNetworkingHandler.OnPacketReceive += packetReceived;
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapManager manager, Storage storage)
        {
            this.manager = manager;
            this.storage = storage;
            temp = this.storage.GetStorageForDirectory("online\\temp");
        }

        private MapSetPacket quedSetPacket;

        private int piecesSize;
        private Dictionary<byte[], int> pieces = new Dictionary<byte[], int>();

        protected override void Update()
        {
            base.Update();

            //this could be for looped but lets not threadlock for now instead
            if (OnlineModset.OsuNetworkingHandler.Tcp && OnlineModset.OsuNetworkingHandler.TcpNetworkingClient.Available > 0)
                fetch();

            //We got the whole map yet?
            if (quedSetPacket != null && piecesSize == quedSetPacket.Size)
            {
                //lets do this!
                import(quedSetPacket);
                //reset for next map
                quedSetPacket = null;

                //Tell the host we are ready for the next one
                OnlineModset.OsuNetworkingHandler.SendToServer(new ImportCompletePacket());
            }
        }

        /// <summary>
        /// Fetch TCP data if we are importing
        /// </summary>
        private void fetch()
        {
            byte[] data = new byte[TcpNetworkingClient.BUFFER_SIZE / 8];
            int count = OnlineModset.OsuNetworkingHandler.TcpNetworkStream.Read(data, 0, data.Length);
            piecesSize += count;
            pieces.Add(data, count);
            Logger.Log($"Data fetched for importing from stable ({piecesSize})", LoggingTarget.Network);
        }

        /// <summary>
        /// Request to import from stable
        /// </summary>
        private void import()
        {
            OnlineModset.OsuNetworkingHandler.SendToServer(new ImportPacket());
        }

        private void packetReceived(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MapSetPacket mapSetPacket:
                    //que it since we might not have the whole mapset.zip yet
                    quedSetPacket = mapSetPacket;
                    break;
            }
        }

        private void import(MapSetPacket set)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Logger.Log($"Beginning Import of ({set.MapName})...", LoggingTarget.Network);

                    //Basically just create the temp folder then delete the writer, bit of a hack but works for now
                    StreamWriter writer = new StreamWriter(temp.GetStream($"{set.MapName}.zip", FileAccess.ReadWrite, FileMode.Create));
                    writer.Close();

                    using (FileStream fs = new FileStream(temp.GetFullPath($"{set.MapName}.zip"), FileMode.Create, FileAccess.Write))
                    {
                        //Save all the pieces until we have all of them
                        foreach (KeyValuePair<byte[], int> pair in pieces)
                            fs.Write(pair.Key, 0, pair.Value);
                    }

                    //Extract it to be imported via the vanilla importer
                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.MapName}.zip"), temp.GetFullPath($"{set.MapName}"), Encoding.UTF8);
                    Logger.Log($"Zip extraction while receiving ({set.MapName}) sucessful, cleaning up...", LoggingTarget.Network);

                    //Cleanup our mess for mobile device's sake!
                    temp.Delete($"{set.MapName}.zip");
                    pieces = new Dictionary<byte[], int>();
                    piecesSize = 0;
                    Logger.Log($"Clean up complete, beginning import...", LoggingTarget.Network);
                }
                catch (Exception e) { Logger.Error(e, "Failed to receive map!", LoggingTarget.Network); }
            }, TaskCreationOptions.LongRunning).ContinueWith(result =>
            {
                //Actually import the map now from the extracted folder, they should delete it when they are done for us!
                manager.Import(new[]
                {
                    temp.GetFullPath($"{set.MapName}")
                });
            });
        }

        public override bool OnExiting(IScreen next)
        {
            //bit of cleanup when we exit
            OnlineModset.OsuNetworkingHandler.OnPacketReceive -= packetReceived;
            OnlineModset.OsuNetworkingHandler.Tcp = false;
            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");
            return base.OnExiting(next);
        }
    }
}
