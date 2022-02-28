using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

/// <summary>
/// Authors: 
///         NTID MAD TEAM
///             Mangers:        Xiangyu Shi
///                             Michelle Olson
///                         
///             Programmer:     Xiangyu Shi, 
///                             Michelle Olson, 
///                             Zhencheng Chen,
///                             Harsh Mathur,
///                             Quoc Nhan,
///                             Chase Roth  
/// </summary>

namespace WalkingForRochester.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        private ICommand emailCommand;
        private ICommand facebookCommand;
        private ICommand phoneCommand;
        private ICommand officialCommand;

        public ICommand EmailCommand => emailCommand
            ??= new Command(async () =>
                await Email.ComposeAsync("Contact", "", "walkingforrochester@gmail.com"));

        public ICommand PhoneCommand => phoneCommand
            ??= new Command(() => PhoneDialer.Open("5853586888"));

        public ICommand FacebookCommand => facebookCommand
            ??= new Command(async () =>
                {
                    await Browser.OpenAsync("https://www.facebook.com/walkingforrochester/");
                }
            );

        public ICommand OfficialCommand => officialCommand
            ??= new Command(async () =>
            {
                await Browser.OpenAsync("https://walkingforrochester.org/");
            });
    }
}