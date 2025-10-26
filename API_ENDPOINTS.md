# TRAFFIK API Endpoints Reference

## 🚗 Vehicles
- `GET /api/vehicle` - Get all vehicles
- `GET /api/vehicle/{licensePlate}` - Get specific vehicle
- `POST /api/vehicle` - Create vehicle (requires VehicleTypeId)
- `PUT /api/vehicle/{licensePlate}` - Update vehicle
- `DELETE /api/vehicle/{licensePlate}` - Delete vehicle
- `GET /api/vehicle/User/{userId}` - Get user's vehicles
- `GET /api/vehicle/Types` - Get all vehicle types (with IDs)

## 📋 Bookings
- `GET /api/Bookings` - Get all bookings
- `GET /api/Bookings/{id}` - Get specific booking
- `GET /api/Bookings/User/{userId}` - Get user's bookings
- `POST /api/Bookings` - Create booking
- `POST /api/Bookings/Confirm` - Confirm booking (checks conflicts)
- `PUT /api/Bookings/{id}` - Update booking
- `DELETE /api/Bookings/{id}` - Delete booking
- `GET /api/Bookings/AvailableSlots?serviceCatalogId=X&date=YYYY-MM-DD` - Get available time slots

## 🧹 Services
- `GET /api/ServiceCatalogs` - Get all services
- `GET /api/ServiceCatalogs?vehicleTypeId={id}` - Get services filtered by vehicle type
- `GET /api/ServiceCatalogs/{id}` - Get specific service
- `GET /api/ServiceCatalogs/ForVehicle/{licensePlate}?sortBy=price&direction=asc` - Get services for a vehicle
- `GET /api/ServiceCatalogs/ByVehicleType/{vehicleTypeId}?sortBy=name` - Get services by vehicle type ID
- `POST /api/ServiceCatalogs` - Create service
- `PUT /api/ServiceCatalogs/{id}` - Update service
- `DELETE /api/ServiceCatalogs/{id}` - Delete service

## 👤 Users
- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get specific user
- `POST /api/Users` - Create user
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user

## 📝 Booking Stages
- `GET /api/BookingStages` - Get all booking stages
- `GET /api/BookingStages/{id}` - Get specific booking stage
- `GET /api/BookingStages/Booking/{bookingId}` - Get stages for booking
- `POST /api/BookingStages` - Create booking stage

## 🔔 Notifications
- `GET /api/Notifications` - Get all notifications
- `GET /api/Notifications/{id}` - Get specific notification
- `GET /api/Notifications/User/{userId}` - Get user's notifications
- `POST /api/Notifications` - Create notification
- `PUT /api/Notifications/{id}` - Update notification
- `DELETE /api/Notifications/{id}` - Delete notification

## 💳 Payments
- `GET /api/Payments` - Get all payments
- `GET /api/Payments/{id}` - Get specific payment
- `GET /api/Payments/Booking/{bookingId}` - Get payments for booking
- `POST /api/Payments` - Create payment
- `PUT /api/Payments/{id}` - Update payment

## 🎁 Rewards
- `GET /api/Rewards` - Get all rewards
- `GET /api/Rewards/{id}` - Get specific reward
- `GET /api/Rewards/User/{userId}` - Get user's rewards
- `POST /api/Rewards` - Create reward

## 🏆 Reward Catalog
- `GET /api/RewardCatalog` - Get reward catalog items
- `GET /api/RewardCatalog/user/{userId}/redeemed` - Get user's redeemed items
- `POST /api/RewardCatalog/redeem/{itemId}` - Redeem reward item

## 📸 Instagram Posts
- `GET /api/InstagramPosts` - Get all posts
- `GET /api/InstagramPosts/{id}` - Get specific post
- `POST /api/InstagramPosts` - Create post
- `PUT /api/InstagramPosts/{id}` - Update post
- `DELETE /api/InstagramPosts/{id}` - Delete post

## 📊 Service History
- `GET /api/ServiceHistory/All` - Get all service history
- `GET /api/ServiceHistory/Vehicle/{licensePlate}` - Get history for vehicle
- `POST /api/ServiceHistory/TrackWash` - Track completed service

## 👥 User Roles
- `GET /api/UserRole` - Get all roles
- `GET /api/UserRole/{id}` - Get specific role
- `POST /api/UserRole` - Create role
- `PUT /api/UserRole/{id}` - Update role
- `DELETE /api/UserRole/{id}` - Delete role

## 🚙 Vehicle Types
- `GET /api/VehicleTypes` - Get all vehicle types
- `GET /api/VehicleTypes/{id}` - Get specific vehicle type
- `POST /api/VehicleTypes` - Create vehicle type
- `PUT /api/VehicleTypes/{id}` - Update vehicle type
- `DELETE /api/VehicleTypes/{id}` - Delete vehicle type

## 🎁 Reward Items (Catalog)
- `GET /api/RewardItems` - Get all reward items
- `GET /api/RewardItems/{id}` - Get specific reward item
- `POST /api/RewardItems` - Create reward item
- `PUT /api/RewardItems/{id}` - Update reward item
- `DELETE /api/RewardItems/{id}` - Delete reward item

---

## Key Data Structures

### Vehicle
```json
{
  "licensePlate": "string (PK)",
  "userId": "int",
  "vehicleTypeId": "int (REQUIRED)",
  "make": "string",
  "model": "string",
  "year": "int",
  "color": "string",
  "imageUrl": "string"
}
```

### Booking
```json
{
  "id": "int",
  "userId": "int",
  "serviceCatalogId": "int",
  "vehicleLicensePlate": "string",
  "bookingTime": "TimeOnly",
  "bookingDate": "DateOnly",
  "status": "string"
}
```

### ServiceCatalog
```json
{
  "id": "int",
  "name": "string",
  "description": "string",
  "price": "decimal",
  "vehicleTypeId": "int? (nullable = available for all types)"
}
```

### VehicleType
```json
{
  "id": "int",
  "type": "string (e.g., 'Sedan', 'SUV', 'Bike')"
}
```

---

## Important Notes

✅ **All foreign keys are properly linked**
✅ **Vehicle must have VehicleTypeId when creating/updating**
✅ **Services can be filtered by VehicleTypeId (null = all types)**
✅ **CORS is enabled for all origins**
✅ **Swagger UI available at `/swagger`**
✅ **Ready for migration: `dotnet ef migrations add InitialCreate`**

