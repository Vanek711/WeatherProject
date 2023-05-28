using System.ComponentModel.DataAnnotations;

namespace WeatherProject.Models.WeatherData
{
    public class WeatherTypes
    {
        [Key]
        public int Id { get; set; }
        public string? Data { get; set; } = null;
        public string? Cloudiness { get; set; } = null;

        public string? Time { get; set; } = null;

        public string? Relativehumidity { get; set; } = null;

        public string? Atmosphericpressure { get; set; } = null;

        public string? Directionofthewind { get; set; } = null;

        public string? Temperature { get; set; } = null;

        public string? H { get; set; } = null;

        public string? Vv { get; set; } = null;

        public string? Dewpoint { get; set; } = null;

        public string? Windspeed { get; set; } = null;

        public string? Weatherconditions { get; set; } = null;
    }
}
