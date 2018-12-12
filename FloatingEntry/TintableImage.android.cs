using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Xam.Plugins.Forms.FloatingEntry;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(TintableImage), typeof(TintableImageRenderer))]

namespace Xam.Plugins.Forms.FloatingEntry
{
    public class TintableImageRenderer : ImageRenderer
    {
        public TintableImageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            SetTint();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == TintableImage.TintColorProperty.PropertyName ||
                e.PropertyName == Image.SourceProperty.PropertyName)
                SetTint();
        }

        private void SetTint()
        {
            if (Control == null || Element == null)
                return;

            if (((TintableImage) Element).Tint.Equals(Color.Transparent))
            {
                //Turn off tinting

                if (Control.ColorFilter != null)
                    Control.ClearColorFilter();

                return;
            }

            //Apply tint color
            var colorFilter =
                new PorterDuffColorFilter(((TintableImage) Element).Tint.ToAndroid(), PorterDuff.Mode.SrcIn);
            Control.SetColorFilter(colorFilter);
        }
    }
}