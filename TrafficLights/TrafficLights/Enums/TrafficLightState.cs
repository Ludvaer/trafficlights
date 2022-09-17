namespace TrafficLights.Enums
{
    /// <summary>
    /// Состояние светофора
    /// </summary>
    public enum TrafficLightState
    {
        /// <summary>
        /// Красный
        /// </summary>
        Red,

        /// <summary>
        /// Жёлтый от красного к зелёному
        /// </summary>
        YellowToGreen,

        /// <summary>
        /// Жёлтый от зелёного к красному
        /// </summary>
        YellowToRed,

        /// <summary>
        /// Зелёный
        /// </summary>
        Green
    }
}
