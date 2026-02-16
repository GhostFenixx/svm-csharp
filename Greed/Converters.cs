using ModernWpf.Controls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace Greed
{
    public class TwoDecimalPlacesFormatter : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            value = Math.Round(value, 2);
            return Math.Clamp(value, 0, 100).ToString("F2"); //value.ToString("F2");
        }

        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }
    public class ThreeDecimalPlacesFormatter : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            value = Math.Round(value, 3);
            return Math.Clamp(value, 0, 100).ToString("F3"); //value.ToString("F2");
        }

        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }
    public class TwoDecimalPlacesFormatterWithNegative : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            value = Math.Round(value, 2);
            return value.ToString("F2"); //value.ToString("F2");
        }

        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }
    public class ValidityFormatter : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            return Math.Clamp(value, 0, 100).ToString();
        }
        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }

    public class ForceToOneInteger : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            return Math.Clamp(value, 1, 168).ToString();
        }
        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }
    public class UpperLimit : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            return Math.Clamp(value, 0, 1000000).ToString();

        }
        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }
    }
    public static class NumberBoxClampBehavior
    {
        public static readonly DependencyProperty EnableClampProperty =
            DependencyProperty.RegisterAttached(
                "EnableClamp",
                typeof(bool),
                typeof(NumberBoxClampBehavior),
                new PropertyMetadata(false, OnEnableClampChanged));

        public static void SetEnableClamp(DependencyObject obj, bool value)
            => obj.SetValue(EnableClampProperty, value);

        public static bool GetEnableClamp(DependencyObject obj)
            => (bool)obj.GetValue(EnableClampProperty);

        private static void OnEnableClampChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
            {
                nb.ValueChanged -= OnValueChanged;

                if ((bool)e.NewValue)
                    nb.ValueChanged += OnValueChanged;
            }
        }

        private static void OnValueChanged(
            NumberBox sender,
            NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(sender.Value))
                return;

            double value = sender.Value;

            if (value < sender.Minimum)
                sender.Value = sender.Minimum;
            else if (value > sender.Maximum)
                sender.Value = sender.Maximum;
        }
    }

}

//internal class Converters
//{
// public class BooleanConverter<T> : IValueConverter
// {
//  public BooleanConverter(T trueValue, T falseValue)
//  {
//   True = trueValue;
//   False = falseValue;
//  }

//  public T True { get; set; }
//  public T False { get; set; }

//  public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//  {
//   return value is bool && ((bool)value) ? True : False;
//  }

//  public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//  {
//   return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
//  }
// }

// public sealed class BooleanToVisibilityConverter2 : BooleanConverter<Visibility>
// {
//  public BooleanToVisibilityConverter2() : base(Visibility.Visible, Visibility.Collapsed) { }
// }
//}
