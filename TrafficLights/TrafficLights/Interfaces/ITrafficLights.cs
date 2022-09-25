using TrafficLights.Enums;
using TrafficLights.Models;

namespace TrafficLights.Interfaces
{
    /// <summary>
    /// Интерфейс для управления светофором
    /// </summary>
    public interface ITrafficLights
    {
        /// <summary>
        /// Связать реализацию интерфейса с реальным светофором
        /// </summary>
        void Setup(TrafficLightsModel model);

        /// <summary>
        /// Изменить состояние огня
        /// </summary>
        /// <param name="light">С каким огнём работаем</param>
        /// <param name="state">В какое состояние его переводим</param>
        void ChangeLightState(LightEnum light, LightStateEnum state);
    }
}
