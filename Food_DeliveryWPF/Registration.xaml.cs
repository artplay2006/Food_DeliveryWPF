using Food_DeliveryWPF.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace Food_DeliveryWPF
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        bool ValidationLogin(string login)
        {
            // Проверка длины логина
            if (login.Length > 8 || login.Length < 3)
            {
                return false;
            }

            // Проверка специальных символов
            Regex regex = new Regex("^[a-zA-Z0-9]*$"); // Регулярное выражение, разрешающее только буквы и цифры
            if (!regex.IsMatch(login))
            {
                return false;
            }

            // Логин прошел все проверки
            return true;
        }
        bool ValidationPassword(string password)
        {
            if (password.Length < 3) { return false; }
            else if (password.Length > 8) { return false; }
            return true;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            if (!ValidationLogin(login.Text))
            {
                MessageBox.Show("Невалидынй логин. Логин должен состоять из 3-8 символов английского алфавита.");
            }
            else if (!ValidationPassword(password.Text))
            {
                MessageBox.Show("Невалидынй пароль. Пароль должен состоять из 3-8 символов.");
            }
            else if (password.Text != repassword.Text)
            {
                MessageBox.Show("Пароль несовподает с повторением пароля.");
            }
            else
            {
                try
                {
                    using (var db = new DatabaseContext())
                    {// 0 - обычный пользователь, 1 - админ
                        User user = new User { Login = login.Text, Password = password.Text, Role = 0 };
                        CurrentUser.Login = user.Login;
                        CurrentUser.Admin = (user.Role==1);
                        db.Users.Add(user);
                        db.SaveChanges();
                        new MainMenu().Show();
                        this.Close();
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show($"Пользователь с таким логином уже существует");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
        }
    }
}
