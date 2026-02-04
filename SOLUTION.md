# Solution Documentation

**Candidate Name:** [Your Name]  
**Completion Date:** [Date]

---

## Problems Identified

_Describe the issues you found in the original implementation. Consider aspects like:_
- Architecture and design patterns
- Code quality and maintainability
- Security vulnerabilities
- Performance concerns
- Testing gaps

Tight coupling and poor DI:
Controllers create TodoService instances directly rather than receiving services via dependency injection.
SQL and security problems:
SQL is built with string interpolation (SQL injection risk).
No parameterized queries or prepared statements.
Synchronous/blocking I/O:
Database operations are synchronous. Modern ASP.NET apps should use async DB calls to avoid blocking thread pool.
Poor separation of concerns:
Program.cs handles DB initialization inline. Consider moving DB/schema responsibilities to a repository/initializer.
Controller API design:
All endpoints use POST and custom route names instead of RESTful verbs and routes (POST/GET/PUT/DELETE + /api/todos).
Request DTOs are defined inside controller file; better to use separate DTO classes and model validation attributes.
Error handling and logging:
Very generic exception handling returning ex.Message — leaks implementation details; no structured logging.
Tests are brittle and integration-style:
Unit tests use the real TodoService and real DB file; tests are not isolated or repeatable.
Tests rely on state in the shared file-based DB (no setup/teardown or in-memory alternative).
Data access layer:
No repository/ORM abstraction. Manually using low-level SQLite APIs increases chance of bugs and repeated code.

---

## Architectural Decisions

_Explain the architecture you chose and why. Consider:_
- Design patterns applied
- Project structure changes
- Technology choices
- Separation of concerns

Add interfaces and DTOs (done: ITodoService)
Add ITodoService, add ITodoRepository interface for data access.
Add request/response DTOs in a separate folder (Dtos/).
Add typed options for DB connection string in appsettings.

---

## Trade-offs

_Discuss compromises you made and the reasoning behind them. Consider:_
- What did you prioritize?
- What did you defer or simplify?
- What alternatives did you consider?

[Your trade-offs here]

---

## How to Run

### Prerequisites
[List required software, versions, etc.]

### Build
```bash
# Add your build commands
```

### Run
```bash
# Add your run commands
```

### Test
```bash
# Add your test commands
```

---

## API Documentation

### Endpoints

#### Create TODO
```
Method: [HTTP method]
URL: [endpoint]
Request Body: [example]
Response: [example]
```

#### Get TODO(s)
```
Method: [HTTP method]
URL: [endpoint]
Request: [example]
Response: [example]
```

#### Update TODO
```
Method: [HTTP method]
URL: [endpoint]
Request Body: [example]
Response: [example]
```

#### Delete TODO
```
Method: [HTTP method]
URL: [endpoint]
Request: [example]
Response: [example]
```

---

## Future Improvements

_What would you do if you had more time? Consider:_
- Additional features
- Performance optimizations
- Enhanced testing
- Better documentation
- Deployment considerations

[Your ideas here]
