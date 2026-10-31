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
using WpfAppLab5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WpfAppLab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            loginBox.Text = "uth4iz@mail.com";
            passwordBox.Password = "2L6KZG";
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            var login = loginBox.Text;
            var password = passwordBox.Password;

            if (login.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Lab5ShopSaikouContext())
            {
                var user = db.Users.Include(x => x.FkUserRole).FirstOrDefault(x => x.Login.Trim() == login.Trim() && x.Password.Trim().Trim() == password);
                if (user is null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newWindow = new Window1(user);
                newWindow.Show();
                this.Close();
            }
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window1(null);
            newWindow.Show();
            this.Close();
        }
    }
}