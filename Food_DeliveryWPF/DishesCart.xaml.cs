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
using System.Windows.Shapes;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для DishesCart.xaml
    /// </summary>
    public partial class DishesCart : Window
    {
        WrapPanel? wrapPanel;
        public DishesCart()
        {
            InitializeComponent();
            wrapPanel = this.FindName("ShowCart") as WrapPanel;
            FillPanel();
        }
        public void FillPanel()
        {
            wrapPanel?.Children.Clear();
            double ?fulsum = 0;
            for (int i=0;i<Cart.dishes.Count;i++)
            {
                int? c = Cart.dishes[i].CountFood;
                fulsum += Cart.dishes[i].PriceDish;
                int? idf = Cart.dishes[i].IdFood;
                wrapPanel?.Children.Add(AddSeller(idf, c, i));
            }
            FullPrice.Content = "Сумма: "+fulsum.ToString()+"BYN";
        }
        public Border AddSeller(int ?idfood, int ?count, int index)
        {
            using (var db = new DatabaseContext())
            {
                // Создаем Border
                Border border = new Border();
                border.BorderBrush = Brushes.Gray;
                border.BorderThickness = new Thickness(0, 0, 0, 1);
                border.Margin = new Thickness(100, 0, 0, 0);
                // Создаем Grid
                Grid grid = new Grid();
                grid.Margin = new Thickness(0);

                // Создаем Image
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ImagesOfFoods\\{db.Foods.FirstOrDefault(f => f.Id == idfood).ImagePath}")));
                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Height = 150;
                image.Width = 200;

                // Создаем первый TextBlock
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = db?.Foods?.FirstOrDefault(f=>f.Id==idfood)?.Name;
                textBlock1.Foreground = Brushes.White;
                textBlock1.FontSize = 20;
                textBlock1.FontWeight = FontWeights.DemiBold;
                textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock1.Margin = new Thickness(277, 30, 0, 0);
                textBlock1.TextWrapping = TextWrapping.Wrap;

                // Создаем второй TextBlock
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = $"Количество: {count}";
                textBlock2.Foreground = Brushes.White;
                textBlock2.FontSize = 20;
                textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock2.Margin = new Thickness(277, 92, 0, 0);
                textBlock2.TextWrapping = TextWrapping.Wrap;

                Button butdel = new Button();
                butdel.Content = "Удалить";
                butdel.Foreground = Brushes.Red;
                butdel.Background = Brushes.Transparent;
                butdel.FontSize = 10;
                butdel.Margin = new Thickness(450, 120, 0, 0);
                butdel.Padding = new Thickness(0, 0, 15, 0);
                butdel.BorderThickness = new Thickness(0);
                butdel.Click += DeleteFromCart_Click;
                butdel.Uid = index.ToString();

                // Добавляем элементы в Grid
                grid.Children.Add(image);
                grid.Children.Add(textBlock1);
                grid.Children.Add(textBlock2);
                grid.Children.Add(butdel);

                // Добавляем Grid в Border
                border.Child = grid;

                return border;
            }
        }
        private void DeleteFromCart_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            Cart.dishes.RemoveAt(int.Parse(but.Uid));
            FillPanel();
        }

        //public static void UpdateCart()
        //{
        //    this.Close();

        //}

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            new InputOrder(this).Show();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите выйти из аккаунта?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Ваш код для закрытия текущего окна и открытия окна MainWindow
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
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

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            new DishesCart().Show();
            this.Close();
        }
    }
}
