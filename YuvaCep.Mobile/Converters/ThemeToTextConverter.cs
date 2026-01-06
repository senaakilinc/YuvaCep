using System.Globalization;
using YuvaCep.Mobile.Enums; 

namespace YuvaCep.Mobile.Converters
{
    public class ThemeToTextConverter : IValueConverter
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
                0 => "Kişisel Bakım & Hijyen",
                1 => "Spor & Egzersiz",       
                2 => "Eğitim & Kitap Okuma",  
                3 => "Sanat & Yaratıcılık",
                4 => "Sağlıklı Beslenme",
                _ => "Genel Aktivite"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}