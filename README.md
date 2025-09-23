# PetGram 2.0

PetGram 2.0 is a robust social network API built with C#/.NET, representing a significant evolution over one of my earliest software projects. This new version aims to dive deeper into software architecture patterns, implementing Domain-Driven Design (DDD) with Clean Architecture, CQRS, and several advanced features not present in the original. 

## Features

On PetGram, users can:
1. Register their pets and upload photos (with an external Python service that predicts the species of the registered animal).
2. Create posts.
3. Interact with other users and posts (likes, comments, etc.).
4. Follow other users (with a dynamic feed based on who you follow).
5. Send real-time messages to other users.
6. Receive real-time notifications based on their actions.
7. Report other users.
8. Create "moments" (temporary posts similar to stories).
9. Admins can ban (for a chosen time) and regulate users on the platform.
10. ...And much more! (:

## Tech Stack and Architecture

- **.NET API**: The project is built on .NET, focusing on maintainability, scalability, and clean code practices.
- **SignalR**: Enables real-time WebSocket communication for messaging and notifications.
- **Redis**: Used for caching, performance, and storing temporary data such as OTPs.
- **Hangfire**: Handles background jobs, such as notifying inactive users or managing banned user status.
- **Python ML Service**: Utilizes a machine learning model (trained on the TensorFlow cats vs. dogs dataset) to predict the pet species from uploaded photos.
- **Docker**: Used for containerizing the environment (databases, Redis, etc.).
- **PostgreSQL + EF Core**: Manages persistent data storage with Entity Framework Core.

## Design Patterns

- **Factory + Strategy**: For creating notifications without polluting the service layer.
- **CQRS**: Commands and queries are separated, with a mediator layer for clean separation of concerns (even though only one database is used).
- **Result Pattern**: Standardizes HTTP responses.

## Motivation

While this architecture introduces additional complexity that might seem unnecessary for a relatively simple project, it provided a great opportunity to learn and apply concepts that are invaluable for larger-scale systems—and to have some fun along the way!

---

## Getting Started

Coming soon: setup instructions, API docs, and usage examples.
