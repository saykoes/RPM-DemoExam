using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfAppLab7
{
    class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string filename = (string)value ?? Domain.AppContext.MOCK_IMAGE_NAME;
                string fullpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filename);

                Uri fileUri = new Uri(fullpath, UriKind.Absolute);
                ImageSource imageSource = BitmapFrame.Create(fileUri, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
                return imageSource;
            }
            catch { return new BitmapImage(); }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
