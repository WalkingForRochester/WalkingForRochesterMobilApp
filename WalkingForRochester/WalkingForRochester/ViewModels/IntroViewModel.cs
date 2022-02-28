using System.Windows.Input;
using WalkingForRochester.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WalkingForRochester.ViewModels
{
    public class IntroViewModel : BaseViewModel
    {
        private ICommand okCommand;

        public ICommand OkCommand => okCommand ??= new Command(() => Application.Current.MainPage = new LoginPage());
    }
}