using Food_DeliveryWPF.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для AdminSellers.xaml
    /// </summary>
    public partial class AdminSellers : Window
    {
        public AdminSellers()
        {
            InitializeComponent();
            using (var db = new DatabaseContext())
            {
                SellersTable.ItemsSource = db.Sellers.ToList();
                GetCategorySellers.ItemsSource = db.Categories.Select(c=>c.Name).ToList();
            }
        }
        public bool ValidationSellerName(string cn)
        {
            // Регулярное выражение для проверки на наличие чисел или специальных символов
            //Regex regex = new Regex(@"[\d!@#$%^&*()_+={}\[\]|\\:;""'<>,.?/]");

            //// Проверяем, содержит ли строка числа или специальные символы
            //if (regex.IsMatch(cn))
            //{
            //    MessageBox.Show("Имя написано с цифрами и/или специальными символами");
            //    return false; // Если найдены числа или специальные символы, возвращаем false
            //}
            if (cn.Length > 22)
            {
                MessageBox.Show($"Длина имени категории не должна быть больше 22ух. Длина: {cn.Length}");
                return false;
            }

            return true; // Если не найдены числа или специальные символы, возвращаем true
        }
        private void AdminUsers_Click(object sender, RoutedEventArgs e)
        {
            new AdminUsers().Show();
            Close();
        }

        private void AdminFoods_Click(object sender, RoutedEventArgs e)
        {
            new AdminFoods().Show();
            Close();
        }

        private void GoAdminSellers_Click(object sender, RoutedEventArgs e)
        {
            new AdminSellers().Show();
            Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new MainMenu().Show();
            Close();
        }
        Seller selectedSeller;
        private void FoodsTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                selectedSeller = row.DataContext as Seller;
                NameSeller.Text = selectedSeller.Name;
                Address.Text = selectedSeller.Addres;
                GetCategorySellers.Text = selectedSeller.Category;
                ImageSeller.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ImagesOfSellers\\{selectedSeller.ImagePath}")));
                ImageSeller.Tag = selectedSeller.ImagePath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                    Title = "Select an image file"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string selectedImagePath = openFileDialog.FileName;
                    // Открываем выбранное изображение
                    BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));

                    // Устанавливаем выбранное изображение в ImageFood.Source
                    ImageSeller.Source = bitmap;

                    ImageSeller.Tag = System.IO.Path.GetFileName(selectedImagePath);
                    //MessageBox.Show(ImageFood.Tag.ToString());
                }
            }
            catch (System.NotSupportedException)
            {
                MessageBox.Show("Выбрана не картинка или картинка не расширения *.png;*.jpeg;*.jpg");
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // сделать валидацию
            //if (selectedSeller == null) { MessageBox.Show("Продавец не выбран"); return; }
            // сделать валидацию
            if (string.IsNullOrEmpty(NameSeller.Text)) { MessageBox.Show("Имя продавца не написано"); return; }
            if (!ValidationSellerName(NameSeller.Text)) { return; }
            if (string.IsNullOrEmpty(Address.Text)) { MessageBox.Show("Адрес не написан"); return; }
            if (string.IsNullOrEmpty(GetCategorySellers.Text)) { MessageBox.Show("Категория не выбрана"); return; }
            if (ImageSeller.Source == null) { MessageBox.Show("Картинка блюда не выбрана"); return; }
            using (var db = new DatabaseContext())
            {
                try
                {
                    Seller item = new Seller { Name = NameSeller.Text, Addres = Address.Text, Category = GetCategorySellers.Text, ImagePath = ImageSeller.Tag.ToString() };
                    // !!!!!!!!!!!! ОНО РАБОТАЕТ???? !!!!!!!!!!!! .Add() без уточнения таблицы
                    db.Add(item);
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Продавец с таким именем уже есть");
                }
            }

            new AdminSellers().Show();
            this.Close();
        }

        private void DelFood_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeller == null) { MessageBox.Show("Продавец не выбран"); return; }
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить продавца {selectedSeller.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new DatabaseContext())
                {
                    db.Sellers.Remove(selectedSeller);
                    db.SaveChanges();
                }

                new AdminSellers().Show();
                this.Close();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameSeller.Text = "";
            Address.Text = "";
            ImageSeller.Source = null;
            selectedSeller = null!;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeller == null) { MessageBox.Show("Продавец не выбран"); return; }
            // сделать валидацию
            if (string.IsNullOrEmpty(NameSeller.Text)) { MessageBox.Show("Имя продавца не написано"); return; }
            if (!ValidationSellerName(NameSeller.Text)) { return; }
            if (string.IsNullOrEmpty(Address.Text)) { MessageBox.Show("Адрес не написан"); return; }
            if (string.IsNullOrEmpty(GetCategorySellers.Text)) { MessageBox.Show("Категория не выбрана"); return; }
            if (ImageSeller.Source == null) { MessageBox.Show("Картинка блюда не выбрана"); return; }
            //string name = NameFood.Text, nameseller = GetNameSellers.Text, weight = Weight.Text, price = Price.Text, imagefood = ImageFood.Tag.ToString();
            using (var context = new DatabaseContext())
            {
                //var ordersInGrid = FoodsTable.ItemsSource as List<Food_DeliveryWPF.Models.Food>;
                //context.Foods.UpdateRange(ordersInGrid);
                selectedSeller.Name = NameSeller.Text;
                selectedSeller.Addres = Address.Text;
                selectedSeller.Category = GetCategorySellers.Text;
                selectedSeller.ImagePath = ImageSeller.Tag.ToString();
                context.Update(selectedSeller);
                context.SaveChanges();
            }

            new AdminSellers().Show();
            this.Close();
        }

        private void AdminCategoryes_Click(object sender, RoutedEventArgs e)
        {
            new AdminCategory().Show();
            Close();
        }
    }
}
