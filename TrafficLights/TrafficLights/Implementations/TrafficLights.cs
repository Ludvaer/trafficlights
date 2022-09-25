using System;
using TrafficLights.Enums;
using TrafficLights.Interfaces;
using TrafficLights.Models;

namespace TrafficLights.Implementations
{
    public class TrafficLights : ITrafficLights
    {
        private TrafficLightsModel _model;

        public void Setup(TrafficLightsModel model)
        {
            _model = model;
        }

        public void ChangeLightState(LightEnum light, LightStateEnum state)
        {
            switch(light)
            {
                case LightEnum.Red:
                    _model.RedLightState = state;
                    break;

                case LightEnum.Yellow:
                    _model.YellowLightState = state;
                    break;

                case LightEnum.Green:
                    _model.GreenLightState = state;
                    break;

                default:
                    throw new ArgumentException(nameof(light));
            }
        }

        
    }
}
