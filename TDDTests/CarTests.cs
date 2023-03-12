using Tdd;

namespace TDDTests
{
    public class CarTests
    {
        private const string _color = "#FFFFF";
        private const CarMake _make = CarMake.Audi;
        private const float _fuelConsumption = 5.5f;
        private const int _tankCapacity = 50;
        private Car _defaultCar;
        private Car _longRangeCar;

        [SetUp]
        public void Setup()
        {
            _defaultCar = new Car(_color, _make, _fuelConsumption, _tankCapacity);

            _longRangeCar = new Car(_color, _make, _fuelConsumption, int.MaxValue);
            _longRangeCar.Refuel(int.MaxValue);
        }

        [Test]
        public void Ctor__ShouldCreateCar()
        {
            //arrange

            //act
            var car = new Car(_color, _make, _fuelConsumption, _tankCapacity);

            //assert
            Assert.IsNotNull(car);
        }

        [Test]
        public void Ctor__CanReadPropertiesFromCreatedObject()
        {
            //arrange

            //act
            var car = new Car(_color, _make, _fuelConsumption, _tankCapacity);

            //assert
            Assert.That(car.HexColor, Is.EqualTo(_color));
            Assert.That(car.Make, Is.EqualTo(_make));
            Assert.That(car.FuelConsumption, Is.EqualTo(_fuelConsumption));
            Assert.That(car.TankCapacity, Is.EqualTo(_tankCapacity));
        }

        [Test]
        public void Car__SetsDefaultValuesForFuelAndOdo()
        {
            //arrange

            //act
            var car = new Car(_color, _make, _fuelConsumption, _tankCapacity);

            //assert
            Assert.That(car.FuelLevel, Is.EqualTo(0));
            Assert.That(car.Odometer, Is.EqualTo(0));
            Assert.That(car.DailyOdometer, Is.EqualTo(0));
        }

        [TestCase(1)]
        [TestCase(15)]
        [TestCase(50)]
        public void Refuel_TankEmptyFuelsNoMoreThanCapacity_SetsFuel(int refueledLiters)
        {
            //arrange

            //act
            _defaultCar.Refuel(refueledLiters);

            //assert
            Assert.That(_defaultCar.FuelLevel, Is.EqualTo((object)refueledLiters));
        }

        [TestCase(51)]
        [TestCase(int.MaxValue)]
        public void Refuel_FuelsOnceMoreThanCapacity_ThrowsException(int refueledLiters)
        {
            //arrange
            //act

            //assert
            Assert.Throws<Exception>(() => _defaultCar.Refuel(refueledLiters));
        }

        [TestCase(-int.MaxValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Refuel_FuelsZeroOrNegativeLiters_ThrowsException(int refueledLiters)
        {
            //arrange

            //act

            //assert
            Assert.Throws<ArgumentException>(() => _defaultCar.Refuel(refueledLiters));
        }

        [TestCase(1, 50)]
        [TestCase(25, 26)]
        [TestCase(26, 25)]
        public void Refuel_SecondRefuelIsTooMuchFuelsManyTimesMoreThanCapacity_ThrowsException(int refuel1, int refuel2)
        {
            //arrange
            //act

            //assert
            _defaultCar.Refuel(refuel1);

            Assert.Throws<Exception>(() => _defaultCar.Refuel(refuel2));
        }

        [TestCase(1, 1)]
        [TestCase(5, 15)]
        [TestCase(25, 25)]
        public void Refuel_FuelsManyTimesNoMoreThanCapacity_SetsFuel(int refuel1, int refuel2)
        {
            //arrange
            //act

            //assert
            _defaultCar.Refuel(refuel1);
            _defaultCar.Refuel(refuel2);

            Assert.That(_defaultCar.FuelLevel, Is.EqualTo(refuel1 + refuel2));
        }

        [TestCase(0)]
        [TestCase(15)]
        [TestCase(50)]
        public void Drive_EnoughFuelNoOdoReset_IncreasesOdometers(int drivenKms)
        {
            //arrange
            //act
            _defaultCar.Refuel(_defaultCar.TankCapacity);
            _defaultCar.Drive(drivenKms);

            //assert
            Assert.That(_defaultCar.Odometer, Is.EqualTo(drivenKms));
            Assert.That(_defaultCar.DailyOdometer, Is.EqualTo(drivenKms));
        }

        [TestCase(555)]
        [TestCase(int.MaxValue)]
        public void Drive_NotEnoughFuel_ThrowsException(int drivenKms)
        {
            //arrange

            //act
            //assert
            Assert.Throws<Exception>(() => _defaultCar.Drive(drivenKms));
        }

        [TestCase(0, 0)]
        [TestCase(1000, 0)]
        [TestCase(2600, 600)]
        public void Drive_MoreThanDailyOdoHolds_ResetsDailyOdometerAndKeepGoing(int kmsDriven, int odometerValue)
        {
            //arrange

            //act
            _longRangeCar.Drive(kmsDriven);

            //assert
            Assert.That(_longRangeCar.DailyOdometer, Is.EqualTo(odometerValue));
        }

        [TestCase(0, 0)]
        [TestCase(1000000, 0)]
        [TestCase(2600000, 600000)]
        public void Drive_DriveMoreThanOdoHolds_ResetsOdometerAndKeepGoing(int kmsDriven, int result)
        {
            //arrange

            //act
            _longRangeCar.Drive(kmsDriven);

            //assert
            Assert.That(_longRangeCar.Odometer, Is.EqualTo(result));
        }

        [Test]
        [TestCase(0)]
        [TestCase(500)]
        public void ResetDailyOdometer_Always_SetsDailyOdometerTo0(int distanceDriven)
        {
            //arrange
            _longRangeCar.Drive(distanceDriven);

            //act
            _longRangeCar.ResetDailyOdometer();

            //assert
            Assert.That(_longRangeCar.DailyOdometer, Is.EqualTo(0));
        }
    }
}