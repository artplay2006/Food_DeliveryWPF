using Food_DeliveryWPF.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            InitializeComponent();
            OrdersTable.IsReadOnly = true;
            using (var db = new DatabaseContext()) 
            {
                if (CurrentUser.Admin)
                {
                    //OrdersTable.IsReadOnly = false;
                    OrdersTable.ItemsSource = db.Orders
                        //.Select(o => new
                        //{
                        //    // Выбираем только нужные столбцы
                        //    o.Paid,
                        //    o.ToDelivery,
                        //    o.OrderTime,
                        //    o.OrderComing,
                        //    o.LoginBuyer
                        //    // Добавьте другие необходимые поля
                        //})
                        .ToList();
                    //Save.Visibility = Visibility.Visible;
                }
                else
                {
                    //OrdersTable.IsReadOnly = true;
                    OrdersTable.ItemsSource = db.Orders
                        .Where(o => o.LoginBuyer == CurrentUser.Login)
                        .Select(o => new
                        {
                            // Выбираем только нужные столбцы
                            o.Paid,
                            o.ToDelivery,
                            o.OrderTime,
                            o.OrderComing,
                            // Добавьте другие необходимые поля
                        })
                        .ToList();
                    //Save.Visibility = Visibility.Collapsed;
                }
                // Переименование имен столбцов DataGrid
                //((DataGridTextColumn)OrdersTable.Columns[0]).Header = "К оплате";
                //((DataGridTextColumn)OrdersTable.Columns[1]).Header = "Адрес доставки";
                //((DataGridTextColumn)OrdersTable.Columns[2]).Header = "Время заказа";
                //((DataGridTextColumn)OrdersTable.Columns[3]).Header = "Время доставки";
                //sMessageBox.Show(OrdersTable.Columns.Count.ToString());
            }
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение изменений в базу данных
            using (var context = new DatabaseContext())
            {
                var ordersInGrid = OrdersTable.ItemsSource as List<Food_DeliveryWPF.Models.Order>;
                context.Orders.UpdateRange(ordersInGrid);
                context.SaveChanges();
            }
        }

        private void OrdersTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Здесь можно выполнить дополнительные действия после завершения редактирования ячейки
                // Например, получить новое значение ячейки и выполнить обновление данных
                //var cellContent = (e.EditingElement as TextBox).Text;
                //MessageBox.Show(cellContent);
                // Далее обновление данных или другая логика
            }
        }
        Order selectedOrder;
        private void OrdersTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                    selectedOrder = row.DataContext as Order;
                if (selectedOrder?.Id == null)
                {
                    int index = OrdersTable.Items.IndexOf(row.DataContext);
                    using (var db = new DatabaseContext())
                    {
                        new ContentSelectedOrder(db.Orders.ElementAt(index).Id).Show();
                    }
                }
                else
                {
                    new ContentSelectedOrder(selectedOrder.Id).Show();
                }                    
                Close();
            }
        }
    }
}
