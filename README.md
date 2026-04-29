# Library Management System — ASP.NET Core 10 Web API

A Clean Architecture Library Management System built with ASP.NET Core 10 and Entity Framework Core (Code-First).

---
```## How to Run

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Steps

```bash
# 1. Restore all NuGet packages
dotnet restore

# 2. Run the API (EF migrations apply automatically on startup)
dotnet run --project src/LibraryManagement.API
```


## Project Structure

```
  LibraryManagement.Domain          ← Entities, Interfaces, Exceptions  (Class Library)
  LibraryManagement.Application     ← DTOs, Service Interfaces, Services (Class Library)
  LibraryManagement.Infrastructure  ← EF Core, DbContext, Repositories  (Class Library)
  LibraryManagement.API             ← Controllers, Middleware, Program   (Web API)
  LibraryManagement.Tests           ← xUnit tests with EF InMemory DB
```

**Dependency flow:**
```
API → Application → Domain
API → Infrastructure → Domain
Tests → Application + Infrastructure + Domain
```


```

Swagger UI opens at: **https://localhost:7140/swagger/index.html**

### Running Migrations Manually (optional)

```bash
dotnet ef migrations add InitialCreate \
  --<project src>/LibraryManagement.Infrastructure \
  --startup-project src/LibraryManagement.API

dotnet ef database update \
  --<project src>/LibraryManagement.Infrastructure \
  --startup-project src/LibraryManagement.API
```

---

## API Endpoints

### Authors
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/authors | List all authors |
| GET | /api/authors/{id} | Get author with their books |
| POST | /api/authors | Create a new author |
| PUT | /api/authors/{id} | Update an author |
| DELETE | /api/authors/{id} | Delete an author |

### Books
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/books | List all books |
| GET | /api/books/{id} | Get a book |
| POST | /api/books | Create a book (assign to an author) |
| PUT | /api/books/{id} | Update a book |
| DELETE | /api/books/{id} | Delete a book |
| POST | /api/books/{id}/borrow | Borrow a book — marks it unavailable |
| POST | /api/books/{id}/return | Return a book — marks it available |

### Borrowers
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/borrowers | List all borrowers |
| GET | /api/borrowers/{id} | Get a borrower |
| GET | /api/borrowers/{id}/history | Get full borrowing history |
| POST | /api/borrowers | Create a borrower |
| PUT | /api/borrowers/{id} | Update a borrower |
| DELETE | /api/borrowers/{id} | Delete a borrower |

---

## Running Tests

```power shell
dotnet test
```

Tests included:
- **AuthorModelTests** — verifies Author properties are set correctly (required by task)
- **AddBookTests** — verifies book count increases after CreateBook using EF InMemory DB (required by task)


---

### 1. Author → Books (One-to-Many) ✅

```
Author (1) ──────────────── (Many) Book
```

- One **Author** can write **many Books**.
- Each **Book** belongs to **exactly one Author**.
- If you try to delete an Author who has Books, it is blocked (`DeleteBehavior.Restrict`).

**How it is implemented in EF Core:**
```csharp
// Author entity
public List<Book> Books { get; set; }   // navigation: one author → many books

// Book entity
public int    AuthorId { get; set; }    // foreign key
public Author Author   { get; set; }    // navigation: each book → one author
```
---

### 2. Borrower → BorrowRecords (One-to-Many) ✅

```
Borrower (1) ──────────────── (Many) BorrowRecord
```

- One **Borrower** can have **many BorrowRecords** (borrows many books over time).
- Each **BorrowRecord** belongs to **exactly one Borrower**.

**How it is implemented in EF Core:**
```csharp
// Borrower entity
public List<BorrowRecord> BorrowRecords { get; set; }  // one borrower → many records

// BorrowRecord entity
public int      BorrowerId { get; set; }    // foreign key
public Borrower Borrower   { get; set; }    // navigation: each record → one borrower
```

---

### 3. Book → BorrowRecords (One-to-Many) ✅

```
Book (1) ──────────────── (Many) BorrowRecord
```

- One **Book** can have **many BorrowRecords** (borrowed and returned multiple times over its lifetime).
- Each **BorrowRecord** belongs to **exactly one Book**.

**How it is implemented in EF Core:**
```csharp
// Book entity
public List<BorrowRecord> BorrowRecords { get; set; }  // one book → many records

// BorrowRecord entity
public int  BookId { get; set; }    // foreign key
public Book Book   { get; set; }    // navigation: each record → one book
```

**Real-world example:**
> Dune (Book) was borrowed by → John (Jan 2024), Sarah (Mar 2024), Mike (Jun 2024)

---

### 4. Book ↔ Borrower via BorrowRecord (Many-to-Many) ✅

```
Book (Many) ──── BorrowRecord ──── (Many) Borrower
```

- **Many Borrowers** can borrow **many Books** over time.
- The `BorrowRecord` table is the **join table** that connects them.
- It is not a plain join table — it carries extra data: `BorrowedAt` and `ReturnedAt`.

**Why BorrowRecord instead of a direct many-to-many?**

A direct many-to-many has no extra columns. We need to store *when* the book was borrowed
and *when* it was returned, so we use an explicit join entity (`BorrowRecord`) instead.

```csharp
public class BorrowRecord
{
    public int       BookId     { get; set; }   // FK to Book
    public int       BorrowerId { get; set; }   // FK to Borrower
    public DateTime  BorrowedAt { get; set; }   // extra data ← why we need the join entity
    public DateTime? ReturnedAt { get; set; }   // extra data ← null means still borrowed
}
```

---

### Entity Relationship Diagram

```
┌─────────────┐         ┌─────────────┐
│   Author    │  1    * │    Book     │
│─────────────│─────────│─────────────│
│ Id          │         │ Id          │
│ FirstName   │         │ Title       │
│ LastName    │         │ ISBN        │
│ Biography*  │         │ PublicYear  │
│ DateOfBirth*│         │ Genre*      │
└─────────────┘         │ IsAvailable │
                        │ AuthorId FK │
                        └──────┬──────┘
                               │ 1
                               │
                               │ *
                        ┌──────┴──────┐         ┌─────────────┐
                        │BorrowRecord │  *    1  │  Borrower   │
                        │─────────────│──────────│─────────────│
                        │ Id          │          │ Id          │
                        │ BookId FK   │          │ Name        │
                        │ BorrowerId FK          │ Email       │
                        │ BorrowedAt  │          │ PhoneNumber*│
                        │ ReturnedAt* │          └─────────────┘
                        └─────────────┘

* = optional field (nullable)
```

---

## Optional Fields
The task requires **at least one optional field**. This application has several:

| Entity | Optional Field | Reason |
|--------|---------------|--------|
| Author | `Biography` | Not every author has a bio on file |
| Author | `DateOfBirth` | Date of birth may not be known |
| Book | `Genre` | Some books do not fit a single genre |
| Borrower | `PhoneNumber` | Phone number is not always required |
| BorrowRecord | `ReturnedAt` | Null while the book is still borrowed |

---



## Sample Requests

### Create an Author
```json
POST /api/authors
{
  "firstName": "Frank",
  "lastName": "Herbert",
  "biography": "American science fiction author.",
  "dateOfBirth": "1920-10-08"
}
```