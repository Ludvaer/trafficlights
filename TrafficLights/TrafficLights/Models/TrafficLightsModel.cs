namespace TrafficLights.Models
{
    /// <summary>
    /// Модель объекта реального мира - светофора
    /// </summary>
    public class TrafficLightsModel
    {

        /// <summary>
        /// Длительность проверки в миллисекундах
        /// </summary>
        public const int CheckLength = 3000;

        /// <summary>
        /// Красный огонь - горит или нет?
        /// </summary>
        public bool IsRedLightOn { get; set; }

        /// <summary>
        /// Жёлтый огонь - горит или нет?
        /// </summary>
        public bool IsYellowLightOn { get; set; }

        /// <summary>
        /// Зелёный огонь - горит или нет?
        /// </summary>
        public bool IsGreenLightOn { get; set; } = true;
    }
}
