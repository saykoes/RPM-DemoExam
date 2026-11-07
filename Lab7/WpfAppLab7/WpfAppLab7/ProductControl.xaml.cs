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
using WpfAppLab7.Models;

namespace WpfAppLab7
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
            if (DataContext is Product p)
            {
                if (p.Discount > 0)
                {
                    OldPrice.Text = p.Price.ToString();
                    NewPrice.Text = (p.Price - (p.Price / 100 * p.Discount)).ToString();
                }

                if (p.Discount >= 20)
                {
                    controlBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#008080")!;
                } else
                {
                    controlBorder.Background = Brushes.Transparent;
                }
            }
        }
    }
}
