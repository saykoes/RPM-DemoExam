using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppDemoExam2
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private ObservableCollection<User> _users = new();
        public Window2()
        {
            InitializeComponent();
            UserGrid.ItemsSource = _users;
            LoadUsers();
        }

        private void LoadUsers()
        {
            using var context = new Book2307b2SaykoEsContext();
            _users.Clear();
            var usersList = context.Users.ToList();

            foreach (var user in usersList)
            {
                _users.Add(user);
            }
        }
    }
}
