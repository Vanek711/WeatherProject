using System.ComponentModel.DataAnnotations;

namespace WeatherProject.Models.WeatherData
{
    public class WeatherTypes
    {
        [Key]
        public int Id { get; set; }
        public string Data { get; set; }
        public string Cloudiness { get; set; } = null!;

        public string Time { get; set; }

        public string Relativehumidity { get; set; }

        public string Atmosphericpressure { get; set; }

        public string Directionofthewind { get; set; } = null!;

        public string? Temperature { get; set; }

        public string? H { get; set; }

        public string? Vv { get; set; }

        public string? Dewpoint { get; set; }

        public string? Windspeed { get; set; }

        public string? Weatherconditions { get; set; }
    }
}
