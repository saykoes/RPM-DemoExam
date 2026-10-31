using Lab1.Models;
using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        private int _roleId = 0;
        private ObservableCollection<Product> _products = new();
        private CollectionViewSource _collectionViewSource;
        public ListWindow(Models.User? user = null)
        {
            InitializeComponent();
            
            if (user is not null)
            {
                _roleId = user.RoleId!.Value;
                var userRole = user.Role!.Name;
                var userName = user.FullName;

                this.Title = $"{userRole} : {userName}";
                UserNameText.Text = userName;

                if(_roleId > 1) // manager or admin
                {
                    // sort filter search
                }
                if (_roleId == 3)
                {
                    // 'add' button

                }
            }

            _collectionViewSource = new()
            {
                Source = _products
            };

            _collectionViewSource.Filter += ItemsFilter;
            itemsControl.ItemsSource = _collectionViewSource.View;

            LoadItems();
        }

        private void ItemsFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void LoadItems()
        {
            using var context = new DemoExamMakarovContext();

            _products.Clear();
            var items = context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Provider).ToList();

            foreach (var item in items)
            {
                _products.Add(item);
            }
        }
    }
}
