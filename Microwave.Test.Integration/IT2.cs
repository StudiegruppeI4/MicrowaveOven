using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT2
    {
        private ICookController cookController;
        private IUserInterface uut;
        private IDisplay display;
        private IOutput output;
        private ILight light;
        private IButton powerButton, timeButton, startCancelButton;
        private IDoor door;


        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            cookController = Substitute.For<ICookController>();
            display = new Display(output);
            light = new Light(output);
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            uut = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
        }

        [Test]
        public void OnDoorOpenedOutputsLightTurnedOn()
        {
            door.Open();
            output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorClosedOutputsLightTurnedOff()
        {
            door.Open();
            door.Close();
            output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnPowerPressedShowsPower()
        {
            powerButton.Press();
            output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnPowerPressedMultiplePressesShowsPowerOnce()
        {
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            output.Received(1).OutputLine("Display shows: 150 W");
        }

        [Test]
        public void OnTimePressedShowsTime()
        {
            powerButton.Press();
            timeButton.Press();
            output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void OnTimePressedMultiplePressesIncreasesTimeShown()
        {
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();
            timeButton.Press();
            output.Received(1).OutputLine("Display shows: 03:00");
        }

        [Test]
        public void OnStartCancelPressedOutputsTurnedOn()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            output.Received(1).OutputLine("Light is turned on");
            cookController.Received(1).StartCooking(50, 60);
        }

        [Test]
        public void OnDoorOpenedWhileCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();
            cookController.Received(1).Stop();
        }

        [Test]
        public void OnStartCancelPressedCancelsCookingReceivesOutputs()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();
            cookController.Received(1).Stop();
            output.Received(1).OutputLine("Light is turned off");
            output.Received(1).OutputLine("Display cleared");
        }
    }
}