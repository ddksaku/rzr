using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Rzr.Core.Converters
{
    [ValueConversion(typeof(System.Drawing.Bitmap), typeof(ImageSource))]
    public class BitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bmp = value as System.Drawing.Bitmap;
            if (bmp == null)
                return null;
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        bmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
