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
using Microsoft.EntityFrameworkCore;

namespace WpfAppDemoExam2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private User? _user;
        private ObservableCollection<Book> _books = new();
        private CollectionViewSource? _booksSource;
        public Window1(User? user)
        {
            InitializeComponent();
            if (user is not null)
            {
                _user = user;
                userNameLabel.Content = _user.RoleId;
                if (user.RoleId > 1)
                {
                    var userGridWindow = new Window2();
                    userGridWindow.Show();
                }
            }

            _booksSource = new()
            {
                Source = _books
            };

            _booksSource.Filter += BooksFilter;
            itemsControl.ItemsSource = _booksSource.View;

            LoadItems();
        }

        private void BooksFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        private void LoadItems()
        {
            using var context = new Book2307b2SaykoEsContext();

            _books.Clear();
            var books = context.Books.ToList();
            //var books = context.Books.Include(b => b.BookTitle).ToList();

            foreach (var book in books)
            {
                _books.Add(book);
            }
        }

        private void signOutButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }
    }
}
