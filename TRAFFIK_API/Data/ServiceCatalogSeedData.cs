using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Data
{
    public static class ServiceCatalogSeedData
    {
        public static async Task SeedServicesAsync(AppDbContext context)
        {
            // Ensure car types exist
            await EnsureCarTypesExistAsync(context);

            // Get car type IDs
            var bikeId = await GetVehicleTypeIdAsync(context, "Bike");
            var sedanId = await GetVehicleTypeIdAsync(context, "Sedan");
            var suvId = await GetVehicleTypeIdAsync(context, "SUV");
            var minivanId = await GetVehicleTypeIdAsync(context, "Minivan");
            var truckId = await GetVehicleTypeIdAsync(context, "Truck");
            var caravanBoatId = await GetVehicleTypeIdAsync(context, "Caravan/Boat");

            // Check if services already exist
            if (context.ServiceCatalogs.Any(s => s.Name.Contains("Traffik")))
            {
                return; // Services already seeded
            }

            var services = new List<ServiceCatalog>
            {
                // Basic Wash Packages
                new() { Name = "Traffik Wash - Bike", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 238.00m, VehicleTypeId = bikeId },
                new() { Name = "Traffik Wash - Sedan", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 238.00m, VehicleTypeId = sedanId },
                new() { Name = "Traffik Wash - SUV", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 298.00m, VehicleTypeId = suvId },
                new() { Name = "Traffik Wash - Minivan", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 348.00m, VehicleTypeId = minivanId },
                new() { Name = "Traffik Wash - Truck", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 398.00m, VehicleTypeId = truckId },
                new() { Name = "Traffik Wash - Caravan/Boat", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.", Price = 948.00m, VehicleTypeId = caravanBoatId },

                new() { Name = "Traffik Special Wash - Bike", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 398.00m, VehicleTypeId = bikeId },
                new() { Name = "Traffik Special Wash - Sedan", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 398.00m, VehicleTypeId = sedanId },
                new() { Name = "Traffik Special Wash - SUV", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 458.00m, VehicleTypeId = suvId },
                new() { Name = "Traffik Special Wash - Minivan", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 498.00m, VehicleTypeId = minivanId },
                new() { Name = "Traffik Special Wash - Truck", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 1458.00m, VehicleTypeId = truckId },
                new() { Name = "Traffik Special Wash - Caravan/Boat", Description = "Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 1458.00m, VehicleTypeId = caravanBoatId },

                new() { Name = "Express Wash", Description = "Don't have the time and only need a quick wash? This package is for you. (Exterior only – while in the line).", Price = 158.00m, VehicleTypeId = null },
                new() { Name = "Wash & Go - Bike/Sedan/SUV", Description = "Exterior wash only — fast and efficient.", Price = 128.00m, VehicleTypeId = null },
                new() { Name = "Wash & Go - Minivan/Truck/Caravan", Description = "Exterior wash only — fast and efficient.", Price = 228.00m, VehicleTypeId = null },
                new() { Name = "Vacuum & Go - Bike/Sedan/SUV", Description = "Interior vacuum only — ideal for quick cleanups.", Price = 158.00m, VehicleTypeId = null },
                new() { Name = "Vacuum & Go - Minivan/Truck/Caravan", Description = "Interior vacuum only — ideal for quick cleanups.", Price = 228.00m, VehicleTypeId = null },

                // Deep Cleaning Packages
                new() { Name = "Decontamination Wash - Bike/Sedan/SUV", Description = "Engine cleaning, clay bar to remove the fallout on the paint surface. Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 658.00m, VehicleTypeId = null },
                new() { Name = "Decontamination Wash - Minivan/Truck/Caravan", Description = "Engine cleaning, clay bar to remove the fallout on the paint surface. Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.", Price = 1208.00m, VehicleTypeId = null },
                new() { Name = "Chassis Wash - Bike", Description = "We have ramps for 4x4 and high cars, so we can wash the chassis and remove all the mud and dirt that gets stuck to the bottom of the car. This package is ideal for off-road vehicles.", Price = 398.00m, VehicleTypeId = bikeId },
                new() { Name = "Chassis Wash - Sedan/SUV/Minivan", Description = "We have ramps for 4x4 and high cars, so we can wash the chassis and remove all the mud and dirt that gets stuck to the bottom of the car. This package is ideal for off-road vehicles.", Price = 250.00m, VehicleTypeId = null },

                // Valet Packages
                new() { Name = "Traffik Mini Valet - Sedan", Description = "Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.", Price = 458.00m, VehicleTypeId = sedanId },
                new() { Name = "Traffik Mini Valet - SUV", Description = "Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.", Price = 658.00m, VehicleTypeId = suvId },
                new() { Name = "Traffik Mini Valet - Minivan", Description = "Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.", Price = 858.00m, VehicleTypeId = minivanId },

                new() { Name = "Traffik Valet - Sedan", Description = "Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.", Price = 528.00m, VehicleTypeId = sedanId },
                new() { Name = "Traffik Valet - SUV", Description = "Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.", Price = 758.00m, VehicleTypeId = suvId },
                new() { Name = "Traffik Valet - Minivan", Description = "Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.", Price = 958.00m, VehicleTypeId = minivanId },

                new() { Name = "Premium Mini Valet - Sedan", Description = "Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.", Price = 528.00m, VehicleTypeId = sedanId },
                new() { Name = "Premium Mini Valet - SUV", Description = "Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.", Price = 658.00m, VehicleTypeId = suvId },
                new() { Name = "Premium Mini Valet - Minivan", Description = "Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.", Price = 658.00m, VehicleTypeId = minivanId },

                new() { Name = "Deluxe Valet - Sedan", Description = "Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.", Price = 658.00m, VehicleTypeId = sedanId },
                new() { Name = "Deluxe Valet - SUV", Description = "Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.", Price = 958.00m, VehicleTypeId = suvId },
                new() { Name = "Deluxe Valet - Minivan", Description = "Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.", Price = 958.00m, VehicleTypeId = minivanId },

                new() { Name = "Premium Detail Valet - Sedan", Description = "Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.", Price = 850.00m, VehicleTypeId = sedanId },
                new() { Name = "Premium Detail Valet - SUV", Description = "Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.", Price = 958.00m, VehicleTypeId = suvId },
                new() { Name = "Premium Detail Valet - Minivan", Description = "Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.", Price = 958.00m, VehicleTypeId = minivanId },

                // Polishing & Restoration
                new() { Name = "Claybar & Polish - Sedan", Description = "Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle's paintwork.", Price = 185.00m, VehicleTypeId = sedanId },
                new() { Name = "Claybar & Polish - SUV", Description = "Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle's paintwork.", Price = 208.00m, VehicleTypeId = suvId },
                new() { Name = "Claybar & Polish - Minivan", Description = "Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle's paintwork.", Price = 1808.00m, VehicleTypeId = minivanId },

                new() { Name = "Machine Polish - Sedan", Description = "Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.", Price = 2658.00m, VehicleTypeId = sedanId },
                new() { Name = "Machine Polish - SUV", Description = "Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.", Price = 3658.00m, VehicleTypeId = suvId },
                new() { Name = "Machine Polish - Minivan", Description = "Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.", Price = 5088.00m, VehicleTypeId = minivanId },

                new() { Name = "Premium Detail Polish - Sedan", Description = "Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.", Price = 3658.00m, VehicleTypeId = sedanId },
                new() { Name = "Premium Detail Polish - SUV", Description = "Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.", Price = 4658.00m, VehicleTypeId = suvId },
                new() { Name = "Premium Detail Polish - Minivan", Description = "Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.", Price = 7058.00m, VehicleTypeId = minivanId },

                new() { Name = "Light Refurbishment", Description = "Restores faded texture of front lights to a clear shine through polishing.", Price = 398.00m, VehicleTypeId = null }
            };

            context.ServiceCatalogs.AddRange(services);
            await context.SaveChangesAsync();
        }

        private static async Task EnsureCarTypesExistAsync(AppDbContext context)
        {
            var carTypes = new[] { "Bike", "Sedan", "SUV", "Minivan", "Truck", "Caravan/Boat" };
            
            foreach (var carType in carTypes)
            {
                if (!context.VehicleTypes.Any(ct => ct.Type == carType))
                {
                    context.VehicleTypes.Add(new VehicleType { Type = carType });
                }
            }
            
            await context.SaveChangesAsync();
        }

        private static async Task<int> GetVehicleTypeIdAsync(AppDbContext context, string carTypeName)
        {
            var carType = await context.VehicleTypes.FirstOrDefaultAsync(ct => ct.Type == carTypeName);
            return carType?.Id ?? 0;
        }
    }
}
