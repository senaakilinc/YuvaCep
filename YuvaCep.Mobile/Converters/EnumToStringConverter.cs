using System.Globalization;
using YuvaCep.Mobile.Enums;

namespace YuvaCep.Mobile.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FoodStatus food)
            {
                return food switch
                {
                    FoodStatus.HepsiniYedi => "Tamamını Yedi 😋",
                    FoodStatus.YarisiniYedi => "Yarısını Yedi ½",
                    FoodStatus.AzYedi => "Az Yedi 🤏",
                    FoodStatus.Yemedi => "Yemedi ❌",
                    _ => value.ToString()
                };
            }

            if (value is SleepStatus sleep)
            {
                return sleep switch
                {
                    SleepStatus.Uyudu => "Uyudu 😴",
                    SleepStatus.Uyumadi => "Uyumadı 😳",
                    SleepStatus.AzUyudu => "Az Uyudu 🥱",
                    _ => value.ToString()
                };
            }

            if (value is ActivityStatus activity)
            {
                return activity switch
                {
                    ActivityStatus.Katildi => "Tam Katılım ✅",
                    ActivityStatus.KismenKatildi => "Kısmen Katıldı ⚠️",
                    ActivityStatus.Katilmadi => "Katılmadı ❌",
                    _ => value.ToString()
                };
            }

            if(value is MoodStatus mood)
            {
                return mood switch
                {
                    MoodStatus.CokUzgun => "Çok Üzgün",
                    MoodStatus.Uzgun => "Üzgün",
                    MoodStatus.Normal => "Normal",
                    MoodStatus.Mutlu => "Mutlu",
                    MoodStatus.Harika => "Harika",
                    _ => value.ToString()
                };
            }

            // Eğer hiçbiri değilse olduğu gibi göster
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}