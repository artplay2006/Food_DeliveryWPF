using Food_DeliveryWPF.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для AdminFoods.xaml
    /// </summary>
    public partial class AdminFoods : Window
    {
        public AdminFoods()
        {
            InitializeComponent();
            using (var db = new DatabaseContext())
            {
                FoodsTable.ItemsSource = db.Foods.ToList();
                GetNameSellers.ItemsSource = db.Sellers.Select(s=>s.Name).ToList();
            }
        }
        public bool ValidationFoodName(string cn)
        {
            // Регулярное выражение для проверки наличия специальных символов
            Regex regex = new Regex(@"[!@#$%^&*()_+={}\[\]|\\:;""'<>,.?/]");

            // Проверяем, содержит ли строка специальные символы
            if (regex.IsMatch(cn))
            {
                MessageBox.Show("Имя содержит специальные символы");
                return false; // Если найдены специальные символы, возвращаем false
            }
            if (cn.Length > 20)
            {
                MessageBox.Show($"Длина имени блюда не должна быть больше 20ти. Длина: {cn.Length}");
                return false;
            }

            return true;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFood == null) { MessageBox.Show("Блюдо не выбрано"); return; }
            
            if (string.IsNullOrEmpty(NameFood.Text)) { MessageBox.Show("Имя блюда не написано"); return; }
            if (!ValidationFoodName(NameFood.Text)) { return; }
            if (string.IsNullOrEmpty(GetNameSellers.Text)) { MessageBox.Show("Имя продавца блюда не написано"); return; }
            if (string.IsNullOrEmpty(Weight.Text)) { MessageBox.Show("Вес блюда не написано"); return; }
            if (!double.TryParse(Weight.Text, out double w)) { MessageBox.Show("Вес написан не числом"); return; }
            if (string.IsNullOrEmpty(Price.Text)) { MessageBox.Show("Цен блюда не написано"); return; }
            if (!double.TryParse(Price.Text, out double p)) { MessageBox.Show("Цена написана не числом"); return; }
            if (ImageFood.Source == null) { MessageBox.Show("Картинка блюда не выбрана"); return; }

            // сделай сохранение картинки из ImageFood.Source в папку ImagesOfFoods(C:\C#\Food_DeliveryWPF\Food_DeliveryWPF\bin\Debug\net9.0-windows\ImagesOfFoods)
            // сделай проверку есть ли файл ImageFood.Tag.ToString() в Папке C:\\C#\\Food_DeliveryWPF\\Food_DeliveryWPF\\bin\\Debug\\net9.0-windows\\ImagesOfFoods
            string imagePath = System.IO.Path.Combine("C:\\C#\\Food_DeliveryWPF\\Food_DeliveryWPF\\bin\\Debug\\net9.0-windows\\ImagesOfFoods", ImageFood.Tag.ToString());
            if (!File.Exists(imagePath))
            {
                // Сохранение изображения
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ImageFood.Source));
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }

            string name = NameFood.Text, nameseller= GetNameSellers.Text, weight= Weight.Text, price= Price.Text, imagefood= ImageFood.Tag.ToString();
            using (var context = new DatabaseContext())
            {
                selectedFood.Name = name;
                selectedFood.NameSeller = nameseller;
                selectedFood.Weight = w;
                selectedFood.Price = p;
                selectedFood.ImagePath = imagefood;
                context.Update(selectedFood);
                context.SaveChanges();
            }

            new AdminFoods().Show();
            this.Close();
        }
        Food ?selectedFood;
        private void FoodsTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                selectedFood = row.DataContext as Food;
                NameFood.Text = selectedFood.Name;
                NameSeller.Text = selectedFood.NameSeller;

                GetNameSellers.Text = selectedFood.NameSeller;

                Weight.Text = selectedFood.Weight.ToString();
                Price.Text = selectedFood.Price.ToString();
                ImageFood.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ImagesOfFoods\\{selectedFood.ImagePath}")));
                ImageFood.Tag = selectedFood.ImagePath;
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
                    ImageFood.Source = bitmap;

                    ImageFood.Tag = System.IO.Path.GetFileName(selectedImagePath);
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
            if (string.IsNullOrEmpty(NameFood.Text)) { MessageBox.Show("Имя блюда не написано"); return; }
            if (!ValidationFoodName(NameFood.Text)) { return; }
            if (string.IsNullOrEmpty(GetNameSellers.Text)) { MessageBox.Show("Имя продавца блюда не написано"); return; }
            if (string.IsNullOrEmpty(Weight.Text)) { MessageBox.Show("Вес блюда не написано"); return; }
            if (!double.TryParse(Weight.Text, out double w)) { MessageBox.Show("Вес написан не числом"); return; }

            if (string.IsNullOrEmpty(Price.Text)) { MessageBox.Show("Цен блюда не написано"); return; }
            if (!double.TryParse(Price.Text, out double p)) { MessageBox.Show("Цена написана не числом"); return; }

            if (ImageFood.Source==null) { MessageBox.Show("Картинка блюда не выбрана"); return; }

            // сделай сохранение картинки из ImageFood.Source в папку ImagesOfFoods(C:\C#\Food_DeliveryWPF\Food_DeliveryWPF\bin\Debug\net9.0-windows\ImagesOfFoods)
            // сделай проверку есть ли файл ImageFood.Tag.ToString() в Папке C:\\C#\\Food_DeliveryWPF\\Food_DeliveryWPF\\bin\\Debug\\net9.0-windows\\ImagesOfFoods
            string imagePath = System.IO.Path.Combine("C:\\C#\\Food_DeliveryWPF\\Food_DeliveryWPF\\bin\\Debug\\net9.0-windows\\ImagesOfFoods", ImageFood.Tag.ToString());
            if (!File.Exists(imagePath))
            {
                // Сохранение изображения
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ImageFood.Source));
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }

            string name = NameFood.Text, nameseller = GetNameSellers.Text, weight = Weight.Text, price = Price.Text, imagefood = ImageFood.Tag.ToString();
            using (var db = new DatabaseContext())
            {
                try
                {
                    Food item = new Food { Name = name, NameSeller = nameseller, Weight = w, Price = p, ImagePath=imagefood };
                    db.Add(item);
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Блюдо с таким именем уже есть");
                }
            }

            new AdminFoods().Show();
            this.Close();
        }

        private void DelFood_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFood == null) { MessageBox.Show("Блюдо не выбрано"); return; }

            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить блюдо {selectedFood.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new DatabaseContext())
                {
                    db.Foods.Remove(selectedFood);
                    db.SaveChanges();
                }

                new AdminFoods().Show();
                this.Close();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameFood.Text = string.Empty;
            GetNameSellers.Text = string.Empty;
            Weight.Text = string.Empty;
            Price.Text = string.Empty;
            ImageFood.Source = null;
            selectedFood = null;
        }

        private void AdminUsers_Click(object sender, RoutedEventArgs e)
        {
            new AdminUsers().Show();
            this.Close();
        }

        private void AdminFoods_Click(object sender, RoutedEventArgs e)
        {
            new AdminFoods().Show();
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new MainMenu().Show();
            Close();
        }

        private void AdminSellers_Click(object sender, RoutedEventArgs e)
        {
            new AdminSellers().Show();
            Close();
        }

        private void AdminCategoryes_Click(object sender, RoutedEventArgs e)
        {
            new AdminCategory().Show();
            Close();
        }
    }
}
