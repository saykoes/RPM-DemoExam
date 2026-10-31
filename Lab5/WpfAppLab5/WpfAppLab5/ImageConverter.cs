using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WpfAppLab5
{
    internal class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string filename = ((string?)value) ?? Domain.AppContext.MOCK_IMAGE_NAME;
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filename);

                Uri fileUri = new Uri(fullPath, UriKind.Absolute);
                ImageSource image = BitmapFrame.Create(fileUri, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
                return image;
            }
            catch { return new BitmapImage(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
