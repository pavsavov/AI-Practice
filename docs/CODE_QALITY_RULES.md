## Development Workflow

## Coding Standards

### Tech Stack
- .NET 10
- Minimal API
- Use a simple JSON file as a db, and use EF CORE to query it.
- Use .NET MAUI for a Desktop UI client.

### C# Standards
- Naming Conventions: PascalCase for public, camelCase for private
- Code Structure: Keep methods under 50 lines, classes focused
- Comments: XML documentation for public APIs
- Security: Input validation, secure coding practices
- Performance: Avoid allocations in hot paths
- Clean Code: Apply SOLID principles and clean code practices
- Performance: Optimize for performance and scalability
- Security: Implement secure coding practices

### Logging Standards
- Preferred Framework: Use Micrsofoft.Extension.Logging for all logging implementations
- Structured Logging: Always use structured logging with named parameters
- Performance: Use structured parameters instead of string interpolation


### Architecture Pattern
- Monolith
- DI
- MVVM for Client

### Tech 

### Testing Patterns
- AAA Pattern: Arrange, Act, Assert
- Mocking: Use NSubstitute for dependencies
- Test Data: Create meaningful test data via AutoFixture
- Test Project Organization: Always place unit tests in dedicated test projects, never within the main application projects

## Implementation Guidelines

### Test Project Organization
- Dedicated Test Projects: Always create unit tests in separate test projects (e.g., `ProjectName.UnitTests`)
- Project References: Test projects should reference the main application projects, not the other way around
- Namespace Convention: Use the same namespace as the main project with `.UnitTests` suffix
- File Structure: Organize test files to mirror the main project structure
- Never Mix: Never place test files within the main application project folders

### Unit Testing
csharp
[Test]
public async Task AddItemToCart_ValidItem_ShouldAddItem()
{
    // Arrange
    var cartId = Guid.NewGuid();
    var itemId = Guid.NewGuid();
    var quantity = 2;
    
    // Act
    var result = await _cartService.AddItemAsync(cartId, itemId, quantity);
    
    // Assert
    result.Should().BeTrue();
    _cartRepository.Verify(x => x.SaveAsync(It.IsAny<Cart>()), Times.Once);
}


### Quality Review
- [ ] Code follows SonarCloud standards
- [ ] Methods are focused and under 50 lines
- [ ] Classes have single responsibility
- [ ] Proper error handling implemented
- [ ] Logging uses Serilog with structured parameters

### Security Review
- [ ] Input validation implemented
- [ ] No hardcoded secrets or credentials
- [ ] SQL injection prevention
- [ ] Authentication and authorization checks
- [ ] Secure data handling


### Documentation Review
- [ ] XML documentation for public APIs
- [ ] README updated if needed
- [ ] Code comments explain complex logic
- [ ] Architecture decisions documented
- [ ] API documentation updated

## Best Practices


### Code Implementation
- Start Simple: Begin with simple implementation, then optimize
- Test-Driven: Write tests before implementation
- Incremental: Make small, incremental changes
- Review Early: Get feedback early and often
- Refactor Regularly: Keep code clean and maintainable


### Testing
- Comprehensive Coverage: Aim for high test coverage
- Meaningful Tests: Write tests that catch real bugs
- Fast Tests: Keep unit tests fast and isolated
- Maintainable Tests: Write tests that are easy to maintain
- Dedicated Test Projects: Always place unit tests in separate test projects, never within main application projects

### Documentation
- Keep Updated: Maintain documentation with code changes
- Clear Examples: Provide clear code examples
- Architecture Decisions: Document important decisions
- API Documentation: Keep API docs current
- Troubleshooting: Include common issues and solutions

## Quick Reference

### Common Commands
- Implement Feature: "Implement the [feature] following our patterns"
- Write Tests: "Write unit tests for the [class/method]"
- Code Review: "Review this code for quality and security"
- Refactor: "Refactor this code to follow our standards"
- SonarCloud Analysis: "Analyze this code with SonarQube MCP server"
- Quality Gate Check: "Validate code against SonarCloud quality gates"
- Security Review: "Check for security vulnerabilities and hotspots"

### Code Templates
- Service Class: Use dependency injection with Serilog logging
- Controller: Follow RESTful patterns and error handling
- Repository: Implement proper data access patterns
- Event Handler: Use MassTransit for event processing

## Object Mapping & AutoMapper Usage
- Do NOT add or modify AutoMapper profiles for simple or one-to-one mappings
- Do NOT introduce AutoMapper by default
- Do NOT upgrade AutoMapper to version 15+
- Prefer explicit manual mappings for simple cases, including:
  - DTO ↔ Entity
  - Command / Query ↔ Domain model
  - API request / response models
- AutoMapper may be used only when:
  - Mapping logic is complex or conditional
  - Nested or polymorphic mappings are required
  - The same mapping is reused in multiple places
- Any new AutoMapper usage must be explicitly justified by complexity or reuse
- Avoid "mapping sprawl" — excessive profiles that obscure data flow and increase cognitive complexity
- Preferred alternatives for simple mappings:
  - Constructors
  - Factory methods
  - Extension methods