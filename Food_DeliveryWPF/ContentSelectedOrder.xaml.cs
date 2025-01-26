using Food_DeliveryWPF.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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
    /// Логика взаимодействия для ContentSelectedOrder.xaml
    /// </summary>
    public partial class ContentSelectedOrder : Window
    {
        public ContentSelectedOrder()
        {
            InitializeComponent();
        }
        public ContentSelectedOrder(int idorder)
        {
            InitializeComponent();
            using (var db = new DatabaseContext()) 
            {
                СonOrdTable.ItemsSource = db.ContentOrders
                    .Where(co => co.IdOrder == idorder)
                    .Select(co=>new 
                    { 
                        db.Foods.FirstOrDefault(f=>f.Id == co.IdFood).Name,
                        co.CountFood,
                        co.PriceDish,
                    })                  
                    .ToList();
            }
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new Orders().Show();
            Close();
        }
    }
}
