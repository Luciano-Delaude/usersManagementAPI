# usersManagementAPI
User management CRUD API. 
The code project is under the "master" branch

Getting Started

This API project provides functionalities for user management, including listing active users, creating new users, updating user states (active/inactive), and deleting users.

Interactive Testing (Swagger)

For quick and easy testing of API endpoints, you can use the provided Swagger documentation:

    Access Swagger at: http://www.usermanagementapi.somee.com/swagger/index.html
    Explore each endpoint:
        ListAllActiveUsers: Click "Try it out" and then "Execute" to retrieve a list of active users.
        CreateNewUser: Click "Try it out", generate a username and birthdate in the format "YYYY-MM-DDTHH:MM:SS.000Z" (e.g., "1998-11-12T00:00:00.000Z"), and click "Execute" to create a new user.
        UpdateUserState:
            Get a valid user ID from the "ListAllActiveUsers" response.
            Click "Try it out", enter the ID in the placeholder, and choose "true" or "false" for the desired user state.
            Click "Execute" to update the user's state.
        DeleteUser:
            Obtain a valid user ID from the "ListAllActiveUsers" response.
            Click "Try it out", enter the ID in the placeholder.
            Click "Execute" to delete the user.

Running Unit Tests (Visual Studio 2022)

For a more comprehensive verification of the API's behavior, you can run unit tests within Visual Studio 2022:

    Open the project solution in Visual Studio 2022.
    Right-click on the "userManagementTests" project.
    Select "Execute Tests" to run the entire test suite.

These tests ensure that the API functions as intended under various scenarios.
