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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppLab5.Models;

namespace WpfAppLab5
{
    /// <summary>
    /// Логика взаимодействия для ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is Product product)
            {

                if (product.Discount > 20)
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#6A5ACD")!;
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
    }
}
