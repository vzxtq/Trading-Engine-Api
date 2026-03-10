# Trading Engine API

A backend simulation of a stock trading platform built with **ASP.NET Core**.
The project models a simplified financial exchange including **order books, order matching, trades, and portfolio management**.

This project focuses on **system design, domain modeling, and backend architecture** rather than UI. It demonstrates how a real-world trading system could be structured using **Clean Architecture** and **Domain-Driven Design (DDD)**.

---

# Overview

The Trading Engine API simulates the core components of a stock exchange:

* Users can place **buy and sell orders**
* Orders are stored in an **order book**
* The **matching engine** matches compatible buy and sell orders
* When orders match, the system generates a **trade**
* User **portfolios and balances** are updated accordingly

The goal of this project is to demonstrate:

* Backend architecture design
* High-level trading engine logic
* Domain modeling for financial systems
* Clean separation of concerns

---

# Architecture

The project follows **Clean Architecture** principles.

```
Clients (Web / Mobile)
        │
        ▼
     API Layer
        │
        ▼
 Application Layer
        │
        ▼
     Domain Layer
        │
        ▼
 Infrastructure Layer
        │
        ▼
     Database
```

### Layers

**API**

* ASP.NET Core Web API
* Controllers and request handling
* Authentication and request validation

**Application**

* Use cases
* Commands / Queries
* Business orchestration
* Interfaces for repositories and services

**Domain**

* Core business logic
* Entities and value objects
* Aggregates
* Domain services

**Infrastructure**

* Entity Framework Core
* Database access
* Repository implementations
* External integrations

---

# Core Components

## Order Book

The **order book** stores active buy and sell orders for each symbol.

Example:

```
BUY ORDERS

Price | Volume
101   | 5
100   | 10
99    | 3

SELL ORDERS

Price | Volume
102   | 2
103   | 8
```

Buy orders are sorted by **highest price first**.
Sell orders are sorted by **lowest price first**.

---

## Matching Engine

The **matching engine** is the core of the system.

It processes incoming orders and attempts to match them against existing orders.

Example:

```
User A: BUY 10 AAPL @ 100
User B: SELL 10 AAPL @ 100
```

Result:

```
Trade executed:
10 AAPL @ 100
```

If partial matches occur:

```
BUY 10 @ 100
SELL 5 @ 99
```

Result:

```
Trade: 5 @ 99
Remaining: BUY 5 @ 100
```

---

## Trades

When a match occurs, a **trade record** is created.

Example:

```
Trade
Symbol: AAPL
Price: 100
Quantity: 10
Timestamp: 2026-03-10
Buyer: UserA
Seller: UserB
```

---

## Portfolio Management

Each user has a portfolio containing:

```
Balance
Positions
```

Example:

```
User Balance: $10,000

Positions:
AAPL: 20
TSLA: 5
```

Trades update both **balance** and **positions**.

---

# Tech Stack

Backend

* ASP.NET Core Web API
* Entity Framework Core
* C#

Architecture

* Clean Architecture
* Domain-Driven Design (DDD)
* Repository Pattern

Infrastructure

* PostgreSQL
* Redis (optional caching)
* RabbitMQ (optional messaging)

Real-time

* SignalR (for live updates)

---

# Project Structure

```
src/

TradingPlatform.API
TradingPlatform.Application
TradingPlatform.Domain
TradingPlatform.Infrastructure
TradingPlatform.MatchingEngine
```

### Domain

```
Entities
ValueObjects
Aggregates
DomainEvents
Enums
```

### Application

```
Commands
Queries
DTOs
Interfaces
Handlers
```

### Infrastructure

```
Persistence
Repositories
EntityConfigurations
Migrations
```

---

# Database

Entity Framework Core is used for persistence.

Schema changes are managed via **migrations**.

Example commands:

Create migration:

```
dotnet ef migrations add InitialCreate
```

Apply migration:

```
dotnet ef database update
```

---

# Example API Endpoints

Create order

```
POST /orders
```

Get order book

```
GET /orderbook/{symbol}
```

Get portfolio

```
GET /portfolio
```

Get trade history

```
GET /trades
```

---

# Concurrency Challenges

Trading systems must handle many simultaneous orders.

Potential issues:

* race conditions
* double spending
* inconsistent balances

Typical solutions:

* single-threaded matching engine
* queue-based processing
* database transactions

---

# Future Improvements

Possible extensions for the project:

* Market orders
* Stop orders
* Candlestick charts
* Historical price data
* Real-time order book streaming
* Trading bots
* Margin trading
* Performance optimization

---

# Why This Project

This project demonstrates:

* backend system architecture
* domain modeling
* financial transaction logic
* real-time system design
* scalable backend concepts

It goes beyond simple CRUD APIs and models a **core component of financial infrastructure**.

---

# License

MIT License
