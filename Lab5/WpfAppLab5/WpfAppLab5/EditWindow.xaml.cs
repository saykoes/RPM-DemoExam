using Microsoft.EntityFrameworkCore;
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
using WpfAppLab5.Models;

namespace WpfAppLab5
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        // Свойство, чтобы передать полный путь к выбранному файлу в MainWindow
        public string SelectedAbsoluteFilePath { get; private set; }
        Product _currentProduct;
        public EditWindow(Product product, Lab5ShopSaikouContext context)
        {
            InitializeComponent();
            _currentProduct = product;
            cathegoryComboBox.ItemsSource = context.Categories.ToList();
            manuComboBox.ItemsSource = context.Manufacturers.ToList();
            providerComboBox.ItemsSource = context.Suppliers.ToList();
            this.DataContext = product;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; // Закроет окно и вернет true в MainWindow
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Закроет окно и вернет false в MainWindow
            this.Close();
        }

        private void imageChangeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";
            dialog.CheckPathExists = true;
            if (dialog.ShowDialog() == true)
            {
                BitmapImage selectedImage = new BitmapImage(new Uri(dialog.FileName));
                if (selectedImage.PixelWidth > 300 || selectedImage.PixelHeight > 200)
                {
                    // oh no! image is too large
                }

                // 1. Запоминаем полный путь, откуда будем копировать файл
                SelectedAbsoluteFilePath = dialog.FileName;
                ProductImage.Source = selectedImage;
                //_currentProduct.ImagePath = dialog.SafeFileName;

            }
        }
    }
}
