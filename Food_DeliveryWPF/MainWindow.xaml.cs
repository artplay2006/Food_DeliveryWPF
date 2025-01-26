using Food_DeliveryWPF.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Scaffold-DbContext "Data Source=C:\C#\Food_DeliveryWPF\Food_DeliveryWPF\bin\Debug\net9.0-windows\database.db" Microsoft.EntityFrameworkCore.Sqlite -OutputDir Models -force
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ПРИЯТНО");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text)) { MessageBox.Show("Логин не написан"); }
            else if (string.IsNullOrEmpty(password.Text)) { MessageBox.Show("Пароль не написан"); }
            else
            {
                using (var db = new DatabaseContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Login == login.Text);
                    if (user == null)
                    {
                        MessageBox.Show("Такого логина нет");
                    }
                    else
                    {
                        if (user.Password != password.Text)
                        {
                            MessageBox.Show("Неправильный пароль");
                        }
                        else
                        {
                            CurrentUser.Login = user.Login;
                            CurrentUser.Admin = (user.Role==1);
                            new MainMenu().Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        private void RegWindow_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }
    }
}