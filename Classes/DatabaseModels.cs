using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace competition.Classes
{
    public class DatabaseModels
    {
        public class Countries
        {
            public string Country_code { get; set; }
            public string Country_name { get; set; }

            public Countries(string country_code, string country_name)
            {
                this.Country_code = country_code;
                this.Country_name = country_name;
            }
        }

        public class Cities
        {
            public int City_id { get; set; }
            public string Country_code { get; set; }
            public string City_name { get; set; }

            public Cities(int city_id, string country_code, string city_name)
            {
                this.City_id = city_id;
                this.Country_code = country_code;
                this.City_name = city_name;
            }
        }

        public class Venues
        {
            public int Venue_id { get; set; }
            public int City_id { get; set; }
            public string Venue_name { get; set; }
            public string Surface_type { get; set; }
            public int? Capacity { get; set; }

            public Venues(int venue_id, int city_id, string venue_name, string surface_type, int? capacity)
            {
                this.Venue_id = venue_id;
                this.City_id = city_id;
                this.Venue_name = venue_name;
                this.Surface_type = surface_type;
                this.Capacity = capacity;
            }
        }

        public class Disciplines
        {
            public int Discipline_id { get; set; }
            public string Discipline_name { get; set; }
            public string Gender_category { get; set; }
            public string Measurement_unit { get; set; }

            public Disciplines(int discipline_id, string discipline_name, string gender_category, string measurement_unit)
            {
                this.Discipline_id = discipline_id;
                this.Discipline_name = discipline_name;
                this.Gender_category = gender_category;
                this.Measurement_unit = measurement_unit;
            }
        }


        public class Athletes
        {
            public int Athlete_id { get; set; }
            public string Country_code { get; set; }
            public string Last_name { get; set; }
            public string First_name { get; set; }
            public string Middle_name { get; set; }
            public string Gender { get; set; }
            public DateTime Birth_date { get; set; }
            public int World_rank_points { get; set; }

            public Athletes(int athlete_id, string country_code, string last_name, string first_name, string middle_name, string gender, DateTime birth_date, int world_rank_points)
            {
                this.Athlete_id = athlete_id;
                this.Country_code = country_code;
                this.Last_name = last_name;
                this.First_name = first_name;
                this.Middle_name = middle_name;
                this.Gender = gender;
                this.Birth_date = birth_date;
                this.World_rank_points = world_rank_points;
            }
        }

        public class AthleteBiometrics
        {
            public int Athlete_id { get; set; }
            public int? Height_cm { get; set; }
            public decimal? Weight_kg { get; set; }
            public DateTime? Updated_at { get; set; }

            public AthleteBiometrics(int athlete_id, int? height_cm, decimal? weight_kg, DateTime? updated_at)
            {
                this.Athlete_id = athlete_id;
                this.Height_cm = height_cm;
                this.Weight_kg = weight_kg;
                this.Updated_at = updated_at;
            }
        }

        public class Referees
        {
            public int Referee_id { get; set; }
            public string Country_code { get; set; }
            public string Last_name { get; set; }
            public string First_name { get; set; }
            public string Middle_name { get; set; }
            public string Category { get; set; }

            public Referees(int referee_id, string country_code, string last_name, string first_name, string middle_name, string category)
            {
                this.Referee_id = referee_id;
                this.Country_code = country_code;
                this.Last_name = last_name;
                this.First_name = first_name;
                this.Middle_name = middle_name;
                this.Category = category;
            }
        }


        public class Tournaments
        {
            public int Tournament_id { get; set; }
            public int Venue_id { get; set; }
            public string Tournament_name { get; set; }
            public DateTime Start_date { get; set; }
            public DateTime End_date { get; set; }
            public string Status { get; set; }

            public Tournaments(int tournament_id, int venue_id, string tournament_name, DateTime start_date, DateTime end_date, string status)
            {
                this.Tournament_id = tournament_id;
                this.Venue_id = venue_id;
                this.Tournament_name = tournament_name;
                this.Start_date = start_date;
                this.End_date = end_date;
                this.Status = status;
            }
        }

        public class Registrations
        {
            public int Registration_id { get; set; }
            public int Tournament_id { get; set; }
            public int Athlete_id { get; set; }
            public int Discipline_id { get; set; }
            public int? Bib_number { get; set; }

            public Registrations(int registration_id, int tournament_id, int athlete_id, int discipline_id, int? bib_number)
            {
                this.Registration_id = registration_id;
                this.Tournament_id = tournament_id;
                this.Athlete_id = athlete_id;
                this.Discipline_id = discipline_id;
                this.Bib_number = bib_number;
            }
        }

        public class Results
        {
            public int Result_id { get; set; }
            public int Registration_id { get; set; }
            public int Referee_id { get; set; }
            public string Stage { get; set; }
            public decimal? Numeric_value { get; set; }
            public int? Place_achieved { get; set; }
            public string Record_status { get; set; }

            public Results(int result_id, int registration_id, int referee_id, string stage, decimal? numeric_value, int? place_achieved, string record_status)
            {
                this.Result_id = result_id;
                this.Registration_id = registration_id;
                this.Referee_id = referee_id;
                this.Stage = stage;
                this.Numeric_value = numeric_value;
                this.Place_achieved = place_achieved;
                this.Record_status = record_status;
            }
        }

        public class Violations
        {
            public int Violation_id { get; set; }
            public int Result_id { get; set; }
            public string Violation_type { get; set; }
            public string Sanction { get; set; }

            public Violations(int violation_id, int result_id, string violation_type, string sanction)
            {
                this.Violation_id = violation_id;
                this.Result_id = result_id;
                this.Violation_type = violation_type;
                this.Sanction = sanction;
            }
        }
    }
}
