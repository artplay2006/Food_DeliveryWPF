using Food_DeliveryWPF.Models;
using Microsoft.EntityFrameworkCore;
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
using Xceed.Wpf.Toolkit.Panels;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для InputOrder.xaml
    /// </summary>
    public partial class InputOrder : Window
    {
        public InputOrder()
        {
            InitializeComponent();
        }
        public InputOrder(DishesCart w)
        {
            InitializeComponent();
            cartwin = w;
        }
        DishesCart cartwin;
        private void CreateOrder2_Click(object sender, RoutedEventArgs e)
        {
            string? date;
            if (DateOrder.SelectedDate != null)
            {
                date = DateOrder.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                MessageBox.Show("не выбрана дата");
                return;
            }
            string? time;
            if (TimeOrder.Value != null)
            {
                time = $"{TimeOrder.Value?.Hour:D2}:{TimeOrder.Value?.Minute:D2}:{TimeOrder.Value?.Second:D2}";
            }
            else 
            {
                MessageBox.Show("не выбрано время");
                return;
            }
            string ?address = AddressOrder.Text;
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("адрес не указан");
                return;
            }

            using (var db = new DatabaseContext())
            {
                double? fulsum = 0;
                for (int i = 0; i < Cart.dishes.Count; i++)
                {
                    fulsum += Cart.dishes[i].PriceDish;
                }

                Order item = new Order { 
                    Paid = fulsum, 
                    ToDelivery=address, 
                    OrderTime= DateTime.Now.ToString(), 
                    OrderComing= date + " " + time,
                    LoginBuyer=CurrentUser.Login,
                };
                db.Orders.Add(item);
                db.SaveChanges();
                MessageBox.Show("Заказ оформлен");

                foreach (var d in Cart.dishes)
                {
                    d.IdOrder = item.Id;
                }
                db.ContentOrders.AddRange(Cart.dishes);
                
                Cart.dishes.Clear();

                db.SaveChanges();

                cartwin.Close();
                new DishesCart().Show();
            }
            this.Close();
        }
    }
}
