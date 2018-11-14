using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using Plugin.Media;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class CameraViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private ImageSource _image = new UriImageSource
        {
            CachingEnabled = false,
            Uri = new Uri(
                "https://images.pexels.com/photos/67636/rose-blue-flower-rose-blooms-67636.jpeg?auto=compress&amp;cs=tinysrgb&amp;h=350")
        };

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public CameraViewModel()
        {
            Title = "Camera";

            OpenWebCommand = new Command(TryStartCamera);
        }

        public async void TryStartCamera()
        {
            Debug.WriteLine("Loading Camera");
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("No Camera");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
            {
                Debug.WriteLine("FILE IS NULL!");
                return;
            }

            Debug.WriteLine($"File Location: {file.Path}, OK");

            Image = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            Debug.WriteLine("Image updated");
        }

        public ICommand OpenWebCommand { get; }
    }
}
