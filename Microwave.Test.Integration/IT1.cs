using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1
    {
        private ICookController uut;
        private IUserInterface userInterface;
        private ITimer timer;
        private IPowerTube powerTube;
        private IDisplay display;
        private IOutput output;


        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            timer = new Timer();
            powerTube = new PowerTube(output);
            display = new Display(output);
            userInterface = Substitute.For<IUserInterface>();
            uut = new CookController(timer, display, powerTube, userInterface);
        }

        [Test]
        public void StartCookingOutputsPower()
        {
            uut.StartCooking(50, 10000);
            output.Received(1).OutputLine("PowerTube works with 50");
        }

        [Test]
        public void StopOutputsTurnedOff()
        {
            uut.StartCooking(50, 10000);
            uut.Stop();
            output.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void StopBeforeStartBookingOutputsNothing()
        {
            uut.Stop();
            output.DidNotReceive().OutputLine("PowerTube turned off");
        }

        [Test]
        public void OnTimerExpiredOutputsTurnedOff()
        {
            uut.StartCooking(50, 1);
            Thread.Sleep(1100);
            output.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void OnTimerExpiredTimerNotExpiredOutputsNothing()
        {
            uut.StartCooking(50, 1200);
            Thread.Sleep(500);
            output.DidNotReceive().OutputLine("PowerTube turned off");
        }
    }
}
