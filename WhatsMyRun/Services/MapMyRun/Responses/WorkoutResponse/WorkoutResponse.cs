using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyRun.Services.MapMyRun.Responses.WorkoutResponse
{
    //Putting in sep folder, as there were overlapping type definitions between different responses (ie: Links class has diff props between Workout and Workouts)
    public class WorkoutResponse
    {
        public DateTime start_datetime { get; set; }
        public string name { get; set; }
        public DateTime updated_datetime { get; set; }
        public DateTime created_datetime { get; set; }
        public string notes { get; set; }
        public object reference_key { get; set; }
        public string start_locale_timezone { get; set; }
        public string source { get; set; }
        public _Links _links { get; set; }
        public bool has_time_series { get; set; }
        public bool is_verified { get; set; }
        public Aggregates aggregates { get; set; }
    }

    public class _Links
    {
        public Privacy[] privacy { get; set; }
        public Self[] self { get; set; }
        public Documentation[] documentation { get; set; }
        public User[] user { get; set; }
        public Route[] route { get; set; }
        public Activity_Type[] activity_type { get; set; }
    }

    public class Privacy
    {
        public string href { get; set; }
        public string id { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
        public string id { get; set; }
    }

    public class Documentation
    {
        public string href { get; set; }
    }

    public class User
    {
        public string href { get; set; }
        public string id { get; set; }
    }

    public class Route
    {
        public string href { get; set; }
        public string id { get; set; }
    }

    public class Activity_Type
    {
        public string href { get; set; }
        public string id { get; set; }
    }

    public class Aggregates
    {
        public float active_time_total { get; set; }
        public float distance_total { get; set; }
        public float steps_total { get; set; }
        public float speed_avg { get; set; }
        public float elapsed_time_total { get; set; }
        public float metabolic_energy_total { get; set; }
    }

}
