using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

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
        public void Test1()
        {
            
        }
    }
}
