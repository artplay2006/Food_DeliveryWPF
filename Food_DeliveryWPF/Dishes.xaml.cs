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
    /// Логика взаимодействия для Dishes.xaml
    /// </summary>
    public partial class Dishes : Window
    {
        WrapPanel? wrapPanel;
        public Dishes()
        {
            InitializeComponent();
        }
        public Dishes(string ?image, string ?sel)
        {          
            InitializeComponent();
            wrapPanel = this.FindName("ShowDishes") as WrapPanel;
            ImgSel.Source = new BitmapImage(new Uri(image));
            ImgSel.Stretch = Stretch.Fill;
            //MessageBox.Show(sel);
            FillPanel(sel);
        }
        public void FillPanel(string seller)
        {
            if (wrapPanel != null)
            {
                //MessageBox.Show("wrapPanel есть");
                using (var db = new DatabaseContext())
                {
                    var foods = db.Foods.Where(s => s.NameSeller == seller);
                    //MessageBox.Show(foods.Count().ToString());
                    for (int i = 0; i < foods.Count(); i++)
                    {
                        string? name = foods.ElementAt(i).Name;
                        string? image = foods.ElementAt(i).ImagePath;
                        string? weight = foods.ElementAt(i).Weight.ToString();
                        string? price = foods.ElementAt(i).Price.ToString();
                        string? id = foods.ElementAt(i).Id.ToString();
                        wrapPanel.Children.Add(AddSeller(name, image, weight, price, id));
                    }
                }
            }
            //else
            //{
            //    MessageBox.Show("wrapPanel нет");
            //}
        }
        public Border AddSeller(string? name, string? img, string? weight, string? price, string? id)
        {
            // Создаем Grid для блока товара
            Grid productGrid = new Grid();
            productGrid.Margin = new Thickness(5);

            // Определяем строки Grid
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для изображения
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для текста
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для веса
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Для цены
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            productGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // Создаем изображение
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ImagesOfFoods\\{img}")));
            image.Width = 200;
            image.Height = 150;
            image.Stretch = Stretch.Fill;
            image.Margin = new Thickness(0, 0, 0, 5);
            Grid.SetRow(image, 0); // Устанавливаем изображение в первую строку Grid

            // Создаем текстовый блок для имени
            TextBlock textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.Foreground = Brushes.White;
            textBlock.FontSize = 20;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.TextAlignment = TextAlignment.Center;
            Grid.SetRow(textBlock, 1); // Устанавливаем во вторую строку Grid

            // Создаем текстовый блок для веса
            TextBlock ves = new TextBlock();
            ves.Text = $"{weight}г.";
            ves.Foreground = Brushes.LightGray;
            ves.FontSize = 16;
            ves.FontWeight = FontWeights.DemiBold;
            ves.TextAlignment = TextAlignment.Center;
            Grid.SetRow(ves, 2); // Устанавливаем в третью строку Grid

            // Создаем текстовый блок для цены
            TextBlock cena = new TextBlock();
            cena.Text = $"{price}р.";
            cena.Foreground = Brushes.OrangeRed;
            cena.FontSize = 24;
            cena.FontWeight = FontWeights.ExtraBold;
            cena.TextAlignment = TextAlignment.Center;
            Grid.SetRow(cena, 3); // Устанавливаем в четвертую строку Grid

            // кнопка
            Button button = new Button();
            button.Content = "Добавить";
            button.Background = Brushes.OrangeRed; // Цвет фона кнопки
            button.Foreground = Brushes.White; // Цвет текста на кнопке
            button.FontSize = 20; // Размер шрифта
            button.FontWeight = FontWeights.Bold; // Насыщенность шрифта
            button.Padding = new Thickness(10); // Отступы внутри кнопки
            button.Margin = new Thickness(15);
            button.Click += AddToCart_Click;
            button.Uid = id;
            Grid.SetRow(button, 4);

            // Создаем ComboBox для выбора количества товара
            //ComboBox quantityComboBox = new ComboBox();
            //quantityComboBox.Uid = id;
            //quantityComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 }; // Пример списка количества товара для выбора
            //quantityComboBox.SelectedIndex = 0; // Устанавливаем выбранный элемент по умолчанию
            //quantityComboBox.Margin = new Thickness(0, 0, 0, 0);
            //quantityComboBox.Width = 50; // Устанавливаем ширину ComboBox
            //quantityComboBox.Foreground = Brushes.Black; // Устанавливаем цвет текста в ComboBox
            TextBox colvo = new TextBox();
            colvo.Name = $"CountId{id}";
            colvo.Margin = new Thickness(0,0,0,15);
            colvo.Text = "1";
            colvo.Foreground = Brushes.White;
            colvo.Background = Brushes.Transparent;
            colvo.Width = 25;
            Grid.SetRow(colvo, 5); // Устанавливаем ComboBox в пятую строку Grid
            button.Tag = colvo;

            // Добавляем изображение и текстовый блок в Grid
            productGrid.Children.Add(image);
            productGrid.Children.Add(textBlock);
            productGrid.Children.Add(ves);
            productGrid.Children.Add(cena);
            productGrid.Children.Add(button);
            productGrid.Children.Add(colvo);

            Border border = new Border();
            border.BorderBrush = Brushes.Gray; // Цвет границы
            border.BorderThickness = new Thickness(0.5); // Толщина границы
            border.CornerRadius = new CornerRadius(10); // Устанавливаем радиус закругления углов
            border.Child = productGrid; // Устанавливаем Grid в качестве содержимого Border
            border.Margin = new Thickness(30, 0, 0, 30);// Отступы между продавцами          

            return border;
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int id = int.Parse(button.Uid);
            string c = (button.Tag as TextBox).Text;
            if(int.TryParse(c,out int count))
            {
                if (count > 0 && count <= 100)
                {
                    double ?fp=null;
                    using (var db = new DatabaseContext()) 
                    {
                        fp = db?.Foods?.FirstOrDefault(f=>f.Id==id)?.Price*count;
                    }
                    //MessageBox.Show("Добавлено в корзину");
                    Cart.dishes.Add(new ContentOrder { CountFood = count, PriceDish = fp, IdFood = id});
                }
                else
                {
                    MessageBox.Show("Допустимое количество от 1 до 100");
                }
            }
            else
            {
                MessageBox.Show("Введено не число в количестве");
            }
            //MessageBox.Show(selectedIndex.ToString());
            //using (var db = new DatabaseContext())
            //{
            //    ContentOrder item = new ContentOrder { CountFood = selectedIndex + 1, PriceDish = 1, IdFood = id }; // Устанавливаем количество товара
            //    Cart.dishes.Add(item); // Добавляем товар в корзину
            //}
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainMenu().Show();
            this.Close();
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
