using Lab1.Models;
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

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var login = loginField.Text;
            var password = passwordField.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fill every field!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using var context = new DemoExamMakarovContext();
            var user = context.Users.Include(u => u.Role).FirstOrDefault(u =>
                                            u.Login == login &&
                                            u.Password == password);
            if (user is null)
            {
                MessageBox.Show("Login or password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (user.RoleId is null)
            {
                MessageBox.Show("User without Role", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            var window = new ListWindow(user);
            window.Show();
            Close();
        }
        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            var window = new ListWindow(null);
            window.Show();
            Close();
        }
    }
}