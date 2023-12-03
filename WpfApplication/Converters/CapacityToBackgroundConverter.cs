using System.Windows.Data;
using System.Windows.Media;

namespace WpfApplication.Converters;

public class CapacityToBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int capacity)
        {
            return capacity > 0 ? Brushes.White : Brushes.Gray;
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}