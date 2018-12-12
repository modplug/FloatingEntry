using Xamarin.Forms;

namespace Xam.Plugins.Forms.FloatingEntry
{
    public class TintableImage : Image
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create(nameof(Tint),
                typeof(Color),
                typeof(TintableImage),
                default(Color));

        public Color Tint
        {
            get => (Color) GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}