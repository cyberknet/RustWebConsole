# Rust WebRCON Multi-Tenant System - Development Plan

## 🎉 Major Discovery: You Already Have a Working RCON Library!

Your workspace contains **RustRcon** - a WebSocket RCON client for Rust servers that provides a solid foundation! This includes:
- ✅ Full WebSocket connection management
- ✅ Basic Rust message parsing (players, console, chat, bans, plugins, server info)
- ✅ Event-driven architecture
- ✅ Already targeting .NET 10
- ⚠️ **Work in Progress** - Not feature-complete, will need extensions for advanced features
- ✅ **Still saves 1-2 weeks of WebSocket/connection boilerplate!**

## Project Overview
A multi-tenant, web-based remote console (WebRCON) system for Rust game servers that allows multiple users to manage and monitor one or more Rust servers through a web interface. The system features separated data collection and frontend layers for scalability and reliability.

## Architecture Overview (Simplified for Personal Use)
- **Single Application**: ASP.NET Core Web API + Blazor Server (one container)
- **Background Services**: Hosted services within the main application for RCON polling
- **RCON Client**: Your existing RustRcon library
- **Persistence Layer**: Dockerized PostgreSQL database with persistent volumes
- **Caching**: In-memory caching (no Redis needed for small scale)
- **Database Strategy**: Code-First Entity Framework with automatic migration on application startup
- **Deployment**: Simple docker-compose with 2 containers (app + database)

---

Key:
[C] = Complete
[I] = In Progress
[D] = Deferred Until Later
[ ] = Not Started

## Phase 1: Foundation & Infrastructure

### 1. Project Setup and Architecture Planning
- [C] **Already have**: RustRcon project (WebSocket RCON client!)
- [C] Add project reference from RustWebConsole.Web to RustRcon
- [C] Add necessary NuGet packages to RustWebConsole.Web:
  - **Npgsql.EntityFrameworkCore.PostgreSQL** - PostgreSQL provider for EF Core
  - **Serilog.AspNetCore** - Structured logging
  - **Serilog.Sinks.File** - File logging sink
  - **Microsoft.AspNetCore.Identity.EntityFrameworkCore** - Identity with EF Core
  - **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT auth
  - **Polly** - Retry and resilience policies for RCON connections
  - **MudBlazor** - Blazor UI components
  - **Swashbuckle.AspNetCore** - Swagger/OpenAPI (built-in with .NET templates)
- [C] Configure Serilog for file-based logging
- [C] Set up configuration management (appsettings.json, user secrets)
- [C] Create .gitignore and README.md
- [C] Create Dockerfile for the application
- [C] Create simple docker-compose.yml (app + PostgreSQL only)
- [C] Set up volume mount for PostgreSQL data

**Key Libraries:**
- ✅ **RustRcon** - YOUR EXISTING WebSocket RCON library (saves 2-3 weeks!)
- ✅ **Websocket.Client** - Already included in RustRcon project
- ✅ **Polly** - Production-ready retry/resilience patterns
- ✅ **MudBlazor** - Beautiful Blazor components out of the box
- ✅ **Serilog** - Best-in-class logging for .NET

### 2. Database Design and Setup (Code-First EF Core)
- [C] 2.1 Design Code-First Entity Framework models for simplified architecture
  - User entity (authentication, profile)
  - Server entity (connection details, encrypted credentials)
  - UserServer access control entity (simple many-to-many)
  - ServerStatistics entity
  - Player entity
  - PlayerStatistics entity
  - ConsoleMessage entity (with retention policy)
  - RconRequest entity (audit trail of all RCON commands sent)
  - RconResponse entity (responses from RCON commands)
  - UserAction entity (audit log of user actions in the system)
- [C] 2.2 Create DbContext with all DbSet properties
- [C] 2.3 Configure entity relationships using DataAnnotations, never Fluent API
- [D] 2.4 Add indexes for frequently queried fields
- [ ] 2.5 Implement automatic migration runner on application startup using **EF Core Migrations**
- [ ] 2.6 Add database seeding for initial data (admin user, sample server)
- [ ] 2.7 Add database health check using **Microsoft.Extensions.Diagnostics.HealthChecks** (built-in)

**Libraries to use:**
- **Npgsql.EntityFrameworkCore.PostgreSQL** - PostgreSQL provider
- **Microsoft.EntityFrameworkCore.Tools** - Migration commands
- **Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore** - Database health checks

### 3. Core Domain Models (Code-First Entities)
- 3.1 [ ] Create User entity with Identity integration and related DTOs
- 3.2 [ ] Create Server entity with encrypted credentials and related DTOs
- 3.3 [ ] Create UserServer relationship entity (which users can access which servers)
- 3.4 [ ] Create Player entity and related DTOs
- 3.5 [ ] Create ConsoleMessage entity and related DTOs
- 3.6 [ ] Create ServerStatistics entity and related DTOs
- 3.7 [ ] Create PlayerInventory models
- 3.8 [ ] Create RconRequest entity (timestamp, user, server, command, status)
- 3.9 [ ] Create RconResponse entity (request reference, response data, timestamp, status)
- 3.10 [ ] Create UserAction entity (user, action type, target, timestamp, details)
- 3.11 [ ] Create shared enums (ServerStatus, PlayerStatus, PermissionLevel, ActionType, etc.)
- 3.12 [ ] Add data annotations for validation (built-in System.ComponentModel.DataAnnotations)

**Libraries to use:**
- **AutoMapper** (optional but recommended) - DTO mapping to reduce boilerplate
- **FluentValidation** (optional) - More sophisticated validation than data annotations

**Consider adding:**
- AutoMapper can save a lot of manual DTO mapping code
- FluentValidation provides better validation rules than attributes

---

## Phase 2: Authentication & Authorization

### 4. Identity and Authentication System
**USE EXISTING LIBRARIES**: ASP.NET Core Identity + JWT
- 4.1 [ ] Install **Microsoft.AspNetCore.Identity.EntityFrameworkCore** (NuGet)
- 4.2 [ ] Configure ASP.NET Core Identity with EF Core
- 4.3 [ ] Install **Microsoft.AspNetCore.Authentication.JwtBearer** for API auth
- 4.4 [ ] Create user registration endpoint
- 4.5 [ ] Create user login endpoint with JWT generation
- 4.6 [ ] Implement refresh token mechanism
- 4.7 [ ] Add password reset functionality using Identity
- 4.8 [ ] Configure password policies (Identity built-in)

**Libraries to use:**
- **ASP.NET Core Identity** - Complete user management system
- **System.IdentityModel.Tokens.Jwt** - JWT token generation/validation
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT authentication

### 5. Simplified Authorization
- 5.1 [ ] Implement simple role-based authorization (Admin, User, Viewer)
- 5.2 [ ] Create authorization handlers for server access control
- 5.3 [ ] Implement server-level access checks (user can only see their assigned servers)
- 5.4 [ ] Add simple permission checking middleware

### 6. User Management
- 6.1 [ ] Create user profile endpoints (GET, PUT)
- 6.2 [ ] Create simple user management for admins (add user, assign servers)
- 6.3 [ ] Implement user-server access assignment endpoints
- 6.4 [ ] Add basic user activity logging

---

## Phase 3: WebSocket RCON Connection Infrastructure

### 7. WebSocket RCON Implementation
**🎯 EXTEND YOUR EXISTING LIBRARY**: Use and enhance **RustRcon** 
- 7.1 [ ] Reference **RustRcon** project (already in solution)
- 7.2 [ ] Review existing **RustWebRconClient** class and understand what's implemented
- 7.3 [ ] Use existing entities and events where available
- 7.4 [ ] **Identify missing features** needed for your requirements:
  - 7.4.1 [ ] Player inventory retrieval and manipulation
  - 7.4.2 [ ] Player teleportation commands
  - 7.4.3 [ ] Advanced server statistics
  - 7.4.4 [ ] Any other missing RCON commands
- 7.5 [ ] **Extend RustWebRconClient or create wrapper** for missing functionality
- 7.6 [ ] Add new parsers for commands not yet supported
- 7.7 [ ] Add retry logic using **Polly** around connection attempts
- 7.8 [ ] Log all RCON requests to RconRequest entity
- 7.9 [ ] Log all RCON responses to RconResponse entity
- 7.10 [ ] Implement request/response correlation tracking

**Your existing library provides (as a foundation):**
- ✅ WebSocket connection management (Websocket.Client)
- ✅ Basic message routing and parsing
- ✅ Core entities: Player, ServerInfo, ConsoleMessage, ChatMessage, Ban, Plugins
- ✅ Event-driven architecture
- ⚠️ **Missing features** you'll need to add:
  - Inventory management commands
  - Teleportation commands
  - Advanced statistics parsing
  - Custom RCON commands specific to your needs

**Libraries already in use:**
- **Websocket.Client** (v4.4.43) - WebSocket client with reactive extensions
- **System.Text.Json** - JSON serialization

**Approach: Build on top of what exists, extend where needed!**

### 8. Server Management
- 8.1 Create Server CRUD API endpoints
- 8.2 Implement server connection validation
- 8.3 Add server credential encryption at rest using **ASP.NET Core Data Protection API** (built-in)
- 8.4 Create server status monitoring service
- 8.5 Implement server connection testing endpoint
- 8.6 Add server grouping/tagging functionality
- 8.7 Log all server configuration changes to UserAction entity
- 8.8 Persist server information to Server entity

### 9. Background Data Collection Service (Hosted Service)
- 9.1 Create IHostedService for continuous data collection within main app (built-in)
- 9.2 Implement server polling mechanism using **BackgroundService** (built-in)
- 9.3 Use **PeriodicTimer** (built-in .NET) for scheduled tasks
- 9.4 Add server status collection job
- 9.5 Add player list collection job
- 9.6 Add console message collection job
- 9.7 Implement rate limiting using **AspNetCoreRateLimit** (optional) or simple semaphore
- 9.8 Add configurable polling intervals per server in appsettings

### 10. Console Message Streaming
- 10.1 Create endpoint to retrieve console messages (paginated)
- 10.2 Implement real-time console message streaming using SignalR
- 10.3 Add console message filtering (by level, keyword, timeframe)
- 10.4 Implement message retention policy and cleanup job
- 10.5 Add ability to send console commands
- 10.6 Create command history tracking
- 10.7 Log all console commands sent as RconRequest entities
- 10.8 Persist console messages to ConsoleMessage entity
- 10.9 Log user action when sending console commands

### 11. Plugin Management
- 11.1 Implement plugin list retrieval from server
- 11.2 Create API endpoint to get plugin information
- 11.3 Add plugin status monitoring
- 11.4 Create plugin reload/restart functionality
- 11.5 Implement plugin configuration viewing (if accessible)

### 12. Server Statistics
- 12.1 Collect and store FPS, memory usage, player count
- 12.2 Create API endpoints for current server statistics
- 12.3 Store basic historical statistics
- 12.4 Add simple statistics visualization data endpoints
- 12.5 Create basic threshold alerts (optional)

### 13. Player Management
- 13.1 Implement player list retrieval and display
- 13.2 Create player search and filtering
- 13.3 Add player detail view with statistics
- 13.4 Implement player kick functionality
- 13.5 Implement player ban/unban functionality
- 13.6 Add player mute/unmute functionality
- 13.7 Create player note/comment system
- 13.8 Log all player management actions to UserAction entity
- 13.9 Log all RCON commands for player management
- 13.10 Persist player data to Player entity

### 14. Player Statistics
- 14.1 Collect basic play time, kills, deaths
- 14.2 Create player statistics API endpoints
- 14.3 Add simple player comparison features
- 14.4 Create basic player activity timeline

### 15. Player Inventory Management
- 15.1 **Research Rust RCON inventory commands** (may not be available via vanilla RCON)
- 15.2 Implement player inventory retrieval (if available via RCON or plugin)
- 15.3 **Extend RustRcon** with inventory parsers if needed
- 15.4 Create inventory viewing endpoint
- 15.5 Add item addition functionality (via RCON commands)
- 15.6 Add item removal functionality
- 15.7 Implement item modification (quantity, condition)
- 15.8 Add inventory clearing functionality
- 15.9 Create preset inventory templates
- 15.10 Implement inventory action logging
- 15.11 Log all inventory manipulation actions to UserAction entity
- 15.12 Log all inventory RCON requests and responses
- 15.13 Persist inventory snapshots if needed

### 16. Player Teleportation
- 16.1 **Research Rust RCON teleport commands** (likely requires plugin)
- 16.2 Implement get player position (if available)
- 16.3 Create teleport player to coordinates command
- 16.4 **Extend RustRcon** if teleport commands need custom parsing
- 16.5 Add teleport player to another player
- 16.6 Implement teleport player to named locations
- 16.7 Add teleport history tracking
- 16.8 Create favorite locations system
- 16.9 Log all teleport actions to UserAction entity
- 16.10 Log all teleport RCON requests and responses

### 17. SignalR Hub Implementation
- 17.1 Set up SignalR hub infrastructure
- 17.2 Create authentication for SignalR connections
- 17.3 Implement server-specific channels/groups
- 17.4 Add console message broadcasting
- 17.5 Add player list updates broadcasting
- 17.6 Add server statistics broadcasting
- 17.7 Implement connection management and cleanup

### 18. Real-Time Notifications
- 18.1 Create simple notification system for important events
- 18.2 Add player connect/disconnect notifications via SignalR
- 18.3 Add server status change notifications
- 18.4 Create basic in-app notification display

### 19. Frontend Project Setup (Blazor Server)
- 19.1 Set up Blazor Server within the same solution
- 19.2 Install **MudBlazor** NuGet package (beautiful Material Design components)
- 19.3 Configure MudBlazor in Program.cs
- 19.4 Configure routing
- 19.5 Create shared layout components using MudBlazor
- 19.6 Set up SignalR for real-time updates (built-in with Blazor Server)
- 19.7 Create simple state management using Blazor cascading parameters or **Fluxor** (optional Flux/Redux for Blazor)

### 20. Authentication UI
- 20.1 Create login page
- 20.2 Create registration page
- 20.3 Implement password reset flow
- 20.4 Add authentication guards for routes
- 20.5 Create logout functionality
- 20.6 Add session timeout handling

### 21. Dashboard and Navigation
- 21.1 Create main dashboard layout using **MudLayout** components
- 21.2 Implement responsive navigation menu with **MudDrawer** and **MudAppBar**
- 21.3 Add server selector component using **MudSelect**
- 21.4 Create quick statistics widgets using **MudCard** and **MudChart**
- 21.5 Add recent activity feed using **MudTimeline** or **MudList**
- 21.6 Implement user profile menu using **MudMenu**

### 22. Server Management UI
- 22.1 Create server list view
- 22.2 Implement add/edit server forms
- 22.3 Add server connection status indicators
- 22.4 Create server detail view
- 22.5 Implement server switching functionality
- 22.6 Add server favorites/pinning

### 23. Console View
- 23.1 Create console message display component using **MudList** or custom component
- 23.2 Implement auto-scrolling with scroll-lock
- 23.3 Add console command input using **MudTextField** with Enter key handling
- 23.4 Implement command history (up/down arrows) - store in list and navigate
- 23.5 Add message filtering UI using **MudSelect** and **MudTextField**
- 23.6 Create console search functionality with **MudTextField**
- 23.7 Add color coding for message types using MudBlazor color utilities

### 24. Player Management UI
- 24.1 Create player list view with **MudTable** (built-in search/filter/sort)
- 24.2 Implement player detail modal using **MudDialog**
- 24.3 Add player action menu with **MudMenu** (kick, ban, teleport)
- 24.4 Create player statistics visualization with **MudChart**
- 24.5 Implement player comparison UI with **MudDataGrid**
- 24.6 Add player notes interface using **MudTextField** multi-line

### 25. Inventory Management UI
- 25.1 Create inventory grid using **MudGrid** or **MudDataGrid**
- 25.2 Implement item search and filter with **MudTextField** and **MudSelect**
- 25.3 Add item addition modal using **MudDialog** and **MudForm**
- 25.4 Create item edit/delete functionality with **MudIconButton** actions
- 25.5 Implement drag-and-drop support (custom or use Blazor component libraries)
- 25.6 Add inventory templates UI with **MudList** and **MudExpansionPanel**

### 26. Server Statistics UI
- 26.1 Create real-time statistics dashboard with **MudCard** grid
- 26.2 Implement charts for historical data using **MudChart** (line, bar, pie charts)
- 26.3 Add customizable metric widgets using **MudCard** with **MudCardActions**
- 26.4 Create alert configuration UI with **MudForm**
- 26.5 Implement export functionality (CSV) - can use **CsvHelper** library

### 27. Administration UI (Simplified)
- 27.1 Create simple user management interface (add users, assign to servers)
- 27.2 Add user-server access management
- 27.3 Create basic audit log viewer
- 27.4 Add simple system settings page

### 28. Command Scheduling (Optional)
- 28.1 Create scheduled command system
- 28.2 Implement simple CRON-like scheduling
- 28.3 Add scheduled command UI
- 28.4 Create command templates library

### 29. Basic Reporting (Optional)
- 29.1 Create server uptime reports
- 29.2 Implement player activity reports
- 29.3 Add CSV export functionality

### 30. Backup Commands (Optional)
- 30.1 Add manual database backup endpoint
- 30.2 Create simple configuration export/import

### 31. API Documentation
- 31.1 Set up Swagger/OpenAPI documentation (built-in)
- 31.2 Document key endpoints with XML comments

### 32. Simple Caching Strategy
- 32.1 Use IMemoryCache for frequently accessed data (player lists, statistics)
- 32.2 Add cache invalidation logic
- 32.3 Configure cache expiration policies in appsettings

### 33. Basic Performance Optimization
- 33.1 Optimize database queries (add indexes where needed)
- 33.2 Implement pagination for large datasets
- 33.3 Optimize SignalR message frequency
- 33.4 Use async/await properly throughout

### 34. Logging and Health Checks
- 34.1 Configure Serilog to write to files
- 34.2 Add structured logging for important events
- 34.3 Create health check endpoints (database, RCON connections)
- 34.4 Add simple error logging

### 35. Security Hardening
- 35.1 Implement basic rate limiting on API endpoints
- 35.2 Add CORS configuration
- 35.3 Add input validation and sanitization
- 35.4 Implement security headers
- 35.5 Add SQL injection prevention (EF Core handles this)
- 35.6 Use user secrets for development credentials
- 35.7 Use environment variables for production secrets

### 36. Basic Testing
- 36.1 Create unit tests for core business logic
- 36.2 Test RCON client functionality
- 36.3 Test authentication flows
- 36.4 Add integration tests for key endpoints
- 36.5 Aim for >60% code coverage of critical paths

### 37. Manual Testing
- 37.1 Test with real Rust server
- 37.2 Test all RCON commands
- 37.3 Test multi-user access
- 37.4 Test real-time updates

### 38. Simple Docker Setup
- 38.1 Create optimized Dockerfile for the application (multi-stage build)
- 38.2 Create production docker-compose.yml
- 38.3 Configure environment variables for production
- 38.4 Set up PostgreSQL with volume mount
- 38.5 Add health checks to docker-compose
- 38.6 Create .dockerignore file
- 38.7 Test full Docker deployment locally

### 39. Deployment
- 39.1 Choose simple hosting (home server, VPS, or cloud VM)
- 39.2 Set up SSL with Let's Encrypt (optional reverse proxy)
- 39.3 Configure PostgreSQL backups (pg_dump cron job)
- 39.4 Deploy using docker-compose
- 39.5 Set up automatic restarts (Docker restart policies)

### 40. Basic Documentation
- 40.1 Write README with setup instructions
- 40.2 Document how to run with Docker
- 40.3 Document environment variables needed
- 40.4 Create simple user guide for friends
- 40.5 Document RCON commands available

### 41. Initial Launch
- 41.1 Deploy to your environment
- 41.2 Add yourself as admin user
- 41.3 Connect first Rust server
- 41.4 Verify all core features work

### 42. Friend Testing
- 42.1 Add friend users
- 42.2 Assign them to servers
- 42.3 Gather feedback
- 42.4 Fix critical bugs

### 43. Ongoing Maintenance
- 43.1 Monitor logs occasionally
- 43.2 Keep dependencies updated
- 43.3 Add features as needed
- 43.4 Regular database backups

---

## Technology Stack (Simplified)

### Backend
- **Framework**: ASP.NET Core 10 Web API + Blazor Server (single application)
- **Real-time**: SignalR (built-in with Blazor Server)
- **Database**: PostgreSQL (Dockerized)
- **ORM**: Entity Framework Core (Code-First with auto-migration)
- **Caching**: IMemoryCache (in-memory, no Redis needed)
- **Authentication**: ASP.NET Core Identity with JWT (for API) + Cookie auth (for Blazor)
- **Logging**: Serilog with file sink
- **Background Jobs**: IHostedService / BackgroundService (built-in)
- **Data Protection**: ASP.NET Core Data Protection API for credential encryption

### Frontend
- **Framework**: Blazor Server (integrated with API in same app)
- **UI Library**: MudBlazor (great for Blazor, easy to use)
- **Charts**: MudBlazor charts or simple Chart.js integration
- **Real-time**: Built-in SignalR with Blazor Server

### Infrastructure & DevOps
- **Containerization**: Docker
- **Deployment**: Simple docker-compose (2 containers: app + PostgreSQL)
- **Reverse Proxy**: Optional nginx for SSL (or use built-in Kestrel with SSL)
- **Monitoring**: File-based logging + health check endpoints
- **Database Backups**: Simple pg_dump cron job or manual exports

### What We're NOT Using (Overkill for Personal Project)
- ❌ Redis - use IMemoryCache instead
- ❌ ELK Stack - use Serilog to files
- ❌ Prometheus/Grafana - use simple health checks
- ❌ Kubernetes - use docker-compose
- ❌ Separate microservices - one app with hosted services
- ❌ Complex multi-tenant architecture - simple user roles
- ❌ Hangfire/Quartz.NET - use BackgroundService
- ❌ Container registry - build locally or use Docker Hub if needed

### Database Persistence Strategy
- **Code-First Approach**: All entities defined in C# with EF Core
- **Auto-Migration**: Database automatically migrates on application startup
- **Data Persistence**: 
  - Server configurations and credentials (encrypted)
  - User profiles and authentication data
  - User actions (audit trail)
  - RCON requests and responses
  - Console messages with retention policies
  - Player data and statistics
  - Server statistics
- **Volume Mount**: PostgreSQL data directory mounted to host for persistence
- **Backup Strategy**: Manual pg_dump or simple scheduled backup script

---

## Estimated Timeline (Simplified)
- **Phase 1-2**: 1-2 weeks (Foundation, Database, Auth)
- **Phase 3-4**: 2-3 weeks (RCON & Core Features)
- **Phase 5**: 1 week (Real-time with SignalR)
- **Phase 6**: 2-3 weeks (Blazor Frontend)
- **Phase 7**: Optional - add later if needed
- **Phase 8-9**: 1 week (Performance & Testing)
- **Phase 10-11**: 1 week (Docker setup & Deploy)

**Total Estimated Time**: 8-12 weeks (2-3 months) for core functionality

This is much more achievable for a personal project!

---

## Success Metrics (Personal Project Scale)
- Support for 5-10 concurrent users (you and friends)
- Response time <500ms for API calls (good enough for personal use)
- Real-time updates within 1-2 seconds
- Reliable uptime when running
- Support for 5-10 concurrent server connections
- Works well, doesn't need to be perfect

## Key Design Principles (Simplified for Personal Use)

### Simple Docker Architecture
- **2 containers**: Application + PostgreSQL (that's it!)
- **docker-compose**: Single command to start everything
- **Persistent volumes**: Database survives container restarts
- **Health checks**: Basic checks to ensure services are running
- **Simple networking**: Containers talk to each other, no complex routing

### Code-First Database Strategy
- **Entity Framework Core Code-First**: All schema in C# code
- **Automatic migrations on startup**: Database self-hydrates on first run
- **Version control**: Schema changes tracked in code
- **Simple seeding**: Create admin user and sample data automatically

### Essential Audit Trail
- **RconRequest entity**: Track RCON commands sent (who, when, what)
- **RconResponse entity**: Store server responses for debugging
- **UserAction entity**: Log important user actions
- **Keep it simple**: Log what's useful, don't go overboard

### Single Application Architecture
- **One ASP.NET Core app**: API + Blazor Server + Background Services all in one
- **IHostedService for polling**: Background worker runs in the same process
- **In-memory caching**: Use IMemoryCache, no need for Redis
- **File-based logging**: Serilog writes to files, easy to check
- **Blazor Server**: Built-in real-time with SignalR, no separate frontend needed

---

## Notes for Personal Project
- **KISS Principle**: Keep It Simple, Stupid - don't over-engineer
- **Start small**: Get core RCON features working first
- **Iterate**: Add features as you actually need them
- **Good enough is good enough**: Doesn't need to be production-grade enterprise software
- **Use built-in features**: .NET has most of what you need already
- **Leverage existing libraries**: Don't reinvent the wheel (see below)
- **Docker for convenience**: Easy to deploy anywhere, easy to backup
- **Skip the optional stuff**: Phase 7 features can wait until you actually want them
- **No need for scalability**: Optimize for simplicity, not for 1000 users
- **Database self-hydrates**: Just run `docker-compose up` and everything works

---

## 🔑 Key Third-Party Libraries Summary

### **CRITICAL - Already Built!**
1. ✅ **RustRcon** (YOUR EXISTING PROJECT!) - Rust WebRCON foundation
   - WebSocket connection management with Websocket.Client library
   - Basic Rust entities and parsers for core features
   - Event-driven architecture
   - Handles console messages, players, bans, plugins, server info
   - ⚠️ **Work in Progress** - Will need extensions for advanced features (inventory, teleport, etc.)
   - **Saves: 1-2 weeks of WebSocket/connection boilerplate!**

### **CRITICAL - Will Save You Weeks:**
2. **MudBlazor** - Complete Material Design UI component library
   - Tables, forms, dialogs, charts, navigation all built-in
   - Responsive and themeable
   - **Saves: 3-4 weeks of UI component development**

### **Highly Recommended:**
3. **Polly** - Resilience and retry policies
   - Circuit breaker, retry, timeout patterns
   - Industry standard for fault tolerance
   - **Saves: Several days of retry logic**

4. **Serilog** - Structured logging
   - File, console, and database sinks
   - Easy configuration
   - **Saves: Days of logging infrastructure**

5. **AutoMapper** (optional but helpful) - Object-to-object mapping
   - Maps entities to DTOs automatically
   - Reduces boilerplate code significantly
   - **Saves: Hours of repetitive mapping code**

### **Built-in .NET Features (No External Libraries Needed):**
- **ASP.NET Core Identity** - Complete user management
- **Entity Framework Core** - ORM and migrations
- **SignalR** - Real-time communication (built into Blazor Server)
- **IMemoryCache** - In-memory caching
- **BackgroundService** - Background workers
- **PeriodicTimer** - Scheduled tasks (.NET 6+)
- **Data Protection API** - Credential encryption
- **Health Checks** - Service health monitoring
- **JWT Bearer Auth** - API authentication
- **Swagger/OpenAPI** - API documentation (Swashbuckle)

### **Optional Libraries to Consider:**
- **FluentValidation** - More powerful than data annotations (if you need complex validation)
- **CsvHelper** - CSV export functionality
- **Blazored.LocalStorage** - Browser storage for user preferences
- **Fluxor** - Redux-style state management for Blazor (only if app gets complex)
- **AspNetCoreRateLimit** - API rate limiting (if needed)
- **Blazor.DragDrop** - Drag and drop for inventory UI (if you want that feature)

### **Libraries You DON'T Need (Keep It Simple):**
- ❌ Hangfire/Quartz.NET - Use BackgroundService instead
- ❌ MassTransit/RabbitMQ - No message bus needed
- ❌ MediatR - No CQRS needed for this scale
- ❌ StackExchange.Redis - Use IMemoryCache
- ❌ Dapper - EF Core is fine for this scale
- ❌ AutoFixture/Bogus - Not worth it for personal project
- ❌ NSwag - Swashbuckle is simpler

---

## 📦 Estimated NuGet Packages for Task #1

```bash
# Core Framework (included in templates)
Microsoft.AspNetCore.App (metapackage)

# Database
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design

# Authentication & Identity
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.Authentication.JwtBearer
System.IdentityModel.Tokens.Jwt

# WebSocket RCON - ALREADY HAVE IT!
# RustRcon (your project)
# Websocket.Client v4.4.43 (already referenced by RustRcon)

# Resilience
Polly

# Logging
Serilog.AspNetCore
Serilog.Sinks.File
Serilog.Sinks.Console

# UI Components - BIG TIME SAVER
MudBlazor

# Health Checks
Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore

# API Documentation (usually included)
Swashbuckle.AspNetCore

# Optional but Recommended
AutoMapper.Extensions.Microsoft.DependencyInjection

# Optional if needed later
CsvHelper (for exports)
FluentValidation.AspNetCore (if complex validation needed)
```

**Total Package Count: ~9-11 core packages**

**🎯 GOOD NEWS WITH REALISTIC EXPECTATIONS:** You have a working WebSocket RCON library as a foundation, which saves significant time on connection management and basic features. However, you'll need to extend it for advanced features like inventory management and teleportation (which likely require plugin support anyway).

**Your existing RustRcon project includes:**
- RustWebRconClient class
- Basic entities: Player, ServerInfo, ConsoleMessage, ChatMessage
- Ban, OxidePlugin, CarbonPlugin support
- Event handlers for core message types
- Connection management infrastructure

**You'll need to add:**
- Advanced command support (inventory, teleport)
- Custom parsers for new response types
- Plugin-specific command handling
- Enhanced statistics parsing

This is still a **huge time-saver** (1-2 weeks) and gives you a proven foundation to build on!

