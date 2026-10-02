using competition.Classes;
using competition.Windows;
using System.Collections.ObjectModel;
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
using static competition.Classes.DatabaseModels;

namespace competition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Countries> CountriesList { get; set; } = new();
        private List<Cities> CitiesList { get; set; } = new();
        private List<Venues> VenuesList { get; set; } = new();
        private List<Athletes> AthletesList { get; set; } = new();
        private List<AthleteBiometrics> AthleteBiometricsList { get; set; } = new();

        // Эти коллекции привязаны к XAML напрямую
        public ObservableCollection<Disciplines> DisciplinesList { get; set; } = new();
        public ObservableCollection<Tournaments> TournamentsList { get; set; } = new();

        // Коллекции для объединенных (связанных) данных
        public ObservableCollection<AthleteDisplayModel> AthletesDisplay { get; set; } = new();
        public ObservableCollection<CityDisplayModel> CitiesDisplay { get; set; } = new();

        public ObservableCollection<RecordModel> RecordsDisplay { get; set; } = new();
        public ObservableCollection<RatingModel> RatingDisplay { get; set; } = new();
        public ObservableCollection<PredictionModel> PredictionsDisplay { get; set; } = new();


        private BdConnection _dbService;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123456789;Database=competition";

            _dbService = new BdConnection(connectionString);

            try
            {
                LoadAndBindData();
                RefreshViewsData();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadAndBindData()
        {
            // 1. Загружаем сырые данные из БД через твой DatabaseService в обычные листы
            // (Временные промежуточные листы для связи данных)
            var disciplinesTemp = new List<Disciplines>();
            var tournamentsTemp = new List<Tournaments>();

            _dbService.LoadCountries(CountriesList);
            _dbService.LoadCities(CitiesList);
            _dbService.LoadVenues(VenuesList);
            _dbService.LoadAthletes(AthletesList);
            _dbService.LoadAthleteBiometrics(AthleteBiometricsList);
            _dbService.LoadDisciplines(disciplinesTemp);
            _dbService.LoadTournaments(tournamentsTemp);

            // 2. Заполняем простые ObservableCollection
            foreach (var d in disciplinesTemp) DisciplinesList.Add(d);
            foreach (var t in tournamentsTemp) TournamentsList.Add(t);

            // 3. СВЯЗЫВАНИЕ ТАБЛИЦ №1: Атлеты + Биометрия (через LINQ Left Join)
            var joinedAthletes = from athlete in AthletesList
                                 join bio in AthleteBiometricsList
                                 on athlete.Athlete_id equals bio.Athlete_id into bioGroup
                                 from subBio in bioGroup.DefaultIfEmpty() // Если биометрии нет, будет null
                                 select new AthleteDisplayModel
                                 {
                                     Athlete_id = athlete.Athlete_id,
                                     Country_code = athlete.Country_code,
                                     // Склеиваем ФИО, убирая лишние пробелы, если Отчества нет
                                     FullName = $"{athlete.Last_name} {athlete.First_name} {athlete.Middle_name}".Trim(),
                                     Gender = athlete.Gender,
                                     Birth_date = athlete.Birth_date,
                                     World_rank_points = athlete.World_rank_points,
                                     // Красиво выводим null-значения
                                     Height = subBio?.Height_cm?.ToString() ?? "—",
                                     Weight = subBio?.Weight_kg?.ToString() ?? "—",
                                     UpdatedAt = subBio?.Updated_at?.ToString("dd.MM.yyyy HH:mm") ?? "—"
                                 };

            foreach (var athleteView in joinedAthletes)
            {
                AthletesDisplay.Add(athleteView);
            }

            // 4. СВЯЗЫВАНИЕ ТАБЛИЦ №2: Города + Страны (Inner Join)
            var joinedCities = from city in CitiesList
                               join country in CountriesList
                               on city.Country_code equals country.Country_code
                               select new CityDisplayModel
                               {
                                   City_id = city.City_id,
                                   City_name = city.City_name,
                                   Country_name = country.Country_name
                               };

            foreach (var cityView in joinedCities)
            {
                CitiesDisplay.Add(cityView);
            }
        }

        public class AthleteDisplayModel
        {
            public int Athlete_id { get; set; }
            public string Country_code { get; set; }
            public string FullName { get; set; }
            public string Gender { get; set; }
            public DateTime Birth_date { get; set; }
            public int World_rank_points { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            public string UpdatedAt { get; set; }
        }

        // Объединяем города и страны
        public class CityDisplayModel
        {
            public int City_id { get; set; }
            public string City_name { get; set; }
            public string Country_name { get; set; }
        }

        public class RecordModel
        {
            public string DisciplineName { get; set; }
            public string GenderCategory { get; set; }
            public string AthleteName { get; set; }
            public string AthleteCountry { get; set; }
            public decimal RecordValue { get; set; }
            public string MeasurementUnit { get; set; }
            public string RecordStatus { get; set; }
            public string TournamentName { get; set; }
            public DateTime RecordDate { get; set; }
        }

        public class RatingModel
        {
            public int GlobalRatingRank { get; set; }
            public string AthleteName { get; set; }
            public string Gender { get; set; }
            public string CountryName { get; set; }
            public int WorldRankPoints { get; set; }
            public int TotalCompetitions { get; set; }
            public int BestPlace { get; set; }
        }

        public class PredictionModel
        {
            public string TournamentName { get; set; }
            public string DisciplineName { get; set; }
            public string GenderCategory { get; set; }
            public string AthleteName { get; set; }
            public decimal EstimatedPerformance { get; set; }
            public int WorldRankPoints { get; set; }
            public string PredictedMedal { get; set; }
        }

        private void RefreshAthletesData()
        {
            AthletesList.Clear();
            AthleteBiometricsList.Clear();
            AthletesDisplay.Clear();

            _dbService.LoadAthletes(AthletesList);
            _dbService.LoadAthleteBiometrics(AthleteBiometricsList);

            var joinedAthletes = from athlete in AthletesList
                                 join bio in AthleteBiometricsList
                                 on athlete.Athlete_id equals bio.Athlete_id into bioGroup
                                 from subBio in bioGroup.DefaultIfEmpty()
                                 select new AthleteDisplayModel
                                 {
                                     Athlete_id = athlete.Athlete_id,
                                     Country_code = athlete.Country_code,
                                     FullName = $"{athlete.Last_name} {athlete.First_name} {athlete.Middle_name}".Trim(),
                                     Gender = athlete.Gender,
                                     Birth_date = athlete.Birth_date,
                                     World_rank_points = athlete.World_rank_points,
                                     Height = subBio?.Height_cm?.ToString() ?? "—",
                                     Weight = subBio?.Weight_kg?.ToString() ?? "—",
                                     UpdatedAt = subBio?.Updated_at?.ToString("dd.MM.yyyy HH:mm") ?? "—"
                                 };

            foreach (var athleteView in joinedAthletes)
            {
                AthletesDisplay.Add(athleteView);
            }
        }

        private void BtnAddAthlete_Click(object sender, RoutedEventArgs e)
        {
            Windows.EditAthlete form = new Windows.EditAthlete();
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    // Передаем в БД сразу два объекта через новый транзакционный метод
                    _dbService.AddAthleteWithBio(form.AthleteResult, form.BioResult);

                    RefreshAthletesData();
                    MessageBox.Show("Атлет и его биометрия успешно добавлены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК: ИЗМЕНЕНИЕ
        private void BtnEditAthlete_Click(object sender, RoutedEventArgs e)
        {
            var selected = AthletesListView.SelectedItem as AthleteDisplayModel;
            if (selected == null)
            {
                MessageBox.Show("Выберите атлета из списка!");
                return;
            }

            Windows.EditAthlete form = new Windows.EditAthlete(selected);
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    // Обновляем обе таблицы одной транзакцией
                    _dbService.UpdateAthleteWithBio(form.AthleteResult, form.BioResult);

                    RefreshAthletesData();
                    MessageBox.Show("Данные успешно обновлены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК: УДАЛЕНИЕ
        private void BtnDeleteAthlete_Click(object sender, RoutedEventArgs e)
        {
            var selected = AthletesListView.SelectedItem as AthleteDisplayModel;
            if (selected == null)
            {
                MessageBox.Show("Выберите атлета для удаления!");
                return;
            }

            var result = MessageBox.Show($"Удалить атлета {selected.FullName} и всю его биометрию?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Каскадно удаляем записи из обеих таблиц
                    _dbService.DeleteAthleteWithBio(selected.Athlete_id);

                    RefreshAthletesData();
                    MessageBox.Show("Атлет и биометрия удалены.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            CityWindow form = new CityWindow();
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _dbService.AddCity(form.CityResult);
                    RefreshCitiesData();
                    MessageBox.Show("Город успешно добавлен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка БД: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК ГОРОДА: ИЗМЕНЕНИЕ
        private void BtnEditCity_Click(object sender, RoutedEventArgs e)
        {
            var selected = CitiesListView.SelectedItem as CityDisplayModel;
            if (selected == null)
            {
                MessageBox.Show("Выберите город из списка!");
                return;
            }

            // Нам нужен country_code для формы, найдем его в сыром списке городов по id
            var rawCity = CitiesList.FirstOrDefault(c => c.City_id == selected.City_id);
            string countryCode = rawCity?.Country_code ?? "";

            CityWindow form = new CityWindow(selected, countryCode);
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _dbService.UpdateCity(form.CityResult);
                    RefreshCitiesData();
                    MessageBox.Show("Данные города обновлены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка БД: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК ГОРОДА: УДАЛЕНИЕ
        private void BtnDeleteCity_Click(object sender, RoutedEventArgs e)
        {
            var selected = CitiesListView.SelectedItem as CityDisplayModel;
            if (selected == null)
            {
                MessageBox.Show("Выберите город для удаления!");
                return;
            }

            var result = MessageBox.Show($"Удалить город {selected.City_name}?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.DeleteCity(selected.City_id);
                    RefreshCitiesData();
                    MessageBox.Show("Город удален.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось удалить город. Возможно, в нем зарегистрированы места проведения (Venues).\nОшибка: {ex.Message}",
                                    "Ошибка связи", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод для быстрой перезагрузки вкладки городов
        private void RefreshCitiesData()
        {
            CitiesList.Clear();
            CountriesList.Clear();
            CitiesDisplay.Clear();

            _dbService.LoadCities(CitiesList);
            _dbService.LoadCountries(CountriesList);

            var joinedCities = from city in CitiesList
                               join country in CountriesList
                               on city.Country_code equals country.Country_code
                               select new CityDisplayModel
                               {
                                   City_id = city.City_id,
                                   City_name = city.City_name,
                                   Country_name = country.Country_name
                               };

            foreach (var cityView in joinedCities)
            {
                CitiesDisplay.Add(cityView);
            }
        }

        private void BtnAddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            DisciplineWindow form = new DisciplineWindow();
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _dbService.AddDiscipline(form.DisciplineResult);
                    RefreshDisciplinesData();
                    MessageBox.Show("Дисциплина успешно добавлена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка БД: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК ДИСЦИПЛИНЫ: ИЗМЕНЕНИЕ
        private void BtnEditDiscipline_Click(object sender, RoutedEventArgs e)
        {
            var selected = DisciplinesListView.SelectedItem as Disciplines;
            if (selected == null)
            {
                MessageBox.Show("Выберите дисциплину из списка!");
                return;
            }

            DisciplineWindow form = new DisciplineWindow(selected);
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _dbService.UpdateDiscipline(form.DisciplineResult);
                    RefreshDisciplinesData();
                    MessageBox.Show("Дисциплина успешно обновлена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка БД: {ex.Message}");
                }
            }
        }

        // ОБРАБОТЧИК ДИСЦИПЛИНЫ: УДАЛЕНИЕ
        private void BtnDeleteDiscipline_Click(object sender, RoutedEventArgs e)
        {
            var selected = DisciplinesListView.SelectedItem as Disciplines;
            if (selected == null)
            {
                MessageBox.Show("Выберите дисциплину для удаления!");
                return;
            }

            var result = MessageBox.Show($"Удалить дисциплину {selected.Discipline_name}?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.DeleteDiscipline(selected.Discipline_id);
                    RefreshDisciplinesData();
                    MessageBox.Show("Дисциплина удалена.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось удалить. Скорее всего, на эту дисциплину уже есть регистрации атлетов (Registrations).\nОшибка: {ex.Message}",
                                    "Ошибка связи", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод для перезагрузки вкладки дисциплин
        private void RefreshDisciplinesData()
        {
            DisciplinesList.Clear();
            var temp = new List<Disciplines>();

            _dbService.LoadDisciplines(temp);

            foreach (var d in temp)
            {
                DisciplinesList.Add(d);
            }
        }

        private void RefreshViewsData()
        {
            var recTemp = new List<RecordModel>();
            var ratTemp = new List<RatingModel>();
            var predTemp = new List<PredictionModel>();

            // Загрузка
            _dbService.LoadRecordsView(recTemp);
            _dbService.LoadRatingView(ratTemp);
            _dbService.LoadPredictionsView(predTemp);

            // Синхронизация с UI
            RecordsDisplay.Clear();
            foreach (var r in recTemp) RecordsDisplay.Add(r);

            RatingDisplay.Clear();
            foreach (var r in ratTemp) RatingDisplay.Add(r);

            PredictionsDisplay.Clear();
            foreach (var r in predTemp) PredictionsDisplay.Add(r);
        }
    }
}