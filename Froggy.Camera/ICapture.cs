using System;

namespace Froggy.Camera
{
    public delegate void FrameHandler(IntPtr pointer, int width, int height, int depth);
    public delegate void ErrorHandler(Exception error);

    public interface ICapture
    {
        void Close();
        void Initialize(string deivceID, Resolution resolution);
        void Start();
        void Stop();

        event FrameHandler FrameCallback;
        event ErrorHandler ErrorCallback;
    }
}