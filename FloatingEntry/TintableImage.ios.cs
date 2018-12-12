using System.ComponentModel;
using UIKit;
using Xam.Plugins.Forms.FloatingEntry;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TintableImage), typeof(TintableImageRenderer))]

namespace Xam.Plugins.Forms.FloatingEntry
{
    public class TintableImageRenderer : ImageRenderer
    {
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
            if (Control?.Image == null || Element == null)
                return;

            if (((TintableImage) Element).Tint == Color.Transparent)
            {
                //Turn off tinting
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);
                Control.TintColor = null;
            }
            else
            {
                //Apply tint color
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = ((TintableImage) Element).Tint.ToUIColor();
            }
        }
    }
}