using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private int _roleId = 0;
        private bool isEditWindowsOpened = false;

        public ListWindow(User? user)
        {
            InitializeComponent();

            _context = new Lab7ShopSaikouContext();

            if (user != null)
            {
                nameBox.Text = user.UserName;
                _roleId = user.FkRoleId ?? 0;

                if (user.FkRoleId > 2)
                {
                    this.Title = "Администратор";
                    addButton.Visibility = Visibility.Visible;
                    itemsControl.Cursor = Cursors.Hand;
                }
                else if (user.FkRoleId > 1)
                {
                    this.Title = "Менеджер";
                }
                else if (user.FkRoleId > 0)
                {
                    this.Title = "Клиент";
                }

                if (user.FkRoleId > 1)
                {
                    //manager + admin
                    searchFilterDock.Visibility = Visibility.Visible;
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
                f.Accepted = false;
                return;
            }

            bool matchesSearch = true;

            if (!string.IsNullOrEmpty(searchBox.Text))
            {
                string searchText = searchBox.Text.Trim().ToLower();

                bool matchesName = product.ProductName?.ToLower().Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesCategory = product.FkCategory?.CategoryName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesId = product.PkId?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesManufacturer = product.FkManufacturer?.ManufacturerName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                bool matchesSupplier = product.FkSupplier?.SupplierName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;

                matchesSearch = matchesName || matchesCategory || matchesId || matchesManufacturer || matchesSupplier;
            }

            bool matchesDiscount = true;

            if (discountBox != null && discountBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filterType = selectedItem.Tag?.ToString() ?? "All";

                double currentDiscount = Convert.ToDouble(product.Discount ?? 0);
                switch (filterType)
                {
                    case "All":
                        matchesDiscount = true;
                        break;
                    case "Low":
                        matchesDiscount = currentDiscount >= 0 && currentDiscount < 11;
                        break;
                    case "Medium":
                        matchesDiscount = currentDiscount >= 11 && currentDiscount < 15;
                        break;
                    case "High":
                        matchesDiscount = currentDiscount > 15;
                        break;
                }
                
            }

            f.Accepted = matchesSearch && matchesDiscount;
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (_roleId < 3) return;

            var newProduct = new Product();
            var newEdit = new EditWindow(newProduct,_context);
            var result = newEdit.ShowDialog();
            
            if (result == false) return;

            if (!string.IsNullOrEmpty(newEdit.SelectedAbsolutePath))
            {
                string extension = System.IO.Path.GetExtension(newEdit.SelectedAbsolutePath);
                string newFileName = Guid.NewGuid().ToString() + extension;
                string targetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                string destinationPath = System.IO.Path.Combine(targetDirectory, newFileName);

                try
                {
                    File.Copy(newEdit.SelectedAbsolutePath, destinationPath, overwrite: true);
                    newProduct.Image = newFileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копироавнии файла: {ex}", "Ошибка копирования", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            LoadItems();
            _productsSource.View.Refresh();
        }

        private void deleteImage(string ImgPath)
        {
            string targetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            var deleteImgPath = System.IO.Path.Combine(targetDirectory, ImgPath);

            if (!Directory.Exists(targetDirectory))
            {
                var msg_res = MessageBox.Show($"Ошибка при удалении изображения: не существующая директория. Продолжить удаление в любом случае?",
                    "Ошибка удаления", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (msg_res == MessageBoxResult.No) return;
            }

            try
            {
                File.Delete(deleteImgPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex}", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void productButton_Click(object sender, RoutedEventArgs e)
        {
            if (_roleId < 3) return;

            Button? clickedBtn = sender as Button;

            if (clickedBtn == null || _context == null) return;

            Product? clickedProduct = clickedBtn!.DataContext as Product;

            if (clickedProduct == null) return;

            Product productCopy = (Product)_context.Entry(clickedProduct).CurrentValues.ToObject();
            string? originalImgPath = clickedProduct.Image;

            var newEdit = new EditWindow(productCopy, _context);
            var result = newEdit.ShowDialog();

            if (result == false) return;

            if (string.IsNullOrEmpty(productCopy.PkId))
            {
                // delete the original product!!

                if (clickedProduct.Image != null) deleteImage(clickedProduct.Image);

                _context.Remove(clickedProduct);
                _context.SaveChanges();
                _products.Remove(clickedProduct);
                _productsSource.View.Refresh();
                return;
            }

            if (!string.IsNullOrEmpty(newEdit.SelectedAbsolutePath))
            {
                string extension = System.IO.Path.GetExtension(newEdit.SelectedAbsolutePath);
                string newFileName = Guid.NewGuid().ToString() + extension;
                string targetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                if (!Directory.Exists(targetDirectory)) 
                { 
                    Directory.CreateDirectory(targetDirectory);
                }

                string destinationPath = System.IO.Path.Combine(targetDirectory, newFileName);

                try
                {
                    File.Copy(newEdit.SelectedAbsolutePath, destinationPath, overwrite: true);
                    productCopy.Image = newFileName;

                    if (originalImgPath != null) deleteImage(originalImgPath); // delete the original image if ok
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копироавнии файла: {ex}", "Ошибка копирования", MessageBoxButton.OK, MessageBoxImage.Error);
                } 

            }

            _context.Entry(clickedProduct).CurrentValues.SetValues(productCopy);
            _context.SaveChanges();
            _productsSource.View.Refresh();
        }

        private void discountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsSource != null && _productsSource.View != null) _productsSource.View.Refresh();
        }

        private void filterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsSource == null || _productsSource.View == null) return;
            _productsSource.View.SortDescriptions.Clear();

            switch (filterBox.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("ProductName", ListSortDirection.Ascending));
                    break;
                case 2:
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("ProductName", ListSortDirection.Descending));
                    break;
                case 3:
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("FinalPrice", ListSortDirection.Ascending));
                    break;
                case 4:
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("FinalPrice", ListSortDirection.Descending));
                    break;
            }

            _productsSource.View.Refresh();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _productsSource.View.Refresh();
        }
    }
}
