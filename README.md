
# Library Management System API

This project is a  Library Management System (LMS) built with ASP.NET Core and Entity Framework Core. It provides a complete backend API and MVC web interface for managing library operations, including user authentication, book inventory, book categories, book copies, and book issuing/returning. The system supports both regular users and admin roles, with secure JWT-based authentication and role-based authorization.


## Features
- User authentication (Sign Up, Login) with JWT
- Book, BookCopy, BookCategory, and IssuedBook management
- Role-based authorization (User/Admin)
- Pagination in list APIs
- Book due date extension (up to three times)
- Custom validation and error handling


The APIs are designed to:
- Allow users to sign up, log in, and manage their accounts securely
- Enable admins to manage books, categories, and users
- Handle book inventory, including multiple copies and their availability
- Support issuing and returning books, with due dates and fine calculation
- Provide clear, consistent responses using a standard `ApiResponse<T>` wrapper for all endpoints

Additional Features:
- Pagination is implemented in list APIs for efficient data retrieval
- Users can extend the due date of an issued book up to three times (with validation)

All business logic is encapsulated in service classes, ensuring controllers remain clean and focused on request handling. The project follows best practices for validation, error handling, and separation of concerns.

## Project Structure
- `Controller/` - API and MVC controllers
- `Services/` - Business logic and data access
- `Models/Entities/` - Entity Framework Core models
- `Models/Dtos/` - Data transfer objects (DTOs) for API and UI
- `helper/` - Shared helpers (AutoMapper, JWT, etc.)
- `Attributes/` - Custom validation attributes
- `MiddleWare/` - Error handling middleware
- `wwwroot/` - Static files (CSS, JS, images)
## Conventions
- All business logic is implemented in `Services/` classes, not controllers
- Use DTOs for all API input/output
- All API responses are wrapped in `ApiResponse<T>` for consistency
- No duplicate folders or redundant code
- Follow separation of concerns and SOLID principles


## Book APIs

### 1. Add Book
**Endpoint:** `POST /lmsApi/book/addBook`
Allows admins to add a new book by providing details such as title, author, category, and other details. Ensures no duplicate books by title and author.

### 2. Update Book
**Endpoint:** `POST /lmsApi/book/updateBook?bookId={id}`
Enables admins to update book details (title, author, publisher, image, etc.) for an existing book.

### 3. Get All Books
**Endpoint:** `GET /lmsApi/book/getAllBooks?pageNumber={page}`
Retrieves a paginated list of all books in the system, including their details and availability.


## Book Copy APIs

### 1. Add Book Copy
**Endpoint:** `POST /lmsApi/book-copies/addBookCopy`
Allows admins to add a new physical copy of a book to the inventory, linked to a specific book.

### 2. Get Book Copies By Book Id
**Endpoint:** `GET /lmsApi/book-copies/getBookCopiesByBookId?bookId={id}`
Retrieves all copies of a specific book, including their availability and status.

### 3. Update Book Copy
**Endpoint:** `PUT /lmsApi/book-copies/updateBookCopy/{id}`
Allows updating the status (active/available) or details of a specific book copy.

### 4. Delete Book Copy
**Endpoint:** `DELETE /lmsApi/book-copies/deleteBookCopy/{id}`
Removes a book copy from the inventory (soft delete or deactivate).


## Issued Book APIs

### 1. Issue Book
**Endpoint:** `POST /lmsApi/issued-books/issue`
Allows a user to issue a book copy, specifying the copy and due date. Checks availability and user eligibility.

### 2. Submit Book (Return)
**Endpoint:** `POST /lmsApi/issued-books/submit`
Allows a user to return an issued book copy. If the book is overdue and a fine is due, the submission will not be successful until the fine is paid.

### 3. Get Issued Books To User
**Endpoint:** `POST /lmsApi/issued-books/getIssuedBooks?userId={userId}`
Retrieves a list of all books currently issued to a specific user.

### 4. Extend Book Submission Date
**Endpoint:** `POST /lmsApi/issued-books/extendSubmissionDate/{issueId}?extendedDate={date}`
Allows a user to extend the due date for a book they have issued, up to three times, if eligible.


### 1. Create Book Category
**Endpoint:** `POST /lmsApi/book-categories/createBookCategory`
Allows admins to create a new book category by providing a name, description, and optional image. Ensures category names are unique.


### 2. Get All Categories
**Endpoint:** `GET /lmsApi/book-categories/getAllCategories`
Retrieves a paginated list of all book categories in the system, including their details and associated books.


### 3. Get Category By Id
**Endpoint:** `GET /lmsApi/book-categories/getCategoryById/{id}`
Fetches detailed information for a specific category, including its books and metadata. Returns an error if the category does not exist.


### 4. Update Book Category
**Endpoint:** `PUT /lmsApi/book-categories/updateCategory/{id}`
Allows admins to update the name, description, or image of an existing category. Ensures the new name is unique among all categories.


### 5. Delete Book Category
**Endpoint:** `DELETE /lmsApi/book-categories/deleteCategory/{id}`
Removes a category from the system. If the category is associated with books, it may require additional handling or validation and here i implemented the soft delete.


### 6. Get Active Categories
**Endpoint:** `GET /lmsApi/book-categories/getActiveCategories`
Retrieves a list of all active (enabled) categories for use in dropdowns or selection lists.


### 1. User Registration (Sign Up)
**Endpoint:** `POST /auth/register`
Allows new users to create an account by providing their name, email, password, and phone number. On successful registration, the user is automatically logged in and receives an access token for authentication.


### 2. User Login
**Endpoint:** `POST /auth/login`
Authenticates existing users using their email and password. If the credentials are valid, the user receives an access token and can access protected resources.


### 3. Update User Profile
**Endpoint:** `PUT /user/updateUser/{id}`
Enables authenticated users to update their profile information, such as name, email, and phone number. The system ensures email uniqueness and updates the user's access token if needed.


### 4. Refresh Token
**Endpoint:** `POST /auth/refresh-token`
Allows users to obtain a new access token using a valid refresh token, without needing to log in again. This helps maintain secure sessions and improves user experience.

### Error Handling
All user APIs return clear error messages on failure. If a request fails, only the `errors` property is set in the response, following the project convention.



