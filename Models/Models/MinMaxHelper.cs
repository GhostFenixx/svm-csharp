using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Greed.Models
{
    //Disclaimer: Vibe coded, essentially for handling minimum/maximums outside xaml and instead utilise classes get-sets.
    //In the end i plan to reduce amount of styles that i've made to utilise front-end limits, may also remove some of the converters as well.
    //Not feelin' guilty tho, this is just for operating values on UI/UX level.
    public abstract class Clamper : INotifyPropertyChanged
    {
        private SynchronizationContext? _uiContext;
        private PropertyChangedEventHandler? _propertyChanged;

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                // Bindings subscribe on the UI thread, so this captures the WPF context.
                // In a non-UI host it stays null and notifications are raised directly.
                _uiContext ??= SynchronizationContext.Current;
                _propertyChanged += value;
            }
            remove => _propertyChanged -= value;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected void SetClamped(ref int field, int value, int min, int max,
                                  [CallerMemberName] string? name = null)
        {
            int clamped = Math.Clamp(value, min, max);
            bool changed = field != clamped;
            field = clamped;
            Notify(changed, value != clamped, name);
        }

        protected void SetClamped(ref double field, double value, double min, double max,
                                  [CallerMemberName] string? name = null)
        {
            if (double.IsNaN(value)) value = field;   // NumberBox sends NaN when cleared

            double clamped = Math.Clamp(value, min, max);
            bool changed = field != clamped;
            field = clamped;
            Notify(changed, value != clamped, name);
        }

        private void Notify(bool changed, bool corrected, string? name)
        {
            if (changed && !corrected)
            {
                OnPropertyChanged(name);
            }
            else if (corrected)
            {
                // Deferred when a UI context exists, so WPF doesn't swallow the
                // notification mid-binding-update and the box snaps back.
                if (_uiContext != null)
                    _uiContext.Post(_ => OnPropertyChanged(name), null);
                else
                    OnPropertyChanged(name);
            }
        }
    }
}
