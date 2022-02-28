using Android.Content;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace WalkingForRochester.Droid
{
    [assembly: ExportRenderer(typeof(CollectionView), typeof(CustomCollectionViewRenderer))]
    public class CustomCollectionViewRenderer : CollectionViewRenderer
    {
        public CustomCollectionViewRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ItemsView> elementChangedEvent)
        {
            base.OnElementChanged(elementChangedEvent);

            if (elementChangedEvent.NewElement != null)
            {
                
            }
        }
    }
}