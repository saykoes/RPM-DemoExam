using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using static System.Net.Mime.MediaTypeNames;

namespace WpfShopAppSaikou
{
    /// <summary>
    /// Логика взаимодействия для ShopItemControl.xaml
    /// </summary>
    public partial class ShopItemControl : UserControl
    {
        public event EventHandler<Product>? ProductDeleted;
        public event EventHandler<Product>? ProductEdited;
        public ShopItemControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (DataContext is Product product)
            {
                using (var db = new Lab4ShopSaikouContext())
                {
                    if (db.OrderArticles.Any(oa => oa.ProductId == product.Id))
                    {
                        deleteButton.IsEnabled = false;
                    }
                    else
                    {
                        deleteButton.IsEnabled = true;
                    }
                }

                if (product.Discount > 20)
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C8A2C8")!;
                }
                else
                {
                    ProductGrid.Background = Brushes.Transparent;
                }

                if (product.Discount > 0)
                {
                    OldPriceText.Text = $"Цена: {product.Price} ";

                    NewPriceText.Text = (product.Price - (product.Price * (product.Discount / 100)))?.ToString("F2");
                }

            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Product product)
            {
                var editWindow = new EditWindow(product);
                if (editWindow.ShowDialog() == true)
                {
                    ProductEdited?.Invoke(this, product);
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Product product)
            {
                var result = MessageBox.Show($"Удалить {product?.Name?.Name}?","Удаление товара",MessageBoxButton.YesNo,MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ProductDeleted?.Invoke(this, product!);
                }
            }
        }
    }
}
