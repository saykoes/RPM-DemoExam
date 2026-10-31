using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using WpfAppLab5.Models;

namespace WpfAppLab5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private int _roleId = 0;
        private ObservableCollection<Product> _products = new();
        private CollectionViewSource _productsSource;
        private readonly Lab5ShopSaikouContext _context;

        public Window1(User? user)
        {
            InitializeComponent();
            _context = new Lab5ShopSaikouContext();
            if (user is not null)
            {
                nameBox.Text = user.UserName;
                _roleId = user.FkUserRoleId;

                this.Title = $"Товары: {user.FkUserRole.RoleName}";

                if (_roleId >= 2)
                {
                    // manager + admin
                    SearchFilterDock.Visibility = Visibility.Visible;
                }
                if (_roleId >= 3)
                {
                    // admin
                    itemsControl.Cursor = Cursors.Hand;
                    addProductButton.Visibility = Visibility.Visible;
                }
            }

            _productsSource = new CollectionViewSource
            {
                Source = _products
            };

            _productsSource.Filter += ItemsFilter;
            itemsControl.ItemsSource = _productsSource.View;

            LoadItems();
        }

        private void ItemsFilter (object sender, FilterEventArgs f)
        {
            if (!(f.Item is Product p))
            {
                f.Accepted = false;
                return;
            }
                // --- ЭТАП 1: ТЕКСТОВЫЙ ПОИСК (Ваш существующий рабочий код) ---
                bool matchesSearch = true;

                if (!string.IsNullOrEmpty(searchBox.Text))
                {
                    string searchText = searchBox.Text.Trim().ToLower();

                    bool matchesName = p.ProductName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool matchesCathegory = p.FkCategory?.CategoryName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool matchesArticle = p.PkId?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool matchesManufacturer = p.FkManufacturer?.ManufacturerName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool matchesSupplier = p.FkSupplier?.SupplierName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false;

                    matchesSearch = matchesName || matchesCathegory || matchesArticle || matchesManufacturer || matchesSupplier;
                }

                // --- ЭТАП 2: ФИЛЬТРАЦИЯ ПО СКИДКАМ ---
                bool matchesDiscount = true;

                // Проверяем, что комбобокс инициализирован и выбран элемент
                if (discountComboBox != null && discountComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    string filterType = selectedItem.Tag?.ToString() ?? "All";

                    // Получаем значение скидки (если null, считаем за 0)
                    // Если у вас в базе скидка хранится как коэффициент (например, 0.15 вместо 15%), 
                    // умножьте значение на 100 или скорректируйте числа ниже
                    double currentDiscount = Convert.ToDouble(p.Discount ?? 0);

                    switch (filterType)
                    {
                        case "All":
                            matchesDiscount = true; // Подходят абсолютно все
                            break;

                        case "Low": // 0 - 11.99%
                            matchesDiscount = currentDiscount >= 0 && currentDiscount < 12;
                            break;

                        case "Medium": // 12 - 18.99%
                            matchesDiscount = currentDiscount >= 12 && currentDiscount < 19;
                            break;

                        case "High": // 19% и более
                            matchesDiscount = currentDiscount >= 19;
                            break;
                    }
                }

                // --- ИТОГ: Товар отображается, только если он прошел ОБА фильтра ---
                f.Accepted = matchesSearch && matchesDiscount;
            }
            

        private void LoadItems()
        {
            if (_context != null)
            {
                _products.Clear();

                var items = _context.Products
                    .Include(p => p.FkManufacturer)
                    .Include(p => p.FkCategory)
                    .Include(p => p.FkSupplier).ToList();

                foreach (var item in items)
                {
                    _products.Add(item);
                }
            }
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (_roleId <= 2)
            {
                return;
            }

            Button? clickedBtn = sender as Button;

            if (clickedBtn != null && _context != null)
            {
                Product? clickedProduct = clickedBtn!.DataContext as Product;

                if (clickedProduct != null)
                {
                    Product productCopy = (Product)_context.Entry(clickedProduct).CurrentValues.ToObject();
                    var editWindow = new EditWindow(productCopy, _context);
                    var result = editWindow.ShowDialog();

                    if (result == true)
                    {
                        // if user changed the image
                        if (!string.IsNullOrEmpty(editWindow.SelectedAbsoluteFilePath))
                        {
                            string extension = System.IO.Path.GetExtension(editWindow.SelectedAbsoluteFilePath); // file extension
                            string newFileName = Guid.NewGuid().ToString() + extension; // file name
                            string targetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                            // if 'Resources' doesn't exist
                            if (!Directory.Exists(targetDirectory))
                            {
                                Directory.CreateDirectory(targetDirectory);
                            }

                            string destinationPath = System.IO.Path.Combine(targetDirectory, newFileName);

                            try
                            {
                                File.Copy(editWindow.SelectedAbsoluteFilePath, destinationPath, overwrite: true);
                                productCopy.ImagePath = newFileName; // write only filename in our db
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
                            }
                        }


                        _context.Entry(clickedProduct).CurrentValues.SetValues(productCopy);
                        _context.SaveChanges();
                        _productsSource.View.Refresh();
                    }
                }
            }
        }

        private void signOutButton_Click(object sender, RoutedEventArgs e)
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

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _productsSource.View.Refresh();
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();

            var editWindow = new EditWindow(newProduct, _context);
            var result = editWindow.ShowDialog();

            if (result == true)
            {
                // if user changed the image
                if (!string.IsNullOrEmpty(editWindow.SelectedAbsoluteFilePath))
                {
                    string extension = System.IO.Path.GetExtension(editWindow.SelectedAbsoluteFilePath); // file extension
                    string newFileName = Guid.NewGuid().ToString() + extension; // file name
                    string targetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                    // if 'Resources' doesn't exist
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    string destinationPath = System.IO.Path.Combine(targetDirectory, newFileName);

                    try
                    {
                        File.Copy(editWindow.SelectedAbsoluteFilePath, destinationPath, overwrite: true);
                        newProduct.ImagePath = newFileName; // write only filename in our db
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
                    }
                }


                _context.Products.Add(newProduct);
                _context.SaveChanges();
                LoadItems();
                _productsSource.View.Refresh();
            }
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsSource == null || _productsSource.View == null) return;

            _productsSource.View.SortDescriptions.Clear();

            switch (sortComboBox.SelectedIndex)
            {
                case 0: // Без сортировки
                    break;

                case 1: // Наименование (А-Я)
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("ProductName", ListSortDirection.Ascending));
                    break;

                case 2: // Наименование (Я-А)
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("ProductName", ListSortDirection.Descending));
                    break;

                case 3: // Цена (по возрастанию)
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("Price", ListSortDirection.Ascending));
                    break;

                case 4: // Цена (по убыванию)
                    _productsSource.View.SortDescriptions.Add(
                        new SortDescription("Price", ListSortDirection.Descending));
                    break;
            }

            _productsSource.View.Refresh();
        }

        private void discountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsSource != null && _productsSource.View != null)
            {
                _productsSource.View.Refresh();
            }
        }
    }
}
