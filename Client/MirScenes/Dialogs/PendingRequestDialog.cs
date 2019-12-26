using System;
using System.Collections.Generic;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public sealed class PendingRequestDialog : MirImageControl
    {
        private List<MirImageControl> _requestList = new List<MirImageControl>();
        private int _requestCount;

        public PendingRequestDialog()
        {
            Index = 20;
            Library = Libraries.Prguse2;
            Movable = false;
            Size = new Size(44, 34);
            //Location = new Point(Settings.ScreenWidth - (Size.Width / 2), Settings.ScreenHeight - (Size.Height / 2));
            Location = Center;
            Visible = true;

            Opacity = 1f;
        }

        public void CreatePendingRequest(PendingRequest pendingRequest)
        {
            var requestImage = PendingRequestImage(pendingRequest.Type);
            var requestLibrary = Libraries.Prguse3;

            var image = new MirImageControl
            {
                Library = requestLibrary,
                Parent = this,
                Visible = true,
                Sort = false,
                Index = requestImage
            };

            _requestList.Insert(0, image);
            UpdateWindow();
        }

        public void RemovePendingRequest(int pendingRequestID)
        {
            _requestList[pendingRequestID].Dispose();
            _requestList.RemoveAt(pendingRequestID);

            UpdateWindow();
        }

        private void UpdateWindow()
        {
            _requestCount = _requestList.Count;

            var oldWidth = Size.Width;

            var newX = Location.X - Size.Width + oldWidth;
            var newY = Location.Y;
            Location = new Point(newX, newY);

            Size = new Size((_requestCount * 24) + 2, 26);
        }

        private int PendingRequestImage(PendingRequestType pendingRequestType)
        {
            switch (pendingRequestType)
            {
                case PendingRequestType.Group:
                    return 13;
                case PendingRequestType.Trade:
                    return 15;
                case PendingRequestType.Guild:
                    return 17;

                default:
                    return 0;
            }
        }

        public void Process()
        {
            if (_requestList.Count != _requestCount)
                UpdateWindow();

            for (var i = 0; i < _requestList.Count; i++)
            {
                var image = _requestList[i];
                var request = GameScene.Scene.PendingRequests[i];

                var requestImage = PendingRequestImage(request.Type);
                var requestLibrary = Libraries.Prguse3;

                image.Location = new Point(Size.Width - 10 - 23 - (i * 23) + ((10 * 23) * (i / 10)), 28);
                image.Index = requestImage;
                image.Library = requestLibrary;

                var time = (request.Expire - CMain.Time) / 100D;

                //if (Math.Round(time) % 10 < 5)
                //    image.Index = -1;
            }
        }
    }
}
