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
    /// Логика взаимодействия для AdminUsers.xaml
    /// </summary>
    public partial class AdminUsers : Window
    {
        public AdminUsers()
        {
            InitializeComponent();
            LoadTable();
        }
        private async void LoadTable()
        {
            await using (var db = new DatabaseContext())
            {
                await UsersTable.Dispatcher.InvokeAsync(() =>
                {
                    UsersTable.ItemsSource = db.Users.ToList();
                    GetRole.ItemsSource = new string[] { "Пользователь", "Админ" };
                });
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null) { MessageBox.Show("Пользователь не выбран"); return; }
            await using(var db = new DatabaseContext())
            {
                //MessageBox.Show(GetRole.SelectedIndex.ToString());
                selectedUser.Role = GetRole.SelectedIndex;
                db.Users.Update(selectedUser);
                await db.SaveChangesAsync();
            }
            selectedUser = null!;

            //new AdminUsers().Show();
            //Close();
            LoadTable();
        }

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (selectedUser == null) { MessageBox.Show("Пользователь не выбран"); return; }

        //}

        private async void DelFood_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null) { MessageBox.Show("Пользователь не выбран"); return; }
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить пользователя {selectedUser.Login}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await using (var db = new DatabaseContext())
                {
                    db.Users.Remove(selectedUser);
                    await db.SaveChangesAsync();
                }
                selectedUser = null!;

                //new AdminUsers().Show();
                //Close();
            }
            LoadTable();
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
        User selectedUser;
        private void UsersTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                selectedUser = row.DataContext as User;
                Login.Text = selectedUser.Login;
                GetRole.Text = selectedUser.Role==0?"Пользователь":"Админ";
            }
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
