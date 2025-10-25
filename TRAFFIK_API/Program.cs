using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TRAFFIK_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Ensure VehicleLicensePlate column exists and seed service catalog data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Add missing columns and tables
    try
    {
        await context.Database.ExecuteSqlRawAsync(@"
            DO $$ 
            BEGIN
                -- Add VehicleLicensePlate column if it doesn't exist
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'Bookings' AND column_name = 'VehicleLicensePlate'
                ) THEN
                    ALTER TABLE ""Bookings"" ADD COLUMN ""VehicleLicensePlate"" text;
                END IF;
                
                -- Make ServiceId nullable if it's not already
                IF EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'Bookings' AND column_name = 'ServiceId' AND is_nullable = 'NO'
                ) THEN
                    ALTER TABLE ""Bookings"" ALTER COLUMN ""ServiceId"" DROP NOT NULL;
                END IF;
                
                -- Create RewardItems table if it doesn't exist
                IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'RewardItems') THEN
                    CREATE TABLE ""RewardItems"" (
                        ""Id"" SERIAL PRIMARY KEY,
                        ""Name"" text NOT NULL,
                        ""Description"" text NOT NULL,
                        ""Cost"" integer NOT NULL,
                        ""ImageUrl"" text,
                        ""IsAvailable"" boolean NOT NULL DEFAULT true
                    );
                END IF;
                
                -- Create RewardRedemptions table if it doesn't exist
                IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'RewardRedemptions') THEN
                    CREATE TABLE ""RewardRedemptions"" (
                        ""Id"" SERIAL PRIMARY KEY,
                        ""UserId"" integer NOT NULL,
                        ""ItemId"" integer NOT NULL,
                        ""RedeemedAt"" timestamp with time zone NOT NULL,
                        ""Used"" boolean NOT NULL DEFAULT false,
                        FOREIGN KEY (""UserId"") REFERENCES ""Users""(""Id""),
                        FOREIGN KEY (""ItemId"") REFERENCES ""RewardItems""(""Id"")
                    );
                END IF;
            END $$;
        ");
        Console.WriteLine("✅ Database schema updated: Added missing tables and columns");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Warning: Could not update database schema: {ex.Message}");
    }
    
    await ServiceCatalogSeedData.SeedServicesAsync(context);
}

app.Run();