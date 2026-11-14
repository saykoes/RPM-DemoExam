using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics.Metrics;
namespace WpfAppLab7.Models
{


    public partial class Product : IDataErrorInfo
    {
        public string Error => null!;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(PkId):
                        if (string.IsNullOrWhiteSpace(PkId))
                            error = "Артикул обязателен для заполнения.";
                        break;

                    case nameof(ProductName):
                        if (string.IsNullOrWhiteSpace(ProductName))
                            error = "Введите наименование товара.";
                        else if (ProductName.Length < 3)
                            error = "Наименование должно быть длиннее 2 символов.";
                        break;

                    case nameof(Measurement):
                        if (string.IsNullOrWhiteSpace(Measurement))
                            error = "Укажите единицу измерения.";
                        break;

                    case nameof(Price):
                        if (Price == null || Price <= 0)
                            error = "Цена должна быть больше 0.";
                        break;

                    case nameof(Discount):
                        if (Discount < 0 || Discount > 99)
                            error = "Скидка должна быть от 0 до 99%.";
                        break;

                    case nameof(Quantity):
                        if (Quantity < 0)
                            error = "Количество не может быть отрицательным.";
                        break;

                    case nameof(FkCategoryId):
                        if (FkCategoryId == null || FkCategoryId <= 0)
                            error = "Выберите категорию.";
                        break;
                }
                return error;
            }
        }

        // Вспомогательный метод для проверки всего объекта перед сохранением
        public bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(PkId)]) &&
                   string.IsNullOrEmpty(this[nameof(ProductName)]) &&
                   string.IsNullOrEmpty(this[nameof(Measurement)]) &&
                   string.IsNullOrEmpty(this[nameof(Price)]) &&
                   string.IsNullOrEmpty(this[nameof(Discount)]) &&
                   string.IsNullOrEmpty(this[nameof(Quantity)]) &&
                   string.IsNullOrEmpty(this[nameof(FkCategoryId)]);
        }
    }

}
