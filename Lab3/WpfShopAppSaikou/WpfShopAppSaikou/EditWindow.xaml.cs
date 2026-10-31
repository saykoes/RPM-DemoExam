using Microsoft.EntityFrameworkCore;
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

namespace WpfShopAppSaikou
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>

    public partial class EditWindow : Window
    {
        private Product _editingProduct { get; set; }

        public EditWindow()
        {
            InitializeComponent();
            this.Title = "Добавить товар";

            _editingProduct = new Product();
            LoadComboBoxValues();

            productPriceBox.Text = "0";
            productQuantityBox.Text = "0";
            productDiscountBox.Text = "0";
        }

        public EditWindow(Product productToEdit)
        {
            InitializeComponent();
            this.Title = "Изменить товар";

            _editingProduct = productToEdit;
            LoadComboBoxValues();
            FillFields();
        }

        private void LoadComboBoxValues()
        {
            using (var db = new Lab3ShopSaikouContext())
            {
                cathegoryComboBox.ItemsSource = db.Categories.ToList();
                cathegoryComboBox.DisplayMemberPath = "Name";
                cathegoryComboBox.SelectedValuePath = "Id";

                manuComboBox.ItemsSource = db.Manufacturers.ToList();
                manuComboBox.DisplayMemberPath = "Name";
                manuComboBox.SelectedValuePath = "Id";

                providerComboBox.ItemsSource = db.Suppliers.ToList();
                providerComboBox.DisplayMemberPath = "Name";
                providerComboBox.SelectedValuePath = "Id";
            }
        }

        private void FillFields()
        {
            using (var db = new Lab3ShopSaikouContext())
            {
                productNameBox.Text = _editingProduct?.Name?.Name;
                cathegoryComboBox.SelectedIndex = _editingProduct?.CategoryId ?? -1;
                productDescBox.Text = _editingProduct?.Description;
                cathegoryComboBox.SelectedValue = _editingProduct?.CategoryId;
                manuComboBox.SelectedValue = _editingProduct?.ManufacturerId;
                providerComboBox.SelectedValue = _editingProduct?.SupplierId;
                productPriceBox.Text = _editingProduct?.Price.ToString();
                productQuantityBox.Text = _editingProduct?.Count.ToString();
                productDiscountBox.Text = _editingProduct?.Discount.ToString();

            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Text Fields
            if (string.IsNullOrWhiteSpace(productNameBox.Text))
                errors.AppendLine("Укажите наименование товара.");

            // ComboBox
            if (cathegoryComboBox.SelectedValue == null)
                errors.AppendLine("Выберите категорию товара.");
            if (manuComboBox.SelectedValue == null)
                errors.AppendLine("Выберите производителя.");
            if (providerComboBox.SelectedValue == null)
                errors.AppendLine("Выберите поставщика.");

            // Price
            if (!double.TryParse(productPriceBox.Text, out double price))
            {
                errors.AppendLine("Цена должна быть числом.");
            }
            else if (price < 0)
            {
                errors.AppendLine("Цена не может быть отрицательной.");
            }

            // Amount
            if (!int.TryParse(productQuantityBox.Text, out int quantity))
            {
                errors.AppendLine("Количество на складе должно быть целым числом.");
            }
            else if (quantity < 0)
            {
                errors.AppendLine("Количество на складе не может быть отрицательным.");
            }

            // Discount
            if (!int.TryParse(productDiscountBox.Text, out int discount))
            {
                errors.AppendLine("Скидка должна быть целым числом.");
            }
            else if (discount < 0 || discount > 100)
            {
                errors.AppendLine("Скидка должна быть в диапазоне от 0 до 100%.");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _editingProduct.Name?.Name = productNameBox.Text.ToString();
            _editingProduct.Description = productDescBox.Text;
            _editingProduct.CategoryId = (int)cathegoryComboBox.SelectedValue!;
            _editingProduct.ManufacturerId = (int)manuComboBox.SelectedValue!;
            _editingProduct.SupplierId = (int)providerComboBox.SelectedValue!;
            _editingProduct.Price = price;
            _editingProduct.Count = quantity;
            _editingProduct.Discount = discount;

            // Save to DB
            try
            {
                using (var db = new Lab3ShopSaikouContext())
                {
                    // Add Product
                    if (_editingProduct!.Id == 0)
                    {
                        _editingProduct.Category = null!;
                        _editingProduct.Manufacturer = null!;
                        _editingProduct.Supplier = null!;
                        _editingProduct.Name = null!; // Если имя создается как новая запись, управляйте им отдельно

                        db.Products.Add(_editingProduct);
                    }
                    // Edit Product
                    else
                    {
                        var dbProduct = db.Products
                            .Include(p => p.Name)
                            .FirstOrDefault(p => p.Id == _editingProduct.Id);

                        if (dbProduct != null)
                        {
                            // Update the fields for db
                            dbProduct.Description = _editingProduct.Description;
                            dbProduct.CategoryId = _editingProduct.CategoryId;
                            dbProduct.ManufacturerId = _editingProduct.ManufacturerId;
                            dbProduct.SupplierId = _editingProduct.SupplierId;
                            dbProduct.Price = _editingProduct.Price;
                            dbProduct.Count = _editingProduct.Count;
                            dbProduct.Discount = _editingProduct.Discount;

                            // Update the name
                            if (dbProduct.Name != null)
                            {
                                dbProduct.Name.Name = productNameBox.Text;
                            }
                        }
                    }

                    db.SaveChanges(); 
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
