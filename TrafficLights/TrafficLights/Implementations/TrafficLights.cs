using System;
using System.Timers;
using TrafficLights.Enums;
using TrafficLights.Interfaces;
using TrafficLights.Models;
using TrafficLights.ViewModels;

namespace TrafficLights.Implementations
{
    public class TrafficLights : ITrafficLights
    {
        private TrafficLightsModel _model;
        private MainWindowViewModel _viewModel;

        /// <summary>
        /// Таймер для мигания
        /// </summary>
        private System.Timers.Timer _blinkTimer;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TrafficLights()
        {
        }

        public void Setup(TrafficLightsModel model, MainWindowViewModel viewModel)
        {
            _model = model;
            _viewModel = viewModel;

            // Настройка таймера мигания
            _blinkTimer = new System.Timers.Timer(TrafficLightsModel.BlinkSpeed);
            _blinkTimer.AutoReset = true;
            _blinkTimer.Enabled = true;

            _blinkTimer.Elapsed += OnBlinkTimeoutEvent;
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

            ProcessState();
        }

        /// <summary>
        /// Обработчик таймера мигания
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnBlinkTimeoutEvent(Object source, ElapsedEventArgs e)
        {
            if (_model.RedLightState == LightStateEnum.Blinking)
            {
                _model.IsRedLightOn = !_model.IsRedLightOn;
            }

            if (_model.YellowLightState == LightStateEnum.Blinking)
            {
                _model.IsYellowLightOn = !_model.IsYellowLightOn;
            }

            if (_model.GreenLightState == LightStateEnum.Blinking)
            {
                _model.IsGreenLightOn = !_model.IsGreenLightOn;
            }

            ProcessState();
        }

        /// <summary>
        /// Обработчик состояний (в частности - цвет сигнала)
        /// </summary>
        private void ProcessState()
        {
            if (_model.RedLightState == LightStateEnum.On)
            {
                _model.IsRedLightOn = true;
            }
            else if (_model.RedLightState == LightStateEnum.Off)
            {
                _model.IsRedLightOn = false;
            }

            if (_model.YellowLightState == LightStateEnum.On)
            {
                _model.IsYellowLightOn = true;
            }
            else if (_model.YellowLightState == LightStateEnum.Off)
            {
                _model.IsYellowLightOn = false;
            }

            if (_model.GreenLightState == LightStateEnum.On)
            {
                _model.IsGreenLightOn = true;
            }
            else if (_model.GreenLightState == LightStateEnum.Off)
            {
                _model.IsGreenLightOn = false;
            }

            _viewModel.IsRedOn = _model.IsRedLightOn;
            _viewModel.IsYellowOn = _model.IsYellowLightOn;
            _viewModel.IsGreenOn = _model.IsGreenLightOn;
        }
    }
}
