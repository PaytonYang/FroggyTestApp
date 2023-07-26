using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Froggy.Camera;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FroggyTest.WPF.ViewModels;

public partial class CameraViewModel : ObservableObject
{
    private ICapture _camera;

    [ObservableProperty]
    private ObservableCollection<CameraModel> _cameras = new ObservableCollection<CameraModel>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCameraCommand))]
    private CameraModel? _selectedCamera = null;

    [ObservableProperty]
    private WriteableBitmap? _image = null;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCameraCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopCameraCommand))]
    private bool _isIdle = true;

    public CameraViewModel(ICapture camera)
    {
        _camera = camera;
    }

    [RelayCommand]
    private async Task InitializePanel() => await RefreshCameraList();

    [RelayCommand(CanExecute = nameof(CanStartCamera))]
    private async Task StartCamera()
    {
        try
        {
            IsIdle = false;
            await Task.Run(() =>
            {
                ImageSize size = SelectedCamera!.SelectedImageSize!;
                _camera.FrameCallback += _onFrame;
                _camera.ErrorCallback += _onError;
                _camera.Initialize(SelectedCamera!.DeviceID, new Resolution { Width = size.Width, Height = size.Height });
                _camera.Start();
            });
        }
        catch(Exception error) { WeakReferenceMessenger.Default.Send(new ShowNormalDialogMessage($"Start Camera Error: {error.Message}")); }
    }

    private bool CanStartCamera()
    {
        return this.Cameras.Count > 0 && this.SelectedCamera != null && this.SelectedCamera.SelectedImageSize != null && IsIdle;
    }

    [RelayCommand(CanExecute =nameof(CanStopCamera))]
    private async Task StopCamera()
    {
        try
        {
            await Task.Run(() =>
            {
                _camera.FrameCallback -= _onFrame;
                _camera.Stop();
                _camera.Close();
                _camera.ErrorCallback -= _onError;
                Image = null;
            }).ContinueWith(t => Application.Current.Dispatcher.Invoke(() => IsIdle = true));
        }
        catch (Exception error) { WeakReferenceMessenger.Default.Send(new ShowNormalDialogMessage($"Stop Camera Error: {error.Message}")); }
    }

    private bool CanStopCamera() => !IsIdle;

    [RelayCommand]
    private async Task RefreshCameraList()
    {
        try
        {
            Cameras.Clear();
            var camera_list = await WebcamDevice.GetWebcamInfoList();
            foreach (var camera in camera_list)
            {
                CameraModel camera_model = new CameraModel
                {
                    CameraName = camera.Name,
                    DeviceID = camera.DeviceID,
                    Resolutions = new ObservableCollection<ImageSize>(camera.Resolutions.Select(x => new ImageSize { Width = x.Width, Height = x.Height })),
                };
                camera_model.SelectedImageSize = camera_model.Resolutions.FirstOrDefault();
                Cameras.Add(camera_model);
            }
            if (Cameras.Count > 0) SelectedCamera = Cameras[0];
        }
        catch(Exception error) 
        {
            WeakReferenceMessenger.Default.Send(new ShowNormalDialogMessage($"Get Camera List Error: {error.Message}"));
        }
    }

    private void _onError(Exception error)
    {
        Application.Current.Dispatcher.Invoke(() => WeakReferenceMessenger.Default.Send(new ShowNormalDialogMessage($"Camera Error: {error.Message}")));
    }

    private void _onFrame(IntPtr pointer, int width, int height, int depth)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (Image == null) Image = new WriteableBitmap(width, height, 96, 96, depth == 3 ? PixelFormats.Bgr24 : PixelFormats.Bgra32, null);
            Image.Lock();
            WebcamDevice.CopyMemory(Image.BackBuffer, pointer, (uint)(width * height * depth));
            Image.AddDirtyRect(new Int32Rect(0, 0, width, height));
            Image.Unlock();
        });
    }
}
