using Microsoft.EntityFrameworkCore.Migrations;
using TRAFFIK_API.Models;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCarDetailingServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, ensure we have the car types
            migrationBuilder.Sql(@"
                INSERT INTO ""CarTypes"" (""Type"") VALUES 
                ('Bike'),
                ('Sedan'), 
                ('SUV'),
                ('Minivan'),
                ('Truck'),
                ('Caravan/Boat')
                ON CONFLICT DO NOTHING;
            ");

            // Get car type IDs (assuming they exist or were just created)
            // We'll use a more robust approach with a temporary table
            migrationBuilder.Sql(@"
                CREATE TEMP TABLE temp_car_types AS
                SELECT ""Id"", ""Type"" FROM ""CarTypes"";
            ");

            // Insert all the car detailing services
            migrationBuilder.Sql(@"
                INSERT INTO ""ServiceCatalogs"" (""Name"", ""Description"", ""Price"", ""CarTypeId"") VALUES
                -- Basic Wash Packages
                ('Traffik Wash - Bike', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 238.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Bike')),
                ('Traffik Wash - Sedan', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 238.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Traffik Wash - SUV', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 298.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Traffik Wash - Minivan', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 348.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),
                ('Traffik Wash - Truck', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 398.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Truck')),
                ('Traffik Wash - Caravan/Boat', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside.', 948.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Caravan/Boat')),

                ('Traffik Special Wash - Bike', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 398.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Bike')),
                ('Traffik Special Wash - Sedan', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 398.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Traffik Special Wash - SUV', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 458.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Traffik Special Wash - Minivan', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 498.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),
                ('Traffik Special Wash - Truck', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 1458.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Truck')),
                ('Traffik Special Wash - Caravan/Boat', 'Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 1458.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Caravan/Boat')),

                ('Express Wash', 'Don''t have the time and only need a quick wash? This package is for you. (Exterior only – while in the line).', 158.00, NULL),
                ('Wash & Go - Bike/Sedan/SUV', 'Exterior wash only — fast and efficient.', 128.00, NULL),
                ('Wash & Go - Minivan/Truck/Caravan', 'Exterior wash only — fast and efficient.', 228.00, NULL),
                ('Vacuum & Go - Bike/Sedan/SUV', 'Interior vacuum only — ideal for quick cleanups.', 158.00, NULL),
                ('Vacuum & Go - Minivan/Truck/Caravan', 'Interior vacuum only — ideal for quick cleanups.', 228.00, NULL),

                -- Deep Cleaning Packages
                ('Decontamination Wash - Bike/Sedan/SUV', 'Engine cleaning, clay bar to remove the fallout on the paint surface. Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 658.00, NULL),
                ('Decontamination Wash - Minivan/Truck/Caravan', 'Engine cleaning, clay bar to remove the fallout on the paint surface. Wash, vacuum, wipe inside, polish tyres and shine the plastics on the outside with a hand polish.', 1208.00, NULL),
                ('Chassis Wash - Bike', 'We have ramps for 4x4 and high cars, so we can wash the chassis and remove all the mud and dirt that gets stuck to the bottom of the car. This package is ideal for off-road vehicles.', 398.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Bike')),
                ('Chassis Wash - Sedan/SUV/Minivan', 'We have ramps for 4x4 and high cars, so we can wash the chassis and remove all the mud and dirt that gets stuck to the bottom of the car. This package is ideal for off-road vehicles.', 250.00, NULL),

                -- Valet Packages
                ('Traffik Mini Valet - Sedan', 'Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.', 458.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Traffik Mini Valet - SUV', 'Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.', 658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Traffik Mini Valet - Minivan', 'Basic valet package: vacuum interior, wipe down dash and panels, clean windows, wipe door handles, aircon vents, door sills, dashboard, and console. Not a deep clean package.', 858.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Traffik Valet - Sedan', 'Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.', 528.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Traffik Valet - SUV', 'Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.', 758.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Traffik Valet - Minivan', 'Basic valet package plus: vacuum seats and carpets, wipe panels, roofline, seatbelts and dashboard. Deep clean seats, carpets, roof lining, panels, and dashboard. Clean windows and door handles. Interior is deep cleaned and treated with a fabric cleaner and shampoo machine.', 958.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Premium Mini Valet - Sedan', 'Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.', 528.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Premium Mini Valet - SUV', 'Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.', 658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Premium Mini Valet - Minivan', 'Deep clean and polish on the outside using a machine and hand polish. Clean windows, door handles, and dashboard. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard.', 658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Deluxe Valet - Sedan', 'Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.', 658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Deluxe Valet - SUV', 'Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.', 958.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Deluxe Valet - Minivan', 'Signature valet: deep clean seats, carpets, roof lining, panels, dashboard, windows, door handles, and aircon vents. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Exterior is treated with a combination of hand polish and machine polish to remove oxidation and polish finish of the outside paint. Clean engine and wheels.', 958.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Premium Detail Valet - Sedan', 'Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.', 850.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Premium Detail Valet - SUV', 'Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.', 958.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Premium Detail Valet - Minivan', 'Best valet package: deep clean inside and outside of the vehicle from top to bottom. Clean and shine the engine, wheels, and tires. Treat leather with leather lotion and wipe down plastics with shine plastics solution. Vacuum seats and carpets, wipe panels and dashboard. Deep clean and polish the exterior with a combination of machine and hand polish. Includes a ceramic wax treatment and polish of the outside paintwork.', 958.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                -- Polishing & Restoration
                ('Claybar & Polish - Sedan', 'Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle''s paintwork.', 185.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Claybar & Polish - SUV', 'Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle''s paintwork.', 208.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Claybar & Polish - Minivan', 'Decontaminates and polishes the vehicle to remove light industrial fallout and aggressive industrial fallout before a wash, wax, and machine polish. Includes a hand polish with a buffer and shine to enhance the vehicle''s paintwork.', 1808.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Machine Polish - Sedan', 'Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.', 2658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Machine Polish - SUV', 'Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.', 3658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Machine Polish - Minivan', 'Similar to Claybar & Polish but includes machine polish with buff and shine to enhance the paintwork.', 5088.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Premium Detail Polish - Sedan', 'Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.', 3658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Sedan')),
                ('Premium Detail Polish - SUV', 'Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.', 4658.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'SUV')),
                ('Premium Detail Polish - Minivan', 'Includes decontamination to remove light and aggressive industrial fallout, clay-bar treatment, machine polish, and buffing. Offers a choice between ceramic coating or traditional finishing treatment.', 7058.00, (SELECT ""Id"" FROM temp_car_types WHERE ""Type"" = 'Minivan')),

                ('Light Refurbishment', 'Restores faded texture of front lights to a clear shine through polishing.', 398.00, NULL);
            ");

            // Clean up temporary table
            migrationBuilder.Sql("DROP TABLE temp_car_types;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove all the car detailing services
            migrationBuilder.Sql(@"
                DELETE FROM ""ServiceCatalogs"" 
                WHERE ""Name"" LIKE '%Traffik%' 
                   OR ""Name"" LIKE '%Express%' 
                   OR ""Name"" LIKE '%Wash & Go%' 
                   OR ""Name"" LIKE '%Vacuum & Go%' 
                   OR ""Name"" LIKE '%Decontamination%' 
                   OR ""Name"" LIKE '%Chassis%' 
                   OR ""Name"" LIKE '%Valet%' 
                   OR ""Name"" LIKE '%Claybar%' 
                   OR ""Name"" LIKE '%Machine Polish%' 
                   OR ""Name"" LIKE '%Premium Detail%' 
                   OR ""Name"" LIKE '%Light Refurbishment%';
            ");
        }
    }
}
