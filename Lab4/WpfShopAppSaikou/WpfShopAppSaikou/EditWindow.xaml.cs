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
using WpfShopAppSaikou.Models;

namespace WpfShopAppSaikou
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>

    public partial class EditWindow : Window
    {
        private Product _editingProduct { get; set; }

        public EditWindow(Product productToEdit)
        {
            InitializeComponent();
            _editingProduct = productToEdit;
            this.Title = _editingProduct.Id == 0 ? "Добавить товар" : "Изменить товар";
            
            LoadComboBoxValues();

            if (_editingProduct.Id == 0)
            {
                productPriceBox.Text = "0";
                productQuantityBox.Text = "0";
                productDiscountBox.Text = "0";
            }
            else
            {
                FillFields();
            }
        }

        private void LoadComboBoxValues()
        {
            using (var db = new Lab4ShopSaikouContext())
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
            productNameBox.Text = _editingProduct?.Name?.Name;
            productDescBox.Text = _editingProduct?.Description;
            cathegoryComboBox.SelectedValue = _editingProduct?.CategoryId;
            manuComboBox.SelectedValue = _editingProduct?.ManufacturerId;
            providerComboBox.SelectedValue = _editingProduct?.SupplierId;
            productPriceBox.Text = _editingProduct?.Price.ToString();
            productQuantityBox.Text = _editingProduct?.Count.ToString();
            productDiscountBox.Text = _editingProduct?.Discount.ToString();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(productNameBox.Text))
                errors.AppendLine("Укажите наименование товара.");

            if (cathegoryComboBox.SelectedValue == null)
                errors.AppendLine("Выберите категорию товара.");
            if (manuComboBox.SelectedValue == null)
                errors.AppendLine("Выберите производителя.");
            if (providerComboBox.SelectedValue == null)
                errors.AppendLine("Выберите поставщика.");

            if (!double.TryParse(productPriceBox.Text, out double price) || price < 0)
                errors.AppendLine("Цена должна быть неотрицательным числом.");

            if (!int.TryParse(productQuantityBox.Text, out int quantity) || quantity < 0)
                errors.AppendLine("Количество на складе должно быть неотрицательным целым числом.");

            if (!int.TryParse(productDiscountBox.Text, out int discount) || discount < 0 || discount > 100)
                errors.AppendLine("Скидка должна быть целым числом в диапазоне от 0 до 100%.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Save to DB
            try
            {
                using (var db = new Lab4ShopSaikouContext())
                {
                    string enteredName = productNameBox.Text.Trim();

                    // Search if the name is already in db
                    var existingNameEntity = db.ProductNames
                        .FirstOrDefault(n => n.Name.ToLower() == enteredName.ToLower());

                    // If not, create new Name ID
                    if (existingNameEntity == null)
                    {
                        existingNameEntity = new ProductName { Name = enteredName };

                        // Increment Id
                        int maxId = db.ProductNames.Any() ? db.ProductNames.Max(n => n.Id) : 0;
                        existingNameEntity.Id = maxId + 1;

                        db.ProductNames.Add(existingNameEntity);
                        db.SaveChanges();
                    }

                    // Save and update the product
                    if (_editingProduct.Id == 0)
                    {
                        // Add the product
                        _editingProduct.Description = productDescBox.Text;
                        _editingProduct.CategoryId = (int)cathegoryComboBox.SelectedValue!;
                        _editingProduct.ManufacturerId = (int)manuComboBox.SelectedValue!;
                        _editingProduct.SupplierId = (int)providerComboBox.SelectedValue!;
                        _editingProduct.Price = price;
                        _editingProduct.Count = quantity;
                        _editingProduct.Discount = discount;

                        _editingProduct.NameId = existingNameEntity.Id;

                        // Delete navigation (for saving)
                        _editingProduct.Category = null!;
                        _editingProduct.Manufacturer = null!;
                        _editingProduct.Supplier = null!;
                        _editingProduct.Name = null!;

                        db.Products.Add(_editingProduct);
                    }
                    else
                    {
                        // Edit the product
                        var dbProduct = db.Products.FirstOrDefault(p => p.Id == _editingProduct.Id);

                        if (dbProduct != null)
                        {
                            dbProduct.Description = productDescBox.Text;
                            dbProduct.CategoryId = (int)cathegoryComboBox.SelectedValue!;
                            dbProduct.ManufacturerId = (int)manuComboBox.SelectedValue!;
                            dbProduct.SupplierId = (int)providerComboBox.SelectedValue!;
                            dbProduct.Price = price;
                            dbProduct.Count = quantity;
                            dbProduct.Discount = discount;

                            dbProduct.NameId = existingNameEntity.Id;
                        }
                    }

                    db.SaveChanges(); 
                }

                this.DialogResult = true; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.InnerException?.Message ?? ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
