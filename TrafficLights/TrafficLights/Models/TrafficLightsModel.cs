using TrafficLights.Enums;

namespace TrafficLights.Models
{
    /// <summary>
    /// Модель объекта реального мира - светофора
    /// </summary>
    public class TrafficLightsModel
    {

        /// <summary>
        /// Скорость мигания
        /// </summary>
        public const int BlinkSpeed = 500;

        /// <summary>
        /// Длительность проверки в миллисекундах
        /// </summary>
        public const int CheckLength = 3000;

        /// <summary>
        /// Длительность красного сигнала
        /// </summary>
        public const int RedLightLength = 10000;

        /// <summary>
        /// Длительность жёлтого при переходе к красному
        /// </summary>
        public const int YellowToRedLightLength = 3000;

        /// <summary>
        /// Длительность жёлтого при переходе к зелёному
        /// </summary>
        public const int YellowToGreenLightLength = 3000;

        /// <summary>
        /// Длительность зелёного сигнала
        /// </summary>
        public const int GreenLightLength = 10000;

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

        /// <summary>
        /// Состояние красного огня
        /// </summary>
        public LightStateEnum RedLightState { get; set; } = LightStateEnum.Off; //по умолчанию выключен

        /// <summary>
        /// Состояние жёлтого огня
        /// </summary>
        public LightStateEnum YellowLightState { get; set; } = LightStateEnum.Off;

        /// <summary>
        /// Состояние зелёного огня
        /// </summary>
        public LightStateEnum GreenLightState { get; set; } = LightStateEnum.On; //по умолчанию включён

        /// <summary>
        /// Текущее состояние светофора
        /// </summary>
        public TrafficLightState CurrentState { get; set; } = TrafficLightState.Green;
    }
}
