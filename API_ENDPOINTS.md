# TRAFFIK API Endpoints Reference

## Vehicles
- `GET /api/vehicle` - Get all vehicles
- `GET /api/vehicle/{licensePlate}` - Get specific vehicle by license plate
- `POST /api/vehicle` - Create vehicle (requires VehicleTypeId and UserId)
- `PUT /api/vehicle/{licensePlate}` - Update vehicle
- `DELETE /api/vehicle/{licensePlate}` - Delete vehicle
- `GET /api/vehicle/User/{userId}` - Get all vehicles for a user
- `GET /api/vehicle/Types` - Get all vehicle types (ordered alphabetically)

## Bookings
- `GET /api/Bookings` - Get all bookings
- `GET /api/Bookings/{id}` - Get specific booking by ID
- `GET /api/Bookings/User/{userId}` - Get all bookings for a user (returns DTO)
- `POST /api/Bookings` - Create booking (expects BookingRequest wrapper)
- `POST /api/Bookings/Confirm` - Confirm booking with conflict checking
- `PUT /api/Bookings/{id}` - Update booking
- `DELETE /api/Bookings/{id}` - Delete booking
- `GET /api/Bookings/AvailableSlots?serviceCatalogId={id}&date={YYYY-MM-DD}` - Get available time slots

## Services
- `GET /api/ServiceCatalogs` - Get all services (includes VehicleType navigation)
- `GET /api/ServiceCatalogs?vehicleTypeId={id}` - Get services filtered by vehicle type ID (returns specific type + generic services)
- `GET /api/ServiceCatalogs/{id}` - Get specific service
- `GET /api/ServiceCatalogs/ForVehicle/{licensePlate}?sortBy={name|price}&direction={asc|desc}` - Get services for a vehicle with sorting
- `GET /api/ServiceCatalogs/ByVehicleType/{vehicleTypeId}?sortBy={name|price}&direction={asc|desc}` - Get services by vehicle type with sorting
- `POST /api/ServiceCatalogs` - Create service
- `PUT /api/ServiceCatalogs/{id}` - Update service
- `DELETE /api/ServiceCatalogs/{id}` - Delete service

## Users
- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get specific user
- `POST /api/Users` - Create user
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user

## Booking Stages
- `GET /api/BookingStages` - Get all booking stages
- `GET /api/BookingStages/{id}` - Get specific booking stage
- `POST /api/BookingStages` - Create booking stage
- `POST /api/BookingStages/UpdateStage` - Update stage (requires Employee/Admin role)
- `PUT /api/BookingStages/{id}` - Update booking stage
- `DELETE /api/BookingStages/{id}` - Delete booking stage

## Notifications
- `GET /api/Notifications` - Get all notifications
- `GET /api/Notifications/{id}` - Get specific notification
- `POST /api/Notifications` - Create notification
- `PUT /api/Notifications/{id}` - Update notification
- `DELETE /api/Notifications/{id}` - Delete notification

## Payments
- `GET /api/Payments` - Get all payments
- `GET /api/Payments/{id}` - Get specific payment
- `POST /api/Payments` - Create payment
- `PUT /api/Payments/{id}` - Update payment
- `DELETE /api/Payments/{id}` - Delete payment

## Rewards
- `GET /api/Reward` - Get all rewards
- `GET /api/Reward/{id}` - Get specific reward
- `GET /api/Reward/User/{userId}/balance` - Get user's reward points balance
- `POST /api/Reward` - Create reward
- `POST /api/Reward/earn` - Earn reward points (1 point per R10 spent)
- `POST /api/Reward/redeem` - Redeem reward points
- `PUT /api/Reward/{id}` - Update reward
- `DELETE /api/Reward/{id}` - Delete reward

## Reward Catalog
- `GET /api/RewardCatalog` - Get all reward catalog items
- `GET /api/RewardCatalog/user/{userId}/redeemed` - Get user's redeemed reward items
- `POST /api/RewardCatalog/redeem/{itemId}` - Redeem a reward item (consumes points)
- `POST /api/RewardCatalog/user/{userId}/redeemed/{itemId}/use` - Mark redeemed item as used

## Instagram Posts
- `GET /api/InstagramPost` - Get all Instagram posts (requires access token)

## Service History
- `GET /api/ServiceHistory/All` - Get all service history (includes User, Vehicle, ServiceCatalog navigation)
- `GET /api/ServiceHistory/Vehicle/{vehicleLicensePlate}` - Get service history for a vehicle
- `POST /api/ServiceHistory/TrackWash` - Track completed service (expects ServiceHistoryDto)

## User Roles
- `GET /api/UserRole` - Get all user roles
- `GET /api/UserRole/{id}` - Get specific user role
- `POST /api/UserRole` - Create user role
- `PUT /api/UserRole/{id}` - Update user role
- `DELETE /api/UserRole/{id}` - Delete user role

## Vehicle Types
- `GET /api/VehicleTypes` - Get all vehicle types
- `GET /api/VehicleTypes/{id}` - Get specific vehicle type
- `POST /api/VehicleTypes` - Create vehicle type
- `PUT /api/VehicleTypes/{id}` - Update vehicle type
- `DELETE /api/VehicleTypes/{id}` - Delete vehicle type

## Reward Items
- `GET /api/RewardItems` - Get all reward items
- `GET /api/RewardItems/{id}` - Get specific reward item
- `POST /api/RewardItems` - Create reward item
- `PUT /api/RewardItems/{id}` - Update reward item
- `DELETE /api/RewardItems/{id}` - Delete reward item

## Authentication
- `POST /api/Auth/Register` - Register new user (returns user info without password)
- `POST /api/Auth/Login` - Login user (returns user profile)
- `POST /api/Auth/Logout` - Logout user
- `DELETE /api/Auth/Delete/{id}` - Delete user account

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

- All foreign keys are properly linked
- Vehicle creation requires VehicleTypeId and UserId
- Services can be filtered by VehicleTypeId (null indicates available for all types)
- Bookings use BookingRequest wrapper: `{ "Booking": {...} }`
- Reward points: 1 point per R10 spent
- Password hashing: SHA256
- CORS is enabled for all origins
- Swagger UI available at `/swagger`
- Database: PostgreSQL with Entity Framework Core
- Instagram Post endpoint requires access token configuration
- JWT authentication not yet implemented

## Request/Response Examples

### Create Vehicle
```json
POST /api/vehicle
{
  "userId": 1,
  "licensePlate": "ABC123",
  "make": "Toyota",
  "model": "Corolla",
  "year": 2020,
  "color": "White",
  "imageUrl": "https://example.com/image.jpg",
  "vehicleTypeId": 3
}
```

### Create Booking
```json
POST /api/Bookings
{
  "Booking": {
    "userId": 1,
    "serviceCatalogId": 5,
    "vehicleLicensePlate": "ABC123",
    "bookingDate": "2024-12-25",
    "bookingTime": "14:00",
    "status": "Pending"
  }
}
```

### Earn Reward Points
```json
POST /api/Reward/earn
{
  "userId": 1,
  "amountSpent": 150.00
}
```
Response: Returns created Reward with points earned (e.g., 15 points)

### Redeem Reward Item
```json
POST /api/RewardCatalog/redeem/3
{
  "userId": 1
}
```

### Update Booking Stage
```json
POST /api/BookingStages/UpdateStage
{
  "bookingId": 1,
  "stageName": "Exterior Wash",
  "status": "In Progress",
  "updatedByUserId": 2
}
```

### Register User
```json
POST /api/Auth/Register
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "SecurePass123",
  "phoneNumber": "1234567890",
  "roleId": 1
}
```

### Login User
```json
POST /api/Auth/Login
{
  "email": "john@example.com",
  "password": "SecurePass123"
}
```
Response:
```json
{
  "id": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "phoneNumber": "1234567890",
  "roleId": 1
}
```

