using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLights.Models
{
    /// <summary>
    /// Модель объекта реального мира - светофора
    /// </summary>
    public class TrafficLightsModel
    {
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
        public bool IsGreenLightOn { get; set; }
    }
}
