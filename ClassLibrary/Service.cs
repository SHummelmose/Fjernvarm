using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Service
    {
        private static DistrictHeatingEntities DB = new DistrictHeatingEntities();
        private const double PRICE = 0.875;

        /**
         * Returns all consumers
         **/
        public static List<Consumer> GetConsumers()
        {
            return DB.Consumers.ToList();
        }

        /**
         * Creates a consumer based on the parameters
         **/
        public static void CreateConsumer(string name, string address, string zip, string city)
        {
            Consumer c = new Consumer { Name = name, Address = address, ZipCode = zip, City = city };
            DB.Consumers.Add(c);
            DB.SaveChanges();
        }

        /**
         * Updates the consumer based on the parameters
         **/
        public static void UpdateConsumer(Consumer c, string name, string address, string zip, string city)
        {
            c.Name = name;
            c.Address = address;
            c.ZipCode = zip;
            c.City = city;
            DB.SaveChanges();
        }

        /**
         * Deletes the consumer
         **/
        public static void DeleteConsumer(Consumer consumer)
        {
            foreach (var g in consumer.Gauges.ToList())
            {
                DB.Gauges.Remove(g);
            }
            DB.Consumers.Remove(consumer);
            DB.SaveChanges();
        }

        /**
         * Checks if the consumer has any readings on it's gauges
         **/
        public static bool CheckConsumerDeleteable(Consumer consumer)
        {
            bool deleteable = true;
            foreach (var g in consumer.Gauges)
            {
                if (g.Readings.Count != 0)
                {
                    deleteable = false;
                }
            }
            return deleteable;
        }


        /**
        * Returns the reading of the specified gauge and year. Can return null if no such exists.
        **/
        public static Reading GetGaugeReadingOnYear(int gaugeID, string year)
        {
            Reading r = (from x in DB.Readings
                         where x.GaugeID == gaugeID && x.Year == year
                         select x).FirstOrDefault();
            return r;
        }

        /**
         * Creates a gauge based on the parameters
         **/
        public static void CreateGauge(int consumerID, string description)
        {
            Gauge g = new Gauge { ConsumerID = consumerID, Description = description };
            DB.Gauges.Add(g);
            DB.SaveChanges();
        }

        /**
         * Creates a reading based on the parameters
         **/
        public static void CreateReading(int kWh, int m3, int hours, string year, int gaugeID)
        {
            Reading r = new Reading { KilowattHours = kWh, CubicMeters = m3, Hours = hours, Year = year, GaugeID = gaugeID };
            DB.Readings.Add(r);
            DB.SaveChanges();
        }

        /**
         * Returns the active year. If there is none, return null.
         **/
        public static string GetActiveYear()
        {
            string year = (from y in DB.ActiveYears
                           where y.IsActive == true
                           select y.Year).FirstOrDefault();
            return year;
        }

        /**
         * Checks if the year is active. If it is, returns true. If it's not or isn't in database, returns false.
         **/
        public static bool CheckActiveYear(string year)
        {
            ActiveYear y = (from x in DB.ActiveYears
                            where x.Year == year
                            select x).FirstOrDefault();

            if (y == null || !y.IsActive)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /**
         * Activates a given year and deactivates all others. Creates the year if it doesn't exist.
         **/
        public static void ActivateYear(string year)
        {
            //Deactivate all years
            foreach (var active in DB.ActiveYears)
            {
                active.IsActive = false;
            }

            //Find given year
            ActiveYear y = (from x in DB.ActiveYears
                            where x.Year == year
                            select x).FirstOrDefault();

            //If exists, activate
            if (y != null)
            {
                y.IsActive = true;
                DB.SaveChanges();
            }
            else //If not exists, create
            {
                ActiveYear a = new ActiveYear { Year = year, IsActive = true };
                DB.ActiveYears.Add(a);
                DB.SaveChanges();
            }
        }

        /**
         * Turns IsActive to false on given year.
         **/
        public static void DeactivateYear(string year)
        {
            ActiveYear y = (from x in DB.ActiveYears
                            where x.Year == year
                            select x).FirstOrDefault();

            y.IsActive = false;
            DB.SaveChanges();
        }

        /**
         * Returns a list of years with recorded readings
         **/
        public static List<string> GetYearsWithReadings()
        {
            List<string> years = (from r in DB.Readings
                                  select r.Year).Distinct().ToList();
            years.Sort();
            return years;
        }

        /**
         * Calculates the cooling based on kWh and m3. Returns true if OK, false if not.
         **/
        public static bool CheckCoolingOnReading(int kilo, int cubic)
        {
            double cooling = 0.86 * kilo / cubic;
            if (cooling >= 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Calculates the cooling of a reading based on the year and gauge
         **/
        public static double CalculateCoolingOnYear(Gauge g, string year)
        {
            Reading r = (from x in DB.Readings
                         where x.GaugeID == g.ID && x.Year == year
                         select x).FirstOrDefault();

            if (r != null)
            {
                int kWh = r.KilowattHours;
                int m3 = r.CubicMeters;

                double cooling = 0.86 * kWh / m3;
                return Math.Round(cooling, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                return -1;
            }
        }

        /**
         * Calculates the price of a reading based on the year and gauge
         **/
        public static double CalculatePriceOnYear(Gauge g, string year)
        {
            Reading r = (from x in DB.Readings
                         where x.GaugeID == g.ID && x.Year == year
                         select x).FirstOrDefault();

            if (r != null)
            {
                int kWh = r.KilowattHours;

                double price = kWh * PRICE;
                return Math.Round(price, 2, MidpointRounding.AwayFromZero);

            }
            else
            {
                return -1;
            }
        }

        /**
         * Returns a list of what years there a reading on the gauge
         **/
        public static List<string> GetYearsOnGauge(Gauge g)
        {
            List<string> years = (from r in DB.Readings
                                  where r.GaugeID == g.ID
                                  select r.Year).ToList();
            years.Sort();
            return years;
        }

        /**
         * Checks the login info against the database. Returns true if the info is correct, false if not.
         **/
        public static bool AuthenticateLogin(int gaugeID, string password)
        {
            Gauge gauge = (from g in DB.Gauges
                           where g.ID == gaugeID
                           select g).FirstOrDefault();

            if (gauge == null)
            {
                return false;
            }

            if (gauge.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Returns an array of the three average values: cooling, price and hours on a given year.
         **/
        public static double[] AverageReadingsOnYear(string year)
        {
            List<Reading> readings = (from r in DB.Readings
                                      where r.Year == year
                                      select r).ToList();
            double totalCooling = 0;
            double totalPrice = 0;
            int totalHour = 0;

            foreach (Reading r in readings)
            {
                totalCooling += 0.86 * r.KilowattHours / r.CubicMeters;
                totalPrice += r.KilowattHours * PRICE;
                totalHour += r.Hours;
            }

            double avgCooling = totalCooling / readings.Count;
            double avgPrice = totalPrice / readings.Count;
            double avgHours = totalHour / readings.Count;

            return new double[] {
                Math.Round(avgCooling,2,MidpointRounding.AwayFromZero),
                Math.Round(avgPrice, 2, MidpointRounding.AwayFromZero),
                Math.Round(avgHours, 2, MidpointRounding.AwayFromZero)
            };
        }
    }
}
