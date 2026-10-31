using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
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
using WpfShopAppSaikou.Models;

namespace WpfShopAppSaikou
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Product> _products = new();
        private CollectionViewSource _source;

        public MainWindow()
        {
            InitializeComponent();
            _source = new()
            {
                Source = _products
            };
            _source.Filter += ItemsFilter;
            searchField.TextChanged += SeacrhField_TextChangedEvent;

            itemsControl.ItemsSource = _source.View;
            LoadItems();
        }


        private void ItemsFilter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(searchField.Text))
            {
                e.Accepted = true;
            }
            else if (e.Item is Product p)
            {
                string searchText = searchField.Text.Trim().ToLower();

                bool matchesName = p.Name?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesCathegory = p.Category?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesArticle = p.Article?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesManufacturer = p.Manufacturer?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesSupplier = p.Supplier?.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;

                e.Accepted = matchesName || matchesCathegory || matchesArticle || matchesManufacturer || matchesSupplier;
            }
            
        }

        private void SeacrhField_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            _source.View.Refresh();
        }

        private void LoadItems()
        {
            using var context = new Lab4ShopSaikouContext();
            _products.Clear();
            var items = context.Products
                .Include(p => p.Category)
                .Include(p => p.Name)
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier);
            foreach (var item in items)
            {
                _products.Add(item);
            }
            
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product();
            var editWindow = new EditWindow(newProduct);

            if (editWindow.ShowDialog() == true)
            {
                using var context = new Lab4ShopSaikouContext();
                var databaseProduct = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Name)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .FirstOrDefault(p => p.Id == newProduct.Id);

                if (databaseProduct != null)
                {
                    _products.Add(databaseProduct);
                }
            }
        }

        private void ShopItemControl_ProductDeleted(object sender, Product product)
        {
            try
            {
                using (var db = new Lab4ShopSaikouContext())
                {
                    db.Products.Attach(product);
                    db.Products.Remove(product);
                    db.SaveChanges();
                }

                _products.Remove(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShopItemControl_ProductEdited(object sender, Product product)
        {
            using (var context = new Lab4ShopSaikouContext())
            {
                var databaseProduct = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Name)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .FirstOrDefault(p => p.Id == product.Id);

                if (databaseProduct != null)
                {
                    int index = _products.IndexOf(product);
                    if (index >= 0)
                    {
                        _products[index] = databaseProduct;
                    }
                }
            }

            _source.View.Refresh();
        }
    }
}