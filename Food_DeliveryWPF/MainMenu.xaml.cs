using Food_DeliveryWPF.Models;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        WrapPanel? wrapPanel;
        public MainMenu()
        {
            InitializeComponent();

            if (CurrentUser.Admin)
            {
                AdminBorder.Visibility = Visibility.Visible;
                AdminUsers.Visibility = Visibility.Visible;
                AdminSellers.Visibility = Visibility.Visible;
                AdminFoods.Visibility = Visibility.Visible;
                AdminCategoryes.Visibility = Visibility.Visible;
            }
            else
            {
                AdminBorder.Visibility = Visibility.Collapsed;
                AdminUsers.Visibility = Visibility.Collapsed;
                AdminSellers.Visibility = Visibility.Collapsed;
                AdminFoods.Visibility = Visibility.Collapsed;
                AdminCategoryes.Visibility = Visibility.Collapsed;
            }

            // Находим WrapPanel по его имени из XAML
            wrapPanel = this.FindName("ShowSellers") as WrapPanel;
            FillPanel("Все");
        }
        
        public void FillPanel(string category)
        {
            if (wrapPanel != null)
            {
                wrapPanel.Children.Clear();
                using (var db = new DatabaseContext())
                {
                    var sellers = category == "Все" ? db.Sellers: db.Sellers.Where(s=>s.Category==category);
                    //MessageBox.Show(sellers.Count().ToString());
                    for (int i = 0; i < sellers.Count(); i++)
                    {
                        string? name = sellers.ElementAt(i).Name;
                        string? image = sellers.ElementAt(i).ImagePath;
                        wrapPanel.Children.Add(AddSeller(name, image));
                    }
                }
            }
        }
        public Border AddSeller(string ?name, string ?img)
        {
            // Создаем Grid для блока товара
            Grid productGrid = new Grid();
            productGrid.Margin = new Thickness(5);

            // Определяем строки Grid
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для изображения
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для текста

            // Создаем изображение
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ImagesOfSellers\\{img}")));
            image.Width = 200;
            image.Height = 150;
            image.Stretch = Stretch.Fill;
            image.Margin = new Thickness(0, 0, 0, 5);
            Grid.SetRow(image, 0); // Устанавливаем изображение в первую строку Grid

            // Создаем текстовый блок
            TextBlock textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.Foreground = Brushes.White;
            textBlock.FontSize = 16;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.TextAlignment = TextAlignment.Center;
            Grid.SetRow(textBlock, 1); // Устанавливаем текстовый блок во вторую строку Grid

            // Добавляем изображение и текстовый блок в Grid
            productGrid.Children.Add(image);
            productGrid.Children.Add(textBlock);

            // Создаем Border с красной границей и добавляем в него Grid
            Border border = new Border();
            border.BorderBrush = Brushes.Gray; // Цвет границы
            border.BorderThickness = new Thickness(0.5); // Толщина границы
            border.CornerRadius = new CornerRadius(10); // Устанавливаем радиус закругления углов
            border.Child = productGrid; // Устанавливаем Grid в качестве содержимого Border
            border.Margin = new Thickness(30,0,0,30);// Отступы между продавцами

            // Добавляем обработчик события нажатия
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

            return border;
        }       
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем объект Border, который вызвал событие
            Border ?border = sender as Border;
            string ?imgseller=null, name=null;
            // Проверяем, что Border не равен null
            if (border != null)
            {
                Image image = null;
                TextBlock textBlock = null;
                // Перебираем дочерние элементы Border и ищем объект Image
                foreach (var child in ((Grid)border.Child).Children)
                {
                    if (child is Image)
                    {
                        image = (Image)child;
                        //break;
                    }
                    if (child is TextBlock)
                    {
                        textBlock = (TextBlock)child;
                        //break;
                    }
                }

                // Проверяем, что объект Image найден
                if (image != null)
                {
                    // Далее можно работать с объектом Image
                    // Например, получить путь к изображению
                    imgseller = (image.Source as BitmapImage)?.UriSource.LocalPath;
                }
                if (textBlock != null)
                {
                    name = textBlock.Text;
                }
            }
            new Dishes(imgseller, name).Show();
            this.Close();
        }

        private void FastFood_Click(object sender, RoutedEventArgs e)
        {
            string ?category = CategoryFastFood.Content.ToString();
            FillPanel(category);
        }

        private void CategoryPizza_Click(object sender, RoutedEventArgs e)
        {
            string ?category = CategoryPizza.Content.ToString();
            FillPanel(category);
        }

        private void CategorySushi_Click(object sender, RoutedEventArgs e)
        {
            string ?category = CategorySushi.Content.ToString();
            FillPanel(category);
        }

        private void CategoryShaurma_Click(object sender, RoutedEventArgs e)
        {
            string ?category = CategoryShaurma.Content.ToString();
            FillPanel(category);
        }

        private void AllCategoryes_Click(object sender, RoutedEventArgs e)
        {
            string? category = AllCategoryes.Content.ToString();
            FillPanel(category);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите выйти из аккаунта?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Ваш код для закрытия текущего окна и открытия окна MainWindow
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                //CurrentUser.Login = string.Empty;
                //CurrentUser.Admin = null;
                Close();
            }
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            new DishesCart().Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainMenu().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
            this.Close();
        }

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    new AdminUsers().Show();
        //}

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
