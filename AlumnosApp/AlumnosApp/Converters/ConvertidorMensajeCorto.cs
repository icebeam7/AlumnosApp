using System;
using System.Globalization;
using Xamarin.Forms;

namespace AlumnosApp.Converters
{
    public class ConvertidorMensajeCorto : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var Mensaje = value.ToString();
                return (Mensaje.Length > 50) ? Mensaje.Substring(0, 50) : Mensaje;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
