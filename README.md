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
2. Run `docker-compose up` in the project root.
3. Access the application at `http://localhost:5000`.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the GNU Affero General Public License (AGPL). A summary of your rights and responsibilities is below.

You are free to use, modify, and distribute the software, provided that:

1. Any changes you make are also shared under the AGPL.
2. If you use this software to provide a service over a network, you must make the complete source code available to the users of that service.
3. The software is provided "as-is" without any warranty.

For complete license terms, see the [full AGPL license text](LICENSE).