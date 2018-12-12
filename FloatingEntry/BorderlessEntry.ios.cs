using Xam.Plugins.Forms.FloatingEntry;
using BorderlessEntryRenderer = Xam.Plugins.Forms.FloatingEntry.FloatingEntry;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace Xam.Plugins.Forms.FloatingEntry
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}