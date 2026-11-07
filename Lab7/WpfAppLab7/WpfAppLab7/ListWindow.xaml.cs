using Microsoft.EntityFrameworkCore;
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
using WpfAppLab7.Models;

namespace WpfAppLab7
{
    /// <summary>
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {

        private ObservableCollection<Product> _products = new();
        private CollectionViewSource _productsSource;
        private readonly Lab7ShopSaikouContext _context;

        public ListWindow(User? user)
        {
            InitializeComponent();

            _context = new Lab7ShopSaikouContext();

            if (user != null)
            {
                nameBox.Text = user.UserName;

                if (user.FkRoleId > 2)
                {
                    this.Title = "Администратор";
                }
                else if (user.FkRoleId > 1)
                {
                    this.Title = "Менеджер";
                }
                else if (user.FkRoleId > 0)
                {
                    this.Title = "Клиент";
                }
            }

            _productsSource = new CollectionViewSource
            {
                Source = _products,
            };

            _productsSource.Filter += ItemsFilter;
            itemsControl.ItemsSource = _productsSource.View;

            LoadItems();
        }

        private void ItemsFilter(object sender, FilterEventArgs f)
        {
            if (!(f.Item is Product product))
            {
                return;
            }

            f.Accepted = true;
        }

        private void LoadItems()
        {
            if (_context is not null)
            {
                _products.Clear();
                var items = _context.Products
                    .Include(p => p.FkManufacturer)
                    .Include(p => p.FkSupplier)
                    .Include(p => p.FkCategory).ToList();

                foreach ( var item in items)
                {
                    _products.Add(item);
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context.Dispose();
        }
    }
}
