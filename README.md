# Login Service

This is my local dev environment of the BackendDevProject Respository which I created to more easily test the SQL and SMTP integration.

Introduction
This project is a login service built using ASP.NET Core 7.0. It provides functionalities for user login, registration, and password management. The project uses Microsoft's Identity framework for user management and NLog for logging.

Technologies
ASP.NET Core 7.0
Microsoft Identity
Entity Framework Core 7.0.10
NLog 5.2.3
SMTP for Email Services
Bootstrap 5.3.1

Setup
To run this project, clone the repository and run it using Visual Studio or any other compatible IDE.
git clone https://github.com/JonathanHoward86/LoginService.git

Features
User Login
User Registration
Password Reset
Username Retrieval
Logging with NLog

Controllers
LoginController
Handles user login functionalities.

PasswordController
Handles password-related functionalities.

RegistrationController
Handles user registration.

Models
ForgotUsernameModel: For forgotten username requests.
LoginModel: For login requests.
RegisterModel: For registration requests.
ResetPasswordModel: For password reset requests.
ResetPasswordConfirmModel: For confirming password reset.

Services
EmailService
Implements IEmailService and handles sending emails.

Startup Configuration
Configures services and middleware in Startup.cs.

Program Entry Point
Initializes the application and configures logging in Program.cs.

Razor Views
Contains Razor views for login, registration, password reset, and username retrieval.

Configuration
appsettings.json: Contains logging and SMTP settings.
appsettings.Development.json: Contains development-specific settings, including the database connection string.

Acknowledgments

The code in this repository were created with a little help from Chat GPT by Jonathan Howard, an aspiring software developer with expertise in Agile Software Development and proficiency in various programming languages.
