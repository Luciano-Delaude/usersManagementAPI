using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using usersManagementAPI.Models;
using usersManagementAPI.Services;
using usersManagementAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Castle.Components.DictionaryAdapter.Xml;

namespace userManagementTests
{
    public class listAllUsersTests
    {
        private readonly Mock<IUserService> _mockUserService;

        public listAllUsersTests()
        {
            _mockUserService = new Mock<IUserService>();
        }

        [Test]
        public async Task listAllActiveUsers_ReturnsEmptyList_WhenNoUsersExist()
        {
            _mockUserService.Setup(service => service.ListAllActiveUsers())
              .Returns(Task.FromResult(new List<User>()));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.ListAllActiveUsers();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var response = okResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = messageProp.GetValue(response) as string;
            Assert.That(messageValue, Is.EqualTo("There are no active users in the database"));

            var responseProp = response.GetType().GetProperty("response");
            Assert.NotNull(responseProp);

            var userList = (List<User>)responseProp.GetValue(response);
            Assert.NotNull(userList); 
            Assert.IsEmpty(userList); 
        }

        [Test]
        public async Task listAllActiveUsers_ReturnsUsersList_WhenIsActiveIsSet()
        {
            _mockUserService.Setup(service => service.ListAllActiveUsers())
              .Returns(Task.FromResult(new List<User> { new User { UserName = "Test User", IsActive = true} }));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.ListAllActiveUsers();

            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(statusResult);
            Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode); // Verify status code

            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Succesfully obtained the active users", messageValue);

            var responseProp = response.GetType().GetProperty("response");
            Assert.NotNull(responseProp);

            var userList = (List<User>)responseProp.GetValue(response);
            Assert.NotNull(userList);
            Assert.IsNotEmpty(userList);

            //Check if first user exists and is active
            var firstUser = userList[0];
            Assert.NotNull(firstUser);
            Assert.AreEqual(firstUser.UserName, "Test User");
            Assert.True(firstUser.IsActive);

        }

        [Test]
        public async Task listAllActiveUsers_InternalServerError()
        {
            _mockUserService.Setup(service => service.ListAllActiveUsers())
              .Throws<Exception>();

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.ListAllActiveUsers();

            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(statusResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusResult.StatusCode);

            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("An error occurred while retrieving active users", messageValue);

            var errorProp = response.GetType().GetProperty("error");
            Assert.NotNull(errorProp);

        }
    }

    public class createNewUserTests
    {
        private readonly Mock<IUserService> _mockUserService;

        public createNewUserTests()
        {
            _mockUserService = new Mock<IUserService>();
        }

        [Test]
        public async Task createNewUser_SuccesfullyCreatedUser()
        {
            string userName = "Test User";
            DateTime birthday = new DateTime(2000, 5, 1);

            _mockUserService.Setup(service => service.CreateUser(userName, birthday))
                .Returns(Task.FromResult(new User { UserName = userName, UserBirthdate = birthday }));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.CreateUser(new UserDto { UserName = userName, UserBirthdate = birthday });

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            //Check if the status code is correct (200)
            Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

            //Assert the response object contains the created user information.
            var response = statusResult.Value as object;
            Assert.NotNull(response);

            //Check user is not null
            var createdUser = statusResult.Value.GetType().GetProperty("response").GetValue(response) as User;
            Assert.NotNull(createdUser);

            //Verify data of created user
            Assert.AreEqual(userName, createdUser.UserName);
            Assert.AreEqual(birthday, createdUser.UserBirthdate);
        }

        [Test]
        public async Task createNewUser_InternalServerError()
        {
            string userName = "Test User";
            DateTime birthday = new DateTime(2000, 5, 1);

            _mockUserService.Setup(service => service.CreateUser(userName, birthday))
                .Throws<Exception>();

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.CreateUser(new UserDto { UserName = userName, UserBirthdate = birthday });

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusResult.StatusCode); // Verify status code

            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Internal error - Unable to create new user", messageValue);

            var errorProp = response.GetType().GetProperty("error");
            Assert.NotNull(errorProp);
        }
    }

    public class updateUserStateTests
    {
        private readonly Mock<IUserService> _mockUserService;

        public updateUserStateTests()
        {
            _mockUserService = new Mock<IUserService>();
        }

        [Test]
        public async Task updateUserState_SuccesfullyUpdatedUser()
        {
            int userId = 1;
            bool isActive = true;

            _mockUserService.Setup(service => service.UpdateUserState(userId, isActive))
                .Returns(Task.FromResult(true));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.UpdateUserState(userId,isActive);

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            //Check if the status code is correct (200)
            Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

            //Assert the response object contains the created user information.
            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Succesfully updated user status", messageValue);

        }

        [Test]
        public async Task updateUserState_UserNotFound()
        {
            int userId = 1;
            bool isActive = true;

            _mockUserService.Setup(service => service.UpdateUserState(userId, isActive))
                .Returns(Task.FromResult(false));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.UpdateUserState(userId, isActive);

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            //Check if the status code is correct (404)
            Assert.AreEqual(StatusCodes.Status404NotFound, statusResult.StatusCode);

            //Assert the response object contains the created user information.
            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("User not found", messageValue);
        }

        [Test]
        public async Task updateUserState_InternalServerError()
        {
            int userId = 1;
            bool isActive = true;

            _mockUserService.Setup(service => service.UpdateUserState(userId, isActive))
              .Throws<Exception>();

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.UpdateUserState(userId, isActive);

            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(statusResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusResult.StatusCode);

            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Internal server error", messageValue);

            var errorProp = response.GetType().GetProperty("error");
            Assert.NotNull(errorProp);

        }
    }

    public class deleteUserTests
    {
        private readonly Mock<IUserService> _mockUserService;

        public deleteUserTests()
        {
            _mockUserService = new Mock<IUserService>();
        }

        [Test]
        public async Task deleteUserTests_SuccesfullyDeletedUser()
        {
            int userId = 1;

            _mockUserService.Setup(service => service.DeleteUser(userId))
                .Returns(Task.FromResult(true));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.DeleteUser(userId);

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            //Check if the status code is correct (200)
            Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

            //Assert the response object contains the created user information.
            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Succesfully deleted user from database", messageValue);

        }

        [Test]
        public async Task deleteUserTests_UserNotFound()
        {
            int userId = 1;

            _mockUserService.Setup(service => service.DeleteUser(userId))
                .Returns(Task.FromResult(false));

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.DeleteUser(userId);

            //Assert the response is of expected type (objectResult)
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(result);

            //Check if the status code is correct (404)
            Assert.AreEqual(StatusCodes.Status404NotFound, statusResult.StatusCode);

            //Assert the response object contains the created user information.
            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("User not found", messageValue);
        }

        [Test]
        public async Task deleteUserTests_InternalServerError()
        {
            int userId = 1;

            _mockUserService.Setup(service => service.DeleteUser(userId))
              .Throws<Exception>();

            var controller = new UserController(_mockUserService.Object);
            var result = await controller.DeleteUser(userId);

            Assert.IsInstanceOf<ObjectResult>(result);
            var statusResult = result as ObjectResult;
            Assert.NotNull(statusResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusResult.StatusCode);

            var response = statusResult.Value as object;
            Assert.NotNull(response);

            var messageProp = response.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = (string)messageProp.GetValue(response);
            Assert.AreEqual("Internal server error", messageValue);

            var errorProp = response.GetType().GetProperty("error");
            Assert.NotNull(errorProp);

        }
    }
}