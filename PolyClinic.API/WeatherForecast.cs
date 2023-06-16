namespace PolyClinic.API
{
    /// <summary>
    /// Sample model class
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Getter and setter for Date property
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Getter and setter for Temperature in Celcius property
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// Getter and setter for Temperature in Fahrenheit property
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// Getter and setter for Summary property
        /// </summary>
        public string Summary { get; set; }
    }
}