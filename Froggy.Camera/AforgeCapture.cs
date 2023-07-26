using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Froggy.Camera
{
    public class AforgeCapture : ICapture
    {
        private VideoCaptureDevice _webcam = null;
        private List<Bitmap> _buffer = new List<Bitmap>();
        private int _bufferIndex = 0;
        private static readonly int BUFFER_SIZE = 5;

        public event FrameHandler FrameCallback;
        public event ErrorHandler ErrorCallback;

        public void Initialize(string deivceID, Resolution resolution)
        {
            try
            {
                _webcam = new VideoCaptureDevice(deivceID);
                VideoCapabilities cap = _webcam.VideoCapabilities.Where(x => x.FrameSize.Width == resolution.Width & x.FrameSize.Height == resolution.Height).FirstOrDefault();
                if (cap != null)
                {
                    _webcam.VideoResolution = cap;
                }
                _createBuffer(resolution);
                _webcam.NewFrame += _onFrame;
                _webcam.VideoSourceError += _onError;
            }
            catch { throw; }
        }

        public void Close()
        {
            try
            {
                Stop();
                _buffer.Clear();
                _webcam = null;
            }
            catch { throw; }
        }

        public void Start()
        {
            try
            {
                if (_webcam != null && !_webcam.IsRunning) { _webcam.Start(); }
            }
            catch { throw; }
        }

        public void Stop()
        {
            try
            {
                if (_webcam != null && _webcam.IsRunning) 
                { 
                    _webcam.SignalToStop();
                    _webcam.WaitForStop();
                }
            }
            catch { throw; }
        }

        private void _onFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if(_bufferIndex == BUFFER_SIZE) _bufferIndex = 0;
            Bitmap bitmap = eventArgs.Frame;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = data.Scan0;
            int depth = data.Stride / data.Width;
            bitmap.UnlockBits(data);
            _buffer[_bufferIndex] = bitmap;
            FrameCallback?.Invoke(ptr, bitmap.Width, bitmap.Height, depth);
            _bufferIndex++;
        }

        private void _createBuffer(Resolution resolution)
        {
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                _buffer.Add(new Bitmap(resolution.Width, resolution.Height));
            }
        }

        private void _onError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            ErrorCallback?.Invoke(new Exception(eventArgs.Description));
        }
    }
}
