# Beyond Todo Application

A modular todo list application built with .NET that provides both a console interface and a web-based interface with API backend.

## Project Structure

- `Beyond.Todo.API` - REST API backend
- `Beyond.Todo.Application` - Application logic and CQRS implementation
- `Beyond.Todo.Console` - Console-based todo list interface
- `Beyond.Todo.Domain` - Domain entities and business logic
- `Beyond.Todo.Infrastructure` - Data persistence and infrastructure concerns
- `Beyond.Todo.SignalR` - Real-time notification infrastructure
- `Beyond.Todo.WebApp` - Blazor web application

## Running the Application

There are two ways to run this application:

### Option 1: Console Application Only

To run just the console application:

```bash
cd Beyond.Todo.Console
dotnet run
```

This will start the standalone console-based todo list application.

### Option 2: Web Application with API

To run the full web application, you'll need to start both the API and the Blazor web app:

1. Start the API:
```bash
cd Beyond.Todo.API
dotnet run
```

2. In a new terminal, start the web application:
```bash
cd Beyond.Todo.WebApp/Beyond.Todo.WebApp
dotnet run
```

The API will be available at `https://localhost:7072` and the web application will be accessible at `https://localhost:7186`.

## Features

- Create, read, update, and delete todo items
- Categorize todos
- Track progression of todo items
- Web interface with modern UI
- Real-time notifications for todo updates using SignalR
- Console interface for command-line operations

## Project Architecture

- Clean Architecture implementation
- CQRS pattern for command and query separation
- Repository pattern for data access
- Unit tests for application logic
- Real-time updates using SignalR for:
  - Task creation and deletion
  - Task updates
  - Progression updates
