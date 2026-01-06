using System.Globalization;
using YuvaCep.Mobile.Enums; 

namespace YuvaCep.Mobile.Converters
{
    public class ThemeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int themeValue = -1;

            if (value is int intVal)
                themeValue = intVal;
            else if (value is ChartTheme themeEnum)
                themeValue = (int)themeEnum;

            return themeValue switch
            {
                0 => "🦷", 
                1 => "⚽", 
                2 => "📚", 
                3 => "🎨", 
                4 => "🥕", 
                _ => "📅"  // Default
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}