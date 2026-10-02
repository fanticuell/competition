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
    /// Логика взаимодействия для EditAthlete.xaml
    /// </summary>
    public partial class EditAthlete : Window
    {
        public Athletes AthleteResult { get; private set; }
        public AthleteBiometrics BioResult { get; private set; }
        public EditAthlete()
        {
            InitializeComponent();
            DpBirthDate.SelectedDate = DateTime.Now.AddYears(-20);
        }

        public EditAthlete(AthleteDisplayModel targetAthlete)
        {
            InitializeComponent();

            TxtId.Text = targetAthlete.Athlete_id.ToString();
            TxtId.IsEnabled = false; // Ключ менять нельзя

            var names = targetAthlete.FullName.Split(' ');
            TxtLastName.Text = names.Length > 0 ? names[0] : "";
            TxtFirstName.Text = names.Length > 1 ? names[1] : "";
            TxtMiddleName.Text = names.Length > 2 ? names[2] : "";

            TxtCountryCode.Text = targetAthlete.Country_code;
            TxtGender.Text = targetAthlete.Gender;
            DpBirthDate.SelectedDate = targetAthlete.Birth_date;
            TxtRankPoints.Text = targetAthlete.World_rank_points.ToString();

            // Заполняем поля биометрии (если они были)
            TxtHeight.Text = targetAthlete.Height == "—" ? "" : targetAthlete.Height;
            TxtWeight.Text = targetAthlete.Weight == "—" ? "" : targetAthlete.Weight;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtLastName.Text) ||
        string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
        string.IsNullOrWhiteSpace(TxtCountryCode.Text) ||
        !int.TryParse(TxtRankPoints.Text, out int rank))
            {
                MessageBox.Show("Заполните корректно основные поля атлета!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Определяем ID (если редактирование — берем из поля, если создание — ставим 0)
            int id = string.IsNullOrEmpty(TxtId.Text) ? 0 : int.Parse(TxtId.Text);

            int? height = null;
            if (!string.IsNullOrWhiteSpace(TxtHeight.Text))
            {
                if (int.TryParse(TxtHeight.Text, out int h)) height = h;
                else { MessageBox.Show("Рост должен быть целым числом!"); return; }
            }

            decimal? weight = null;
            if (!string.IsNullOrWhiteSpace(TxtWeight.Text))
            {
                if (decimal.TryParse(TxtWeight.Text.Replace('.', ','), out decimal w)) weight = w;
                else { MessageBox.Show("Вес должен быть числом!"); return; }
            }

            AthleteResult = new Athletes(
                id, // Передается 0 для инкремента при INSERT, или реальный ID при UPDATE
                TxtCountryCode.Text.Trim().ToUpper(),
                TxtLastName.Text.Trim(),
                TxtFirstName.Text.Trim(),
                string.IsNullOrWhiteSpace(TxtMiddleName.Text) ? null : TxtMiddleName.Text.Trim(),
                TxtGender.Text.Trim(),
                DpBirthDate.SelectedDate ?? DateTime.Now,
                rank
            );

            BioResult = new AthleteBiometrics(id, height, weight, DateTime.Now);

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
