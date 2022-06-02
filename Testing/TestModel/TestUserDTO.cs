using Shared.DTOs;
using Xunit;


namespace TestModel
{
    public class TestUserDTO
    {
        [Fact]
        public void Test()
        {
            var model = new UserDTO
            {
                Id = "abc",
                UserName = "abc",
                PhoneNumber = "abc",
                Email = "abc",
                BirthDay = "abc",
                IsConfirmed = true,
                IsLockedOut = true,
                IsAvailable = true,
                FullName = "abc",
                Role = "abc"
            };
            Assert.True(model.IsAvailable);
            Assert.True(model.IsAvailable);
            Assert.True(model.IsAvailable);
            Assert.Equal("abc", model.Id);
            Assert.Equal("abc", model.UserName);
            Assert.Equal("abc", model.PhoneNumber);
            Assert.Equal("abc", model.Email);
            Assert.Equal("abc", model.BirthDay);
            Assert.Equal("abc", model.FullName);
            Assert.Equal("abc", model.Role);

        }
    }
}
