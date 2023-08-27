Login Service

Introduction

This project serves as a local development environment for the BackendDevProject Repository. It's designed to facilitate easier testing of SQL and SMTP integrations. Built using ASP.NET Core 7.0, the service provides functionalities for user login, registration, and password management. It leverages Microsoft's Identity framework for secure user management and NLog for robust logging capabilities.

Technologies

ASP.NET Core 7.0
Microsoft Identity
Entity Framework Core 7.0.10
NLog 5.2.3
SMTP for Email Services
Bootstrap 5.3.1

Setup

To get started with this project:
Clone the repository
git clone https://github.com/JonathanHoward86/LoginService.git
Open the project using Visual Studio or any other compatible IDE.
Run the application.

Features

User Login
User Registration
Password Reset
Username Retrieval
Logging with NLog

Architecture

Controllers
LoginController: Manages user login functionalities.
PasswordController: Responsible for password-related features.
RegistrationController: Manages user registration.

Models
ForgotUsernameModel: Used for forgotten username requests.
LoginModel: Used for login requests.
RegisterModel: Used for registration requests.
ResetPasswordModel: Used for password reset requests.
ResetPasswordConfirmModel: Used for confirming password resets.

Services
EmailService: Implements IEmailService and is responsible for sending emails.

Configuration
Startup.cs: Configures services and middleware.
Program.cs: Serves as the entry point for the application, initializing it and setting up logging.

Razor Views
Contains Razor views for login, registration, password reset, and username retrieval.

Configuration Files
appsettings.json: Contains general settings including logging and SMTP configurations.
appsettings.Development.json: Contains development-specific settings, including the database connection string.

Acknowledgments

The code in this repository was created with assistance from Chat GPT by Jonathan Howard, an aspiring software developer with expertise in Agile Software Development and proficiency in various programming languages.