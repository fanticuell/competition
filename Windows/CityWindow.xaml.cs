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
using static competition.Classes.DatabaseModels;
using static competition.MainWindow;

namespace competition.Windows
{
    /// <summary>
    /// Логика взаимодействия для CityWindow.xaml
    /// </summary>
    public partial class CityWindow : Window
    {
        public Cities CityResult { get; private set; }

        // Конструктор для добавления
        public CityWindow()
        {
            InitializeComponent();
        }

        // Конструктор для редактирования
        public CityWindow(CityDisplayModel targetCity, string countryCode)
        {
            InitializeComponent();

            TxtId.Text = targetCity.City_id.ToString();
            TxtCityName.Text = targetCity.City_name;
            TxtCountryCode.Text = countryCode; // Передаем код, так как в модели отображения у нас название страны
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCityName.Text) || string.IsNullOrWhiteSpace(TxtCountryCode.Text))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = string.IsNullOrEmpty(TxtId.Text) ? 0 : int.Parse(TxtId.Text);

            CityResult = new Cities(
                id,
                TxtCountryCode.Text.Trim().ToUpper(),
                TxtCityName.Text.Trim()
            );

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
