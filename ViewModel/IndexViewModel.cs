using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Zwiggy.AppData;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.Model;
using Zwiggy.Core.Utility;

namespace Zwiggy.ViewModel
{
    class IndexViewModel : BindingsNotification
    {
        private bool _isLoginPaneVisible = true;
        private bool _cartViewRequested = false;
        private string _message = "";
        private string _typeOfMessage = "";

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                OnPropertyChange(nameof(Message));
            }
        }

        public bool CartViewRequested
        {
            get
            {
                return _cartViewRequested;
            }

            set
            {
                _cartViewRequested = value;
                OnPropertyChange(nameof(CartViewRequested));
            }
        }

        public Color GetMessageBackground
        {
            get
            {
                if (TypeOfMessage.Equals("Error!"))
                {
                    return Color.FromArgb(255, 248, 196, 180);
                }

                return Color.FromArgb(255, 234, 231, 177);
            }
        }

        public Color GetMessageForeground
        {
            get
            {
                if (TypeOfMessage.Equals("Error!"))
                {
                    return Color.FromArgb(255, 133, 0, 0);
                }

                return Color.FromArgb(255, 60, 98, 85);
            }
        }

        public string TypeOfMessage
        {
            get
            {
                return _typeOfMessage;
            }

            set
            {
                _typeOfMessage = value;
                OnPropertyChange(nameof(TypeOfMessage));
                OnPropertyChange(nameof(GetMessageBackground));
                OnPropertyChange(nameof(GetMessageForeground));
            }
        }

        public bool IsLoginPaneVisible
        {
            get
            {
                return _isLoginPaneVisible;
            }

            set
            {
                _isLoginPaneVisible = value;
                OnPropertyChange(nameof(IsLoginPaneVisible));
                OnPropertyChange(nameof(IsSignupPaneVisible));
            }
        }

        public bool IsSignupPaneVisible
        {
            get
            {
                return !_isLoginPaneVisible;
            }

            set
            {
                _isLoginPaneVisible = !value;
                OnPropertyChange(nameof(IsLoginPaneVisible));
                OnPropertyChange(nameof(IsSignupPaneVisible));
            }
        }

        public double ViewportHeight
        {
            get
            {
                return Window.Current.Bounds.Height;
            }
        }

        public double WarningPosition
        {
            get
            {
                return ViewportHeight - 70;
            }
        }

        public async Task UpdateTransitionTextblock(TextBlock transitionTextBlock,Storyboard transitionStoryBoard)
        {
            async Task SetAnimation(string test)
            {
                transitionTextBlock.Text = test;
                transitionStoryBoard.Begin();

                await Task.Delay(3000);
            }

            string[] captions = { "Hungry?", "Unexpected guests?", "Cooking gone wrong?", "Movie marathon?", "Game night?", "Late night at office?" };

            while (true)
            {
                foreach (string caption in captions)
                {
                    await SetAnimation(caption);
                }
            }
        }

        public async Task<bool> BeginSignup(User user)
        {
            UserSignupDataManager dataManager = new UserSignupDataManager();

            if (!await dataManager.UserExists(user.Email))
            {
                dataManager.AddUser(user);
                TypeOfMessage = "Success!";
                Message = " User has been signed up.";
                return true;
            }

            TypeOfMessage = "Error!";
            Message = " User already exist.";

            return false;
        }

        public async Task<bool> BeginLogin(User user)
        {
            UserLoginDataManager dataManager = new UserLoginDataManager();

            if (await dataManager.UserExists(user.Email))
            {
                if(await dataManager.ValidateUserCredentials(user))
                {
                    //await Database.GetCartAsync(connection, user);
                    //await Database.GetOrdersAsync(connection, user);
                    //await Database.GetRatingsAsync(connection, user);
                    Repository.CurrentUser = user;
                    return true;
                }
                else
                {
                    TypeOfMessage = "Error!";
                    Message = " User credentials is wrong.";
                }
            }
            else
            {
                TypeOfMessage = "Error!";
                Message = " User doesn't exist.";
            }

            return false;
        }

        public void ResumeSession(string email)
        {
            Repository.CurrentUser = new User
            {
                Email = email
            };
        }
    }
}
