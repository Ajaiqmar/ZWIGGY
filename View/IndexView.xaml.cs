using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zwiggy.Core.Model;
using Zwiggy.Service;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IndexView : Page
    {
        public IndexView()
        {
            this.InitializeComponent();
            AppearanceService.UpdateTitlebarForIndexView();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if((e.Parameter as string) != null && (e.Parameter as string).Equals("CartView"))
            {
                IndexVM.CartViewRequested = true;
            }
        }

        private async void OnTransitionTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock TransitionTextBlock = sender as TextBlock;
            await IndexVM.UpdateTransitionTextblock(TransitionTextBlock,TransitionStoryBoard);
        }

        private void OnCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            SignupStackpanel.Visibility = Visibility.Visible;
        }

        private async void BeginLogin(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(LoginEmailTextBox.Text) || string.IsNullOrWhiteSpace(LoginPasswordTextBox.Password))
            {
                IndexVM.TypeOfMessage = "Error!";
                IndexVM.Message = " Input cannot be empty.";

                WarningStoryBoard.Begin();
                await Task.Delay(5000);
                RemoveWarningStoryBoard.Begin();

                return;
            }

            bool userExists = true;
            User user = new User
            {
                Email = LoginEmailTextBox.Text,
                Password = LoginPasswordTextBox.Password
            };

            LoginSubmitButton.IsEnabled = false;

            if (!await IndexVM.BeginLogin(user))
            {
                LoginSubmitButton.IsEnabled = true;
                userExists = false;
            }

            if (userExists)
            {
                ApplicationData.Current.LocalSettings.Values["UserSessionObj"] = user.Email;
                ApplicationData.Current.LocalSettings.Values["VegFilter"] = "0";
                ApplicationData.Current.LocalSettings.Values["AccentColorType"] = "DarkOrangeGrid";
                Frame.Navigate(typeof(MainView));
            }
            else
            {
                WarningStoryBoard.Begin();
                await Task.Delay(5000);
                RemoveWarningStoryBoard.Begin();
            }
        }

        private void OnLoginRequestClicked(object sender, RoutedEventArgs e)
        {
            LoginStackpanel.Visibility = Visibility.Visible;
        }

        private async void BeginSignUp(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                IndexVM.TypeOfMessage = "Error!";
                IndexVM.Message = " Input cannot be empty.";

                WarningStoryBoard.Begin();
                await Task.Delay(5000);
                RemoveWarningStoryBoard.Begin();
                return;
            }

            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            if(!Regex.IsMatch(EmailTextBox.Text, regex, RegexOptions.IgnoreCase))
            {
                IndexVM.TypeOfMessage = "Error!";
                IndexVM.Message = " Email format is invalid.";

                WarningStoryBoard.Begin();
                await Task.Delay(5000);
                RemoveWarningStoryBoard.Begin();

                return;
            }

            SignupSubmitButton.IsEnabled = false;

            User user = new User
            {
                Email = EmailTextBox.Text,
                Name = UserNameTextBox.Text,
                Password = PasswordTextBox.Password
            };

            if (!await IndexVM.BeginSignup(user))
            {
                SignupSubmitButton.IsEnabled = true;
            }
            else
            {
                SignupSubmitButton.IsEnabled = true;
                LoginStackpanel.Visibility = Visibility.Visible;
            }

            WarningStoryBoard.Begin();
            await Task.Delay(5000);
            RemoveWarningStoryBoard.Begin();
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if((bool)LoginPasswordCheckBox.IsChecked || (bool)PasswordCheckBox.IsChecked)
            {
                LoginPasswordTextBox.PasswordRevealMode = PasswordRevealMode.Visible;
                PasswordTextBox.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                LoginPasswordTextBox.PasswordRevealMode = PasswordRevealMode.Hidden;
                PasswordTextBox.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private async void GuestUserLogin(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                Email = "",
                Password = ""
            };

            await IndexVM.BeginLogin(user);
            ApplicationData.Current.LocalSettings.Values["UserSessionObj"] = user.Email;
            ApplicationData.Current.LocalSettings.Values["VegFilter"] = "0";
            ApplicationData.Current.LocalSettings.Values["AccentColorType"] = "DarkOrangeGrid";
            Frame.Navigate(typeof(MainView));
        }

        private void OnIndexViewLoaded(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["UserSessionObj"] != null)
            {
                IndexVM.ResumeSession(ApplicationData.Current.LocalSettings.Values["UserSessionObj"] as string);

                if(IndexVM.CartViewRequested)
                {
                    Frame.Navigate(typeof(MainView),"CartView");
                }
                else
                {
                    Frame.Navigate(typeof(MainView));
                }
            }
        }
    }
}
