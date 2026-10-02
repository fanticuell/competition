using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static competition.Classes.DatabaseModels;
using static competition.MainWindow;

namespace competition.Classes
{
    public class BdConnection
    {
        private readonly string _connectionString;

        public BdConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void LoadCountries(List<Countries> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"country_code\", \"country_name\" FROM \"countries\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Countries(reader.GetString(0), reader.GetString(1)));
            }
        }

        public void LoadCities(List<Cities> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"city_id\", \"country_code\", \"city_name\" FROM \"cities\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Cities(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }
        }

        public void LoadVenues(List<Venues> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"venue_id\", \"city_id\", \"venue_name\", \"surface_type\", \"capacity\" FROM \"venues\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Venues(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetInt32(4)
                ));
            }
        }

        public void LoadDisciplines(List<Disciplines> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"discipline_id\", \"discipline_name\", \"gender_category\", \"measurement_unit\" FROM \"disciplines\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Disciplines(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            }
        }

        public void LoadAthletes(List<Athletes> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"athlete_id\", \"country_code\", \"last_name\"" +
                ", \"first_name\", \"middle_name\", \"gender\", \"birth_date\", \"world_rank_points\" FROM \"athletes\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Athletes(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetString(5),
                    reader.GetDateTime(6),
                    reader.GetInt32(7)
                ));
            }
        }

        public void LoadAthleteBiometrics(List<AthleteBiometrics> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"athlete_id\", \"height_cm\", \"weight_kg\", \"updated_at\" FROM \"athlete_biometrics\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new AthleteBiometrics(
                    reader.GetInt32(0),
                    reader.IsDBNull(1) ? null : reader.GetInt32(1),
                    reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                    reader.IsDBNull(3) ? null : reader.GetDateTime(3)
                ));
            }
        }

        public void LoadReferees(List<Referees> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"referee_id\", \"country_code\", \"last_name\", \"first_name\", \"middle_name\", \"category\" FROM \"referees\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Referees(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetString(5)
                ));
            }
        }

        public void LoadTournaments(List<Tournaments> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"tournament_id\", \"venue_id\"," +
                " \"tournament_name\", \"start_date\", \"end_date\", \"status\" FROM \"tournaments\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Tournaments(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetDateTime(4),
                    reader.GetString(5)
                ));
            }
        }

        public void LoadRegistrations(List<Registrations> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"registration_id\", \"tournament_id\", \"athlete_id\", \"discipline_id\", \"bib_number\" FROM \"registrations\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Registrations(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.IsDBNull(4) ? null : reader.GetInt32(4)
                ));
            }
        }

        public void LoadResults(List<Results> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"result_id\", \"registration_id\", \"referee_id\", \"stage\", \"numeric_value\", \"place_achieved\", \"record_status\" FROM \"results\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Results(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6)
                ));
            }
        }

        public void LoadViolations(List<Violations> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT \"violation_id\", \"result_id\", \"violation_type\", \"sanction\" FROM \"violations\"", connection);
            using var reader = cmd.ExecuteReader();

            list.Clear();
            while (reader.Read())
            {
                list.Add(new Violations(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3)
                ));
            }
        }

        public void AddAthleteWithBio(Athletes athlete, AthleteBiometrics bio)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Вставляем атлета БЕЗ указания athlete_id. Используем RETURNING для получения нового ID.
                string athleteSql = @"INSERT INTO athletes 
                (country_code, last_name, first_name, middle_name, gender, birth_date, world_rank_points) 
                VALUES (@country, @last, @first, @middle, @gender, @birth, @rank) 
                RETURNING athlete_id";

                int newAthleteId;
                using (var cmd = new NpgsqlCommand(athleteSql, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("country", athlete.Country_code);
                    cmd.Parameters.AddWithValue("last", athlete.Last_name);
                    cmd.Parameters.AddWithValue("first", athlete.First_name);
                    cmd.Parameters.AddWithValue("middle", (object)athlete.Middle_name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("gender", athlete.Gender);
                    cmd.Parameters.AddWithValue("birth", athlete.Birth_date);
                    cmd.Parameters.AddWithValue("rank", athlete.World_rank_points);

                    // Получаем сгенерированный базой ID
                    newAthleteId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Вставляем биометрию, используя только что полученный newAthleteId
                string bioSql = @"INSERT INTO athlete_biometrics 
                (athlete_id, height_cm, weight_kg, updated_at) 
                VALUES (@id, @height, @weight, @updated)";

                using (var cmd = new NpgsqlCommand(bioSql, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("id", newAthleteId);
                    cmd.Parameters.AddWithValue("height", (object)bio.Height_cm ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("weight", (object)bio.Weight_kg ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("updated", (object)bio.Updated_at ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdateAthleteWithBio(Athletes athlete, AthleteBiometrics bio)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Обновляем атлета (тут ID уже есть, по нему ищем)
                string athleteSql = @"UPDATE athletes SET 
                country_code = @country, last_name = @last, first_name = @first, 
                middle_name = @middle, gender = @gender, birth_date = @birth, world_rank_points = @rank 
                WHERE athlete_id = @id";

                using (var cmd = new NpgsqlCommand(athleteSql, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("id", athlete.Athlete_id);
                    cmd.Parameters.AddWithValue("country", athlete.Country_code);
                    cmd.Parameters.AddWithValue("last", athlete.Last_name);
                    cmd.Parameters.AddWithValue("first", athlete.First_name);
                    cmd.Parameters.AddWithValue("middle", (object)athlete.Middle_name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("gender", athlete.Gender);
                    cmd.Parameters.AddWithValue("birth", athlete.Birth_date);
                    cmd.Parameters.AddWithValue("rank", athlete.World_rank_points);
                    cmd.ExecuteNonQuery();
                }

                // 2. Обновляем биометрию (UPSERT на случай, если раньше записи не было)
                string bioSql = @"INSERT INTO athlete_biometrics (athlete_id, height_cm, weight_kg, updated_at)
                VALUES (@id, @height, @weight, @updated)
                ON CONFLICT (athlete_id) DO UPDATE SET 
                height_cm = EXCLUDED.height_cm, 
                weight_kg = EXCLUDED.weight_kg, 
                updated_at = EXCLUDED.updated_at";

                using (var cmd = new NpgsqlCommand(bioSql, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("id", bio.Athlete_id);
                    cmd.Parameters.AddWithValue("height", (object)bio.Height_cm ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("weight", (object)bio.Weight_kg ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("updated", (object)bio.Updated_at ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void DeleteAthleteWithBio(int athleteId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                using (var cmd = new NpgsqlCommand("DELETE FROM athlete_biometrics WHERE athlete_id = @id", connection, transaction))
                {
                    cmd.Parameters.AddWithValue("id", athleteId);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand("DELETE FROM athletes WHERE athlete_id = @id", connection, transaction))
                {
                    cmd.Parameters.AddWithValue("id", athleteId);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void AddCity(Cities city)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO cities (country_code, city_name) VALUES (@country, @name)";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("country", city.Country_code);
            cmd.Parameters.AddWithValue("name", city.City_name);

            cmd.ExecuteNonQuery();
        }

        // Изменение города
        public void UpdateCity(Cities city)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE cities SET country_code = @country, city_name = @name WHERE city_id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", city.City_id);
            cmd.Parameters.AddWithValue("country", city.Country_code);
            cmd.Parameters.AddWithValue("name", city.City_name);

            cmd.ExecuteNonQuery();
        }

        // Удаление города
        public void DeleteCity(int cityId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "DELETE FROM cities WHERE city_id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", cityId);

            cmd.ExecuteNonQuery();
        }

        public void AddDiscipline(Disciplines discipline)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO disciplines (discipline_name, gender_category, measurement_unit) VALUES (@name, @gender, @unit)";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("name", discipline.Discipline_name);
            cmd.Parameters.AddWithValue("gender", discipline.Gender_category);
            cmd.Parameters.AddWithValue("unit", discipline.Measurement_unit);

            cmd.ExecuteNonQuery();
        }

        // Изменение дисциплины
        public void UpdateDiscipline(Disciplines discipline)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE disciplines SET discipline_name = @name, gender_category = @gender, measurement_unit = @unit WHERE discipline_id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", discipline.Discipline_id);
            cmd.Parameters.AddWithValue("name", discipline.Discipline_name);
            cmd.Parameters.AddWithValue("gender", discipline.Gender_category);
            cmd.Parameters.AddWithValue("unit", discipline.Measurement_unit);

            cmd.ExecuteNonQuery();
        }

        // Удаление дисциплины
        public void DeleteDiscipline(int disciplineId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = "DELETE FROM disciplines WHERE discipline_id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", disciplineId);

            cmd.ExecuteNonQuery();
        }

        public void LoadRecordsView(List<RecordModel> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT discipline_name, gender_category, athlete_name, athlete_country, record_value, measurement_unit, record_status, tournament_name, record_date FROM view_records", connection);
            using var reader = cmd.ExecuteReader();
            list.Clear();
            while (reader.Read())
            {
                list.Add(new RecordModel
                {
                    DisciplineName = reader.GetString(0),
                    GenderCategory = reader.GetString(1),
                    AthleteName = reader.GetString(2),
                    AthleteCountry = reader.GetString(3),
                    RecordValue = reader.GetDecimal(4),
                    MeasurementUnit = reader.GetString(5),
                    RecordStatus = reader.GetString(6),
                    TournamentName = reader.GetString(7),
                    RecordDate = reader.GetDateTime(8)
                });
            }
        }

        public void LoadRatingView(List<RatingModel> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT global_rating_rank, athlete_name, gender, country_name, world_rank_points, total_competitions, best_place FROM view_athlete_rating", connection);
            using var reader = cmd.ExecuteReader();
            list.Clear();
            while (reader.Read())
            {
                list.Add(new RatingModel
                {
                    GlobalRatingRank = Convert.ToInt32(reader.GetInt64(0)),
                    AthleteName = reader.GetString(1),
                    Gender = reader.GetString(2),
                    CountryName = reader.GetString(3),
                    WorldRankPoints = reader.GetInt32(4),
                    TotalCompetitions = Convert.ToInt32(reader.GetInt64(5)),
                    BestPlace = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                });
            }
        }

        public void LoadPredictionsView(List<PredictionModel> list)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT tournament_name, discipline_name, gender_category, athlete_name, estimated_performance, world_rank_points, predicted_medal FROM view_predictions", connection);
            using var reader = cmd.ExecuteReader();
            list.Clear();
            while (reader.Read())
            {
                list.Add(new PredictionModel
                {
                    TournamentName = reader.GetString(0),
                    DisciplineName = reader.GetString(1),
                    GenderCategory = reader.GetString(2),
                    AthleteName = reader.GetString(3),
                    EstimatedPerformance = reader.GetDecimal(4),
                    WorldRankPoints = reader.GetInt32(5),
                    PredictedMedal = reader.GetString(6)
                });
            }
        }


    }
}
    

