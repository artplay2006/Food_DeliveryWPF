using Food_DeliveryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для AdminCategory.xaml
    /// </summary>
    public partial class AdminCategory : Window
    {
        public AdminCategory()
        {
            InitializeComponent();
            using (var db = new DatabaseContext())
            {
                CategoryTable.ItemsSource = db.Categories.ToList();
            }
        }
        public bool ValidationCategoryName(string cn)
        {
            // Регулярное выражение для проверки на наличие чисел или специальных символов
            Regex regex = new Regex(@"[\d!@#$%^&*()_+={}\[\]|\\:;""'<>,.?/]");

            // Проверяем, содержит ли строка числа или специальные символы
            if (regex.IsMatch(cn))
            {
                MessageBox.Show("Имя написано с цифрами и/или специальными символами");
                return false; // Если найдены числа или специальные символы, возвращаем false
            }
            if (cn.Length > 22)
            {
                MessageBox.Show($"Длина имени категории не должна быть больше 22ух. Длина: {cn.Length}");
                return false;
            }

            return true; // Если не найдены числа или специальные символы, возвращаем true
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new MainMenu().Show();
            Close();
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

        private void GoAdminSellers_Click(object sender, RoutedEventArgs e)
        {
            new AdminSellers().Show();
            Close();
        }

        private void AdminCategoryes_Click(object sender, RoutedEventArgs e)
        {
            new AdminCategory().Show();
            Close();
        }
        Category selectedCategory;
        int indexSelected;
        private void CategoryTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                selectedCategory = row.DataContext as Category;
                NameCategory.Text = selectedCategory.Name;

                // Определение индекса строки, по которой был сделан клик
                indexSelected = CategoryTable.Items.IndexOf(selectedCategory);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameCategory.Text)) { MessageBox.Show("Имя категории не написано"); return; }
            else if (!ValidationCategoryName(NameCategory.Text)) {  return; }
            using (var db = new DatabaseContext())
            {
                try
                {
                    Category item = new Category { Name = NameCategory.Text};
                    db.Categories.Add(item);
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Категория с таким именем уже есть");
                }
            }

            new AdminCategory().Show();
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCategory == null) { MessageBox.Show("Продавец не выбран"); return; }
            // сделать валидацию
            if (string.IsNullOrEmpty(NameCategory.Text)) { MessageBox.Show("Имя продавца не написано"); return; }
            else if (!ValidationCategoryName(NameCategory.Text)) { MessageBox.Show("Имя написано с цифрами и/или специальными символами"); return; }
            //string name = NameFood.Text, nameseller = GetNameSellers.Text, weight = Weight.Text, price = Price.Text, imagefood = ImageFood.Tag.ToString();
            using (var context = new DatabaseContext())
            {
                // Находим сущность по индексу
                var categoryToUpdate = context.Categories.Skip(indexSelected).FirstOrDefault();

                // Проверяем, найдена ли категория
                if (categoryToUpdate != null)
                {
                    // Удаляем текущую категорию
                    context.Categories.Remove(categoryToUpdate);

                    // Создаем новую категорию с обновленными данными
                    var updatedCategory = new Category { Name = NameCategory.Text };

                    // Добавляем новую категорию
                    context.Categories.Add(updatedCategory);

                    // Сохраняем изменения
                    context.SaveChanges();

                    //MessageBox.Show("Категория успешно обновлена");

                    // Переход к новому окну и закрытие текущего
                    new AdminCategory().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Категория с указанным индексом не найдена");
                }
            }

            //new AdminCategory().Show();
            //this.Close();
        }

        private void DelFood_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCategory == null) { MessageBox.Show("Категория не выбрана"); return; }

            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить категорию {selectedCategory.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new DatabaseContext())
                {
                    db.Categories.Remove(selectedCategory);
                    db.SaveChanges();
                }

                new AdminCategory().Show();
                this.Close();
            }
        }
    }
}
