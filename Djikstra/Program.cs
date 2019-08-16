using System;

namespace Djikstra
{
    class Program
    {
        static void Main(string[] args)
        {
            CityFlights flights = new CityFlights();
            flights.SetUpCityFlights();
            Console.WriteLine($"${flights.GetCheapestConnectingFlightFrom("Atlanta", "Chicago")}");
            Console.WriteLine($"${flights.GetCheapestConnectingFlightFrom("Atlanta", "Boston")}");
            Console.WriteLine($"${flights.GetCheapestConnectingFlightFrom("Atlanta", "El Peso")}");
            Console.Read();
        }
    }
}
