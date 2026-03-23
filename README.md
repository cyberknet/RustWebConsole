# Rust WebRCON Multi-Tenant System

## Overview
This project is a multi-tenant, web-based remote console (WebRCON) system for managing Rust game servers. It
allows multiple users to monitor and manage one or more Rust servers through a web interface.

## Features
- WebSocket-based RCON client for Rust servers
- Multi-tenant architecture
- Real-time updates using SignalR
- Blazor Server frontend
- Dockerized PostgreSQL database

## Getting Started
1. Clone the repository.
2. Build the solution using Visual Studio.
3. Run the application using Docker Compose.

## Prerequisites
- .NET 10
- Docker and Docker Compose
- Visual Studio 2026 or later

## Running the Application
1. Ensure Docker is installed and running.
2. Set the required environment variables for your deployment. You can use the following table as a reference:

   | Environment Variable Name                   | Description                                                               |
   |---------------------------------------------|---------------------------------------------------------------------------|
   | RWC_IDENTITY_PASSWORDREQUIREDIGIT           | Whether passwords must contain a digit (true/false).                      |
   | RWC_IDENTITY_PASSWORDREQUIREDLENGTH         | Minimum length for passwords.                                             |
   | RWC_IDENTITY_PASSWORDREQUIRENONALPHANUMERIC | Whether passwords must contain a non-alphanumeric character (true/false). |
   | RWC_IDENTITY_PASSWORDREQUIREUPPERCASE       | Whether passwords must contain an uppercase letter (true/false).          |
   | RWC_IDENTITY_PASSWORDREQUIRELOWERCASE       | Whether passwords must contain a lowercase letter (true/false).           |
   | RWC_IDENTITY_REQUIRECONFIRMEDACCOUNT        | Whether users must confirm their accounts (true/false).                   |
   | RWC_JWT_VALIDISSUER                         | The issuer of the JWT tokens (e.g., https://yourdomain.com).              |
   | RWC_JWT_VALIDAUDIENCE                       | The audience for the JWT tokens (e.g., https://yourdomain.com).           |
   | RWC_JWT_ISSUERSIGNINGKEY                    | A secret key used to sign the JWT tokens.                                 |
   | RWC_CONNECTIONS_DEFAULT                     | The connection string for the default database.                           |
   | RWC_DATAPROTECTION_KEYPATH                  | Path to the key repository for Data Protection API.                       |

   Alternatively, you can map a configuration file to appsettings.json in your deployment environment. Below is an example file to get you started:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=yourserver;Database=yourdb;User Id=youruser;Password=yourpassword;"
     },
     "Identity": {
       "PasswordRequireDigit": true,
       "PasswordRequiredLength": 8,
       "PasswordRequireNonAlphanumeric": true,
       "PasswordRequireUppercase": true,
       "PasswordRequireLowercase": true,
       "RequireConfirmedAccount": true
     },
     "Jwt": {
       "ValidIssuer": "https://yourdomain.com",
       "ValidAudience": "https://yourdomain.com",
       "IssuerSigningKey": "YourSuperSecretKey"
     },
     "DataProtection": {
       "KeyPath": "/app/keys"
     }
   }
   ```
   See [`appsettings.json configuration`](#appsettingsjson) on what each setting does.

   You can mount this file into your Docker container or place it in the appropriate directory for your hosting environment.
3. Run `docker-compose up` in the project root.
4. Access the application at `http://localhost:5000`.

## Configuration

### appsettings.json
The `appsettings.json` file contains configuration values for the application. Below are the key sections and their expected values:

#### Jwt
- **ValidIssuer**: The issuer of the JWT tokens (e.g., `https://yourdomain.com`).
- **ValidAudience**: The audience for the JWT tokens (e.g., `https://yourdomain.com`).
- **IssuerSigningKey**: A secret key used to sign the JWT tokens. This should be a long, random string.

#### Identity
- **PasswordRequireDigit**: Whether passwords must contain a digit.
- **PasswordRequiredLength**: Minimum length for passwords.
- **PasswordRequireNonAlphanumeric**: Whether passwords must contain a non-alphanumeric character.
- **PasswordRequireUppercase**: Whether passwords must contain an uppercase letter.
- **PasswordRequireLowercase**: Whether passwords must contain a lowercase letter.
- **RequireConfirmedAccount**: Whether users must confirm their accounts.

#### DataProtection
- **KeyPath**: Path to the key repository for Data Protection API. This should be a directory that the application has read/write access to.

### Environment Variable Overrides
For deployments using Docker, you can override the `appsettings.json` values using environment variables. The following environment variables are supported:

| AppSettings Key                        | Environment Variable Name                   |
|----------------------------------------|---------------------------------------------|
| Identity:PasswordRequireDigit          | RWC_IDENTITY_PASSWORDREQUIREDIGIT           |
| Identity:PasswordRequiredLength        | RWC_IDENTITY_PASSWORDREQUIREDLENGTH         |
| Identity:PasswordRequireNonAlphanumeric| RWC_IDENTITY_PASSWORDREQUIRENONALPHANUMERIC |
| Identity:PasswordRequireUppercase      | RWC_IDENTITY_PASSWORDREQUIREUPPERCASE       |
| Identity:PasswordRequireLowercase      | RWC_IDENTITY_PASSWORDREQUIRELOWERCASE       |
| Identity:RequireConfirmedAccount       | RWC_IDENTITY_REQUIRECONFIRMEDACCOUNT        |
| Jwt:ValidIssuer                        | RWC_JWT_VALIDISSUER                         |
| Jwt:ValidAudience                      | RWC_JWT_VALIDAUDIENCE                       |
| Jwt:IssuerSigningKey                   | RWC_JWT_ISSUERSIGNINGKEY                    |
| ConnectionStrings:DefaultConnection    | RWC_CONNECTIONS_DEFAULT                     |
| DataProtection:KeyPath                 | RWC_DATAPROTECTION_KEYPATH                  |

These environment variables are mapped in the `AppSettings.EnvironmentMap` dictionary in `AppSettings.cs`. Ensure these variables are set in your Docker environment to override the default values.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the GNU Affero General Public License (AGPL). A summary of your rights and responsibilities is below.

You are free to use, modify, and distribute the software, provided that:

1. Any changes you make are also shared under the AGPL.
2. If you use this software to provide a service over a network, you must make the complete source code available to the users of that service.
3. The software is provided "as-is" without any warranty.

For complete license terms, see the [full AGPL license text](LICENSE).