using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Froggy.Camera
{
    public class WebcamDevice
    {
        public static async Task<List<WebcamInfo>> GetWebcamInfoList()
        {
            try
            {
                List<WebcamInfo> results = new List<WebcamInfo>();
                await Task.Run(() =>
                {
                    FilterInfoCollection webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    foreach (FilterInfo webcam in webcams)
                    {
                        string deivce_id = webcam.MonikerString;
                        VideoCaptureDevice device = new VideoCaptureDevice(deivce_id);
                        WebcamInfo webcam_info = new WebcamInfo();
                        webcam_info.Name = webcam.Name;
                        webcam_info.DeviceID = deivce_id;
                        foreach (VideoCapabilities cap in device.VideoCapabilities)
                        {
                            webcam_info.Resolutions.Add(new Resolution { Width = cap.FrameSize.Width, Height = cap.FrameSize.Height });
                        }
                        results.Add(webcam_info);
                        device = null;
                    }
                });
                return results;
            }
            catch { throw; }
        }

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
    }

    public class WebcamInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DeviceID { get; set; } = string.Empty;
        public List<Resolution> Resolutions { get; set; } = new List<Resolution>();
    }

    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
