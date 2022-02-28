using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingForRochester.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsFeedPage : ContentPage
    {
        public NewsFeedPage()
        {
            InitializeComponent();
            // BindingContext = new NewsFeedViewModel();
            // BindingContext = this;
        }

        // private async Task OpenAnimation(View view, uint length = 250)
        // {
        //     view.RotationX = -90;
        //     view.IsVisible = true;
        //     view.Opacity = 0;
        //     _ = view.FadeTo(1, length);
        //     await view.RotateXTo(0, length);
        // }
        //
        // private async Task CloseAnimation(View view, uint length = 250)
        // {
        //     _ = view.FadeTo(0, length);
        //     await view.RotateXTo(-90, length);
        //     view.IsVisible = false;
        // }
        //
        // private async void MainExpander_Tapped(object sender, EventArgs e)
        // {
        //     if (!IsBusy)
        //     {
        //         var expander = sender as Expander;
        //         var imgView = expander.FindByName<Grid>("ImageView");
        //         var detailsView = expander.FindByName<Grid>("DetailsView");
        //
        //         if (expander.IsExpanded)
        //         {
        //             await OpenAnimation(imgView);
        //             await OpenAnimation(detailsView);
        //         }
        //         else
        //         {
        //             await CloseAnimation(detailsView);
        //             await CloseAnimation(imgView);
        //         }
        //     }
        // }
    }
}