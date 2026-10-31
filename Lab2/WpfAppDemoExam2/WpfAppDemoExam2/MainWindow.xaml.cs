using Microsoft.EntityFrameworkCore;
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

namespace WpfAppDemoExam2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loginBox.Text = "user2";
            passwordBox.Password = "asdfgh456";
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(loginBox.Text))
            {
                MessageBox.Show("Login is empty","Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Password is empty", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = new Book2307b2SaykoEsContext();

            var user = context.Users.FirstOrDefault( u => u.UserLogin == loginBox.Text && u.UserPassword == passwordBox.Password);

            if (user is null)
            {
                MessageBox.Show("User is not found", "Auth Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new Window1(user);
            newWindow.Show();
            this.Close();

        }

        private void signInAsGuestButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window1(null);
            newWindow.Show();
            this.Close();
        }
    }
}