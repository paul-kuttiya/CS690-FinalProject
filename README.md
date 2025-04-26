# GiftTrackerApp
Wiki: [wiki](https://github.com/paul-kuttiya/CS690-FinalProject/wiki)  
User Documentation: [user documentation](https://github.com/paul-kuttiya/CS690-FinalProject/wiki/User-Documentation)
Developer Documentation: [developer documentation](https://github.com/paul-kuttiya/CS690-FinalProject/wiki/Developer-documentation)
Deployment Documentation: [deployment documentation](https://github.com/paul-kuttiya/CS690-FinalProject/wiki/Deployment-documentation)

## Project Structure

```
GiftTrackerApp
├── Data
│   └── DataPlaceholder
├── GiftTrackerApp.csproj
├── Helpers
│   └── ConsoleHelper.cs
├── Models
│   └── GiftIdea.cs
├── Program.cs
├── Repositories
│   └── GiftIdeaRepository.cs
└── Services
    └── GiftIdeaService.cs
GiftTrackerApp.Tests
└──...test files
```

## Data

`Data/` holds a `DataPlaceholder` file and will store individual `<username>.txt` files containing gift records.

## Helpers

### ConsoleHelper.cs

Centralizes console input methods (`ReadString`, `ReadInt`, `ReadFloat`) and a `Pause` function to simplify user prompts.

## Models

### GiftIdea.cs

Defines the `GiftIdea` data model with fields for ID, timestamp, title, description, notes, recipient, and price.

## Repositories

### GiftIdeaRepository.cs

Manages file I/O: loading all ideas, saving lists, and appending new entries.

## Services

### GiftIdeaService.cs

Offers core operations: add, update, delete, search, view, and summarize gift ideas, while delegating persistence to the repository.

## Application Entry Point

### Program.cs

Provides the user interface loop: user selection, menu display, and invoking actions between steps.

## Tests

Unit tests can be added in `GiftTrackerApp.Tests` to cover models, helpers, repository, and service logic.
