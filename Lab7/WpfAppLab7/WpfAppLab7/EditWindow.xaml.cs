using Microsoft.Win32;
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
using WpfAppLab7.Models;

namespace WpfAppLab7
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {

        public string? SelectedAbsolutePath { get; private set; }
        private bool isNewProduct = true;
        private bool isInOrder = false;
        private Product _currentProduct;

        private int _errorCount = 0;

        public EditWindow(Product product, Lab7ShopSaikouContext context)
        {
            InitializeComponent();
            categoryComboBox.ItemsSource = context.Categories.ToList();
            manuComboBox.ItemsSource = context.Manufacturers.ToList();
            supplierComboBox.ItemsSource = context.Suppliers.ToList();
            this.DataContext = product;
            _currentProduct = product;

            if (string.IsNullOrEmpty(product.PkId) == false)
            {
                deleteButton.Visibility = Visibility.Visible;
                isNewProduct = false;
                deleteButton.IsEnabled = true;
            }
            else
            {
                isNewProduct = true;
                product.PkId = Guid.NewGuid().ToString();
            }

            if (context.OrderProducts.FirstOrDefault(p => p.FkProductId == product.PkId) != null)
            {
                isInOrder = true;
                deleteButton.IsEnabled = false;
            }
        }

        private void imageChangeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";
            openFileDialog.CheckPathExists = true;
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage selectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                if (selectedImage.Width > 300 || selectedImage.Height > 200)
                {
                    //too big
                    MessageBox.Show("Изорбажение слишком большое (макс. 300х200 пикс.)","Ошибка загрузки изображения",MessageBoxButton.OK, MessageBoxImage.Error);
                    //return;
                }

                SelectedAbsolutePath = openFileDialog.FileName;
                productImage.Source = selectedImage;
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct.IsValid())
            {
                this.DialogResult = true;
                this.Close();

            }
            else
            {
                MessageBox.Show("Заполните все обязательные поля корректно.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (isNewProduct == true || isInOrder == true) return;

            var result = MessageBox.Show("Вы уверены что хотите удалить этот товар?","Подтверждение удаления",MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            _currentProduct.PkId = "";

            DialogResult = true;
            this.Close();
        }
    }
}
