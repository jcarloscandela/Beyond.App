# Beyond Todo Application

A modular todo list application built with .NET that provides both a console interface and a web-based interface with API backend.

## Project Structure

- `Beyond.Todo.API` - REST API backend
- `Beyond.Todo.Application` - Application logic and CQRS implementation
- `Beyond.Todo.Console` - Console-based todo list interface
- `Beyond.Todo.Domain` - Domain entities and business logic
- `Beyond.Todo.Infrastructure` - Data persistence and infrastructure concerns
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

### Option 2: Web Application with API (Frontend and Backend)

To run the full web application, you'll need to start both the API (backend) and the Blazor web app (frontend) separately:

1. Start the API (Backend):
```bash
cd Beyond.Todo.API
dotnet run
```
The API will be available at `https://localhost:7026`

2. In a new terminal, start the web application (Frontend):
```bash
cd Beyond.Todo.WebApp/Beyond.Todo.WebApp
dotnet run
```
The web application will be accessible at `https://localhost:5173`

The frontend is configured to communicate with the backend API through the settings in `wwwroot/appsettings.json`. You can modify the backend URL by updating the `ApiSettings.BaseUrl` value in this file. By default, it points to `http://localhost:5175`, but you can change it if your API is running on a different port or host.

Example `appsettings.json` configuration:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5175"
  }
}
```

## Features

- Create, read, update, and delete todo items
- Categorize todos
- Track progression of todo items with history
- Web interface with modern UI
- Console interface for command-line operations
- Toast notifications for important actions
- Comprehensive error handling
- User-friendly error messages

## Project Architecture

- Clean Architecture implementation
- CQRS pattern for command and query separation
- Repository pattern for data access
- Unit tests for application logic
- Middleware for consistent error handling

## Roadmap

The following features are currently in development:

1. **Real-time Updates**: Integration of SignalR for real-time notifications when todo items are created, updated, or deleted.
2. **Database Implementation**: Adding persistent storage with a proper database solution.

## Testing

The application includes a comprehensive test suite covering:
- Command handlers (Add, Update, Remove, Register Progression)
- Query handlers (Get Todos, Get Categories, Get Todo by Id)
- Business logic and domain rules
