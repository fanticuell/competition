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

namespace competition.Windows
{
    /// <summary>
    /// Логика взаимодействия для DisciplineWindow.xaml
    /// </summary>
    public partial class DisciplineWindow : Window
    {
        public Disciplines DisciplineResult { get; private set; }

        // Для добавления
        public DisciplineWindow()
        {
            InitializeComponent();
        }

        // Для редактирования
        public DisciplineWindow(Disciplines targetDiscipline)
        {
            InitializeComponent();

            TxtId.Text = targetDiscipline.Discipline_id.ToString();
            TxtName.Text = targetDiscipline.Discipline_name;
            TxtGender.Text = targetDiscipline.Gender_category;
            TxtUnit.Text = targetDiscipline.Measurement_unit;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) ||
                string.IsNullOrWhiteSpace(TxtGender.Text) ||
                string.IsNullOrWhiteSpace(TxtUnit.Text))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = string.IsNullOrEmpty(TxtId.Text) ? 0 : int.Parse(TxtId.Text);

            DisciplineResult = new Disciplines(
                id,
                TxtName.Text.Trim(),
                TxtGender.Text.Trim(),
                TxtUnit.Text.Trim()
            );

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
