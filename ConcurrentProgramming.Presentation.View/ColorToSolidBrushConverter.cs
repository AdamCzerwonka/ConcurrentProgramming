using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ConcurrentProgramming.Presentation.View;

public class ColorToSolidBrushConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is System.Drawing.Color)
        {
            var valueColor = (System.Drawing.Color)value;
            var color = new Color
            {
                A = valueColor.A,
                R = valueColor.R,
                G = valueColor.G,
                B = valueColor.B
            };
            return new SolidColorBrush(color);
        }

        var type = value.GetType();
        throw new InvalidOperationException($"Unsuported type: {type.Name}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}