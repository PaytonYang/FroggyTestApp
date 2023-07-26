using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggyTest.WPF.Models
{
    public partial class CameraModel : ObservableObject
    {
        [ObservableProperty]
        private string _cameraName = string.Empty;

        [ObservableProperty]
        private string _deviceID = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ImageSize> _resolutions = new ObservableCollection<ImageSize>();

        [ObservableProperty]
        private ImageSize? _selectedImageSize = null;
    }

    public partial class ImageSize : ObservableObject
    {
        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;
    }
}
