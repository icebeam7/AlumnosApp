using System;
using System.Globalization;
using Xamarin.Forms;

namespace AlumnosApp.Converters
{
    public class ConvertidorEvaluacionTarea : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return ((bool)value) ? "Tarea evaluada" : "Pendiente de evaluar";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
