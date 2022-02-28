using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toast;
using WalkingForRochester.Controls;
using WalkingForRochester.Models;
using WalkingForRochester.Services;
using WalkingForRochester.Views;
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
    public class ResetPasswordViewModel : BaseViewModel
    {
        public ResetPasswordViewModel()
        {
            resetPasswordModel = new ResetPasswordModel();
            SendCommand = new Command(OnSendClicked, ValidateCaptcha);
            ResetCommand = new Command(OnResetCommand, ValidateEntry);
            PropertyChanged += (_, __) => SendCommand.ChangeCanExecute();
            PropertyChanged += (_, __) => ResetCommand.ChangeCanExecute();
        }

        private bool ValidateEntry()
        {
            return ResetText switch
            {
                "Next" => !string.IsNullOrEmpty(CaptchaCode) && CaptchaCode.Count() >= 6,
                "Reset Password" => !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword),
                _ => false
            };
        }

        private bool ValidateCaptcha()
        {
            return !string.IsNullOrEmpty(EmailAddress) &&
                   CheckEmail(EmailAddress);
        }

        private async void OnSendClicked()
        {
            if (!string.IsNullOrEmpty(EmailAddress))
            {
                // Check email address exists
                var accounts = await RestService.Get.GetAccount();
                var flag = accounts.Any(i => i.Email == EmailAddress);

                if (flag)
                {
                    accounts = accounts.Where(i => i.Email == EmailAddress).ToList();
                    var account = accounts[0];
                    resetPasswordModel.UserId = account.Id;

                    SendEmail(EmailAddress);
                    SendVisible = false;
                    CodeVisible = true;
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError("Send failed! Please check your email address.");
                }
            }
            else
            {
                CrossToastPopUp.Current.ShowToastWarning("Please enter your email address.");
            }
        }

        private async void OnResetCommand()
        {
            if (ResetText == "Next")
            {
                if (CaptchaCode == code)
                {
                    SendEmailFrame = false;
                    ResetPasswordFrame = true;
                    ResetText = "Reset Password";
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError("Code does not match. Please try again.");
                }
            }
            else
            {
                ResetEnabled = false;
                await ResetPassword(NewPassword, ConfirmPassword);
            }
        }

        public async Task ResetPassword(string n, string c)
        {
            try
            {
                if (n == c)
                {
                    resetPasswordModel.Password = ToolKit.Encrypt32Bit(n);
                    if (await RestService.Update.ResetPassword(resetPasswordModel))
                    {
                        Application.Current.MainPage = new LoginPage();
                        CrossToastPopUp.Current.ShowToastSuccess("Modify password had been successfully changed.");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please talk with the Administrator.",
                            "Ok");
                    }
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError("Password mismatch");
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please talk with Administrator.", "Ok");
            }
        }

        #region Email

        private static bool CheckEmail(string e)
        {
            const string regex = "([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,5})+";
            var r = GetPathPoint(e, regex);
            return r != null && ((IList) r).Contains(e);
        }

        private static string[] GetPathPoint(string value, string regx)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var isMatch = Regex.IsMatch(value, regx);

            if (!isMatch) return null;

            var matchCol = Regex.Matches(value, regx);

            var result = new string[matchCol.Count];

            if (matchCol.Count > 0)
                for (var i = 0; i < matchCol.Count; i++)
                    result[i] = matchCol[i].Value;

            return result;
        }

        private async void SendEmail(string toEmail)
        {
            try
            {
                var gmail = await RestService.Get.GetGmailInfo();

                // Generate Code
                code = CreateRandomCode();

                var mail = new MailMessage
                {
                    // Sender Email Address
                    From = new MailAddress(gmail.SenderMailAddress),

                    // Recipient Email Address
                    To = {toEmail},
                    // Subject
                    Subject = gmail.Subject,

                    // Body
                    Body = $"Reset Password Code: {code}"
                };

                var client = new SmtpClient("smtp.gmail.com")
                {
                    // Post
                    Port = 587,

                    // SSL 
                    EnableSsl = true,

                    //Credentials - Username: Sender Email || Password: Application Authorization Code
                    Credentials = new NetworkCredential(
                        gmail.CredentialUsername,
                        gmail.CredentialPassword
                    )
                };

                client.Send(mail);

                // Toast message
                CrossToastPopUp.Current.ShowToastSuccess("Send successfully.");
            }
            catch
            {
                CrossToastPopUp.Current.ShowToastError("Send failed! Please check your email address.");
            }
        }

        // Generate CAPTCHA
        public string CreateRandomCode(int length = 6)
        {
            var l = "1234567890";
            var r = new Random();
            var c = "";

            for (var i = 0; i < length; i++) c += l[r.Next(0, l.Length - 1)];

            return c;
        }

        #endregion

        #region Fields

        private string code = "";

        private readonly ResetPasswordModel resetPasswordModel;

        #endregion

        #region Commands

        public Command SendCommand { get; }
        public Command ResetCommand { get; }

        private ICommand cancelCommand;

        public ICommand CancelCommand => cancelCommand ??= new Command(() =>
        {
            Application.Current.MainPage = new LoginPage();
        });

        #endregion

        #region Props

        private string emailAddress;
        private string captchaCode;
        private string newPassword;
        private string confirmPassword;

        public string EmailAddress
        {
            get => emailAddress;
            set => SetProperty(ref emailAddress, value);
        }

        public string CaptchaCode
        {
            get => captchaCode;
            set => SetProperty(ref captchaCode, value);
        }

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        #endregion

        #region Control Props

        private string sendText = "Send";
        private string resetText = "Next";
        private bool sendEmailFrame = true;
        private bool resetPasswordFrame;
        private bool resetEnabled;

        private bool sendVisible = true;
        private bool codeVisible;

        public bool SendVisible
        {
            get => sendVisible;
            set => SetProperty(ref sendVisible, value);
        }

        public bool CodeVisible
        {
            get => codeVisible;
            set => SetProperty(ref codeVisible, value);
        }

        public bool SendEmailFrame
        {
            get => sendEmailFrame;
            set => SetProperty(ref sendEmailFrame, value);
        }

        public bool ResetPasswordFrame
        {
            get => resetPasswordFrame;
            set => SetProperty(ref resetPasswordFrame, value);
        }

        public bool ResetEnabled
        {
            get => resetEnabled;
            set => SetProperty(ref resetEnabled, value);
        }

        public string ResetText
        {
            get => resetText;
            set => SetProperty(ref resetText, value);
        }

        public string SendText
        {
            get => sendText;
            set => SetProperty(ref sendText, value);
        }

        #endregion
    }
}