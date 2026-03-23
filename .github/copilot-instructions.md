# DDD Systems & .NET Guidelines
You are an AI assistant specialized in DRY code, SOLID principles, and .NET good practices for software Development. Follow these guidelines for building robust, maintainable systems.

## C# Standards
- Use PascalCase for class names, method names, and properties.
- Use camelCase for local variables and method parameters.
- use _camelCase for private fields, and prefix them with an underscore.
- use nullable reference types; annotate `string?` where null is expected.
- use String.Empty instead of "" for empty strings.
- Put every class, enum, interface, struct, and record into its own file, named for the object it contains.
- Wrap async event handlers in `try/catch` and log errors with `Console.WriteLine` or an injected `ILogger<T>`.
- Use DataAnnotations to configure entities. Never use EF Fluent API for configuration. If you need to configure something that can't be done with DataAnnotations, ask for an alternative approach.
- When annotating entities, at a minimum consider:
	- [Required], [MaxLength], and [MinLength] for string properties
	- [Required] for non-nullable reference types. 
	- [ForeignKey] for navigation properties
	- [Range] for numeric properties
	- [EncryptedField] for sensitive data
- Public methods should have XML documentation comments.
- Initialize new objects via new () instead of new ClassName() when the type can be inferred.
- Code should be commented, but comments should explain why something is done, not what is being done. The code itself should be clear enough to explain what is being done.

## Compiling
- Treat all warnings as errors. Do not ignore or suppress any compiler warnings. If you encounter a warning, address it immediately by fixing the underlying issue in the code.

## Project Guidelines
- Tables in markdown files must be spaced nicely for text viewers. The | column dividers must all line up vertically, maintaining proper alignment and readability.

## Configuration Management
- Do not pull values straight from IConfiguration. Instead, use the AppSettings class, set up the appropriate environment variable override in AppSettings.cs, add a default value into appsettings.json, add a good testing value into appsettings.development.json, and update README.md with the new setting in all appropriate places.
- Use EnvironmentMap for environment variable mapping in AppSettings instead of adding separate methods or logic for mapping.