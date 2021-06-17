using System;
using System.Text;
using System.Device.Gpio;
using System.Threading;

namespace GreenHouseAssistant.Gpio
{
    class OnBoardLed
    {
        private static OnBoardLed _instance = null;
        private static readonly object _mutex = new object();

        private readonly int _pinNumber;
        private GpioPin _led;
        private LedState _ledState;
        
        private OnBoardLed(int pinNumber)
        {
            _pinNumber = pinNumber;

            var gpioController = new GpioController();
            _led = gpioController.OpenPin(_pinNumber, PinMode.Output);

            _ledState = LedState.Off;
            _led.Write(PinValue.Low);
        }
        public static OnBoardLed GetOnBoardLed(int pinNumber = 2)
        {
            if (_instance == null)
            {
                lock (_mutex) 
                {
                    if (_instance == null)
                    {
                        _instance = new OnBoardLed(pinNumber);
                    }
                }
            }

            return _instance;
        }

        public void StartBlinking()
        {
            if (_ledState == LedState.Blinking)
                return;

            _ledState = LedState.Blinking;
            new Thread(() =>
            {
                while (_ledState == LedState.Blinking)
                {
                    _led.Toggle();
                    Thread.Sleep(200);
                }
            }).Start();
        }
        public void TurnOn()
        {
            _ledState = LedState.On;
            _led.Write(PinValue.High);
        }

        public void TurnOff()
        {
            _ledState = LedState.Off;
            _led.Write(PinValue.Low);
        }
    }

    enum LedState
    {
        On,
        Off,
        Blinking
    }
}
