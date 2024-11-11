using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RevitScriptETM
{
    public class BoolToNullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем значение на DBNull или null
            if (value == DBNull.Value || value == null)
            {
                return null; // Для состояния Indeterminate (квадратик)
            }

            // Если значение 1, значит чекбокс активирован (галочка)
            if (value.Equals(1))
            {
                return true; // Галочка
            }

            // Если значение 2, то это Indeterminate (квадратик)
            if (value.Equals(2))
            {
                return null; // Для состояния Indeterminate (квадратик)
            }

            // Если значение 0, то чекбокс не выбран (unchecked)
            return false; // Unchecked
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если значение true, то возвращаем 1 (галочка)
            if (value is bool boolValue)
            {
                return boolValue ? 1 : 0; // true - 1, false - 0
            }

            // Если значение null (Indeterminate), то возвращаем 2 (квадратик)
            return DBNull.Value; // Для состояния Indeterminate (квадратик)
        }
    }
}
