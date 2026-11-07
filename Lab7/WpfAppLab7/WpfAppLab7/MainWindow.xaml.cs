using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppLab7.Models;

namespace WpfAppLab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            loginBox.Text = "94d5ous@gmail.com";
            passwordBox.Password = "uzWC67";
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new ListWindow(null);
            newWindow.Show();
            this.Close();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {   
            bool emptyFields = loginBox.Text.IsNullOrEmpty() || passwordBox.Password.IsNullOrEmpty();
            if (emptyFields)
            {
                MessageBox.Show("Пустой логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Lab7ShopSaikouContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login.Trim() == loginBox.Text.Trim() && u.Password.Trim() == passwordBox.Password.Trim());

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newWindow = new ListWindow(user);
                newWindow.Show();
                this.Close();
            } 


        }
    }
}