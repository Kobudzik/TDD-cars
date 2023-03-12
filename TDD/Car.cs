using TDDTests;

namespace Tdd
{
    public class Car
    {
        public string HexColor { get; }
        public CarMake Make { get; }

        /// <summary>
        /// Consumption per 100 km
        /// </summary>
        public float FuelConsumption { get; }
        public int TankCapacity { get; }
        public int DailyOdometer { get; private set; }
        public int Odometer { get; private set; }
        public int FuelLevel { get; private set; }
        public int OdometerCapacity { get; } = 999999;
        public int DailyOdometerCapacity { get; } = 999;

        public Car(string color, CarMake make, float fuelConsumption, int tankCapacity)
        {
            HexColor = color;
            Make = make;
            FuelConsumption = fuelConsumption;
            TankCapacity = tankCapacity;
        }

        public void Refuel(int liters)
        {
            if (liters <= 0)
                throw new ArgumentException("Cannot refuel less than or equal to 0 liters.");
            if (liters > TankCapacity - FuelLevel)
                throw new Exception($"Cannot fuel more than tank capacity: {TankCapacity}");

            FuelLevel += liters;
        }

        public void Drive(int distanceKm)
        {
            var range = FuelLevel / FuelConsumption * 100;
            if (distanceKm > range)
                throw new Exception("Car ran out of fuel");

            Odometer = GetOdometerValue(distanceKm, OdometerCapacity, Odometer);
            DailyOdometer = GetOdometerValue(distanceKm, DailyOdometerCapacity, DailyOdometer);
        }

        private int GetOdometerValue(int distanceToAdd, int odometerCapacity, int odometerValueBefore)
        {
            var odometerOverflowTimes = Math.Floor((decimal)distanceToAdd / odometerCapacity);
            var result = (odometerValueBefore + distanceToAdd) - (odometerCapacity * odometerOverflowTimes + odometerOverflowTimes);
            return (int)result;
        }

        public void ResetDailyOdometer()
            => DailyOdometer = 0;
    }
}
