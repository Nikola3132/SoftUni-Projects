namespace TheTankGame.Tests
{
    using NUnit.Framework;
    using System;

    using TheTankGame.Entities.Miscellaneous;
    using TheTankGame.Entities.Miscellaneous.Contracts;
    using TheTankGame.Entities.Parts;
    using TheTankGame.Entities.Parts.Contracts;
    using TheTankGame.Entities.Vehicles;
    using TheTankGame.Entities.Vehicles.Contracts;
    
    [TestFixture]
    public class BaseVehicleTests
    {
        [Test]
        public void CheckingTheModel()
        {
            Assert.Throws<ArgumentException>(()=>new Revenger("", 44, 233, 10, 22, 11, new VehicleAssembler()));
            Assert.That(()=>new Revenger("Model", 44, 233, 10, 22, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void CheckingTheWeight()
        {
            Assert.Throws<ArgumentException>(() => new Revenger("Model", -2, 233, 10, 22, 11, new VehicleAssembler()));
            Assert.That(() => new Revenger("Model", 44, 233, 10, 22, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void CheckingThePrice()
        {
            Assert.Throws<ArgumentException>(() => new Revenger("", 44,0, 10, 22, 11, new VehicleAssembler()));
            Assert.That(() => new Revenger("Model", 44, 233, 10, 22, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void CheckingTheAttack()
        {
            Assert.Throws<ArgumentException>(() => new Revenger("", 44, 233, -2, 22, 11, new VehicleAssembler()));
            Assert.That(() => new Revenger("Model", 44, 233, 0, 22, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void CheckingTheDefense()
        {
            Assert.Throws<ArgumentException>(() => new Revenger("", 44, 233, 6, -7, 11, new VehicleAssembler()));
            Assert.That(() => new Revenger("Model", 44, 233, 0, 0, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void CheckingTheHitPoints()
        {
            Assert.Throws<ArgumentException>(() => new Revenger("", 44, 233, 6, -7, 11, new VehicleAssembler()));
            Assert.That(() => new Revenger("Model", 44, 233, 0, 0, 11, new VehicleAssembler()), Throws.Nothing);
        }
        [Test]
        public void TestTheTotalProps()
        {
            IAssembler assembler = new VehicleAssembler();

            assembler.AddArsenalPart(new ArsenalPart("Arsenal",1, 1, 1));
            assembler.AddEndurancePart(new EndurancePart("Endurence", 2,2, 2));
            assembler.AddShellPart(new ShellPart("Shell", 3, 3, 3));
            IVehicle revenger = new Revenger("Revenger", 4, 4, 4, 4, 4, assembler);
            IVehicle vanguard = new Vanguard("Vanguard", 4, 4, 4, 4, 4, assembler);


            Assert.That(() => revenger.TotalWeight, Is.EqualTo(10));
            Assert.That(() => revenger.TotalPrice, Is.EqualTo(10));
            Assert.That(() => revenger.TotalHitPoints, Is.EqualTo(6));
            Assert.That(() => revenger.TotalDefense, Is.EqualTo(7));
            Assert.That(() => revenger.TotalAttack, Is.EqualTo(5));

            Assert.That(() => vanguard.TotalWeight, Is.EqualTo(10));
            Assert.That(() => vanguard.TotalPrice, Is.EqualTo(10));
            Assert.That(() => vanguard.TotalHitPoints, Is.EqualTo(6));
            Assert.That(() => vanguard.TotalDefense, Is.EqualTo(7));
            Assert.That(() => vanguard.TotalAttack, Is.EqualTo(5));
        }
        [Test]
        public void TestAddingMethods()
        {
            IPart part = new ArsenalPart("Arsenal", 2, 2, 2);
            IAssembler assembler = new VehicleAssembler();
            assembler.AddArsenalPart(part);
            assembler.AddArsenalPart(part);

            IVehicle vehicle = new Revenger("Revenger", 22, 22, 22, 22, 22, assembler);

            vehicle.AddArsenalPart(part);

            Assert.That(() => assembler.ArsenalParts.Count, Is.EqualTo(3));

            vehicle.AddEndurancePart(new EndurancePart("Endurance", 2, 2, 2));
            Assert.That(() => assembler.EnduranceParts.Count, Is.EqualTo(1));

            vehicle.AddShellPart(new ShellPart("Shell", 2, 2, 2));
            Assert.That(() => assembler.ShellParts.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestPartsCollection()
        {
            IPart part = new ArsenalPart("Arsenal", 2, 2, 2);
            IAssembler assembler = new VehicleAssembler();
            assembler.AddArsenalPart(part);
            assembler.AddArsenalPart(part);
            assembler.AddEndurancePart(new EndurancePart("22",22,22,22));
            assembler.AddShellPart(new ShellPart("ss", 22, 22, 22));

            IVehicle vehicle = new Revenger("Revenger", 22, 22, 22, 22, 22, assembler);

            int count = 0;

            foreach (var item in vehicle.Parts)
            {
                count++;
            }

            Assert.That(() => count, Is.EqualTo(4));
        }


        [Test]
        public void ToStringTest()
        {
            IPart part = new ArsenalPart("Arsenal", 2, 2, 2);
            IAssembler assembler = new VehicleAssembler();
            assembler.AddArsenalPart(part);
            assembler.AddArsenalPart(part);
            assembler.AddEndurancePart(new EndurancePart("22", 22, 22, 22));
            assembler.AddShellPart(new ShellPart("ss", 22, 22, 22));

            IVehicle vehicle = new Revenger("Revenger", 22, 22, 22, 22, 22, assembler);

            string actual = vehicle.ToString();
            string excpected = "Revenger - Revenger\r\nTotal Weight: 70.000\r\nTotal Price: 70.000\r\nAttack: 26\r\nDefense: 44\r\nHitPoints: 44\r\nParts: None";

            Assert.That(() => actual, Is.EqualTo(excpected));
        }
    }
}