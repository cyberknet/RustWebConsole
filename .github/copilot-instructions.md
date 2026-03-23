# Copilot Instructions

## Project Guidelines
- The user prefers tables in markdown files to be spaced nicely for text viewers, maintaining proper alignment and readability.

## Configuration Management
- Do NOT pull values straight from IConfiguration. Instead, use the AppSettings class, set up the appropriate environment variable override in AppSettings.cs, add a default value into appsettings.json, add a good testing value into appsettings.development.json, and update README.md with the new setting in all appropriate places.
- Use EnvironmentMap for environment variable mapping in AppSettings instead of adding separate methods or logic for mapping.