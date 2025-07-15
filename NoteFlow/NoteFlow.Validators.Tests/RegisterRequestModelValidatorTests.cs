namespace NoteFlow.Validators.Tests
{
    using API.Validators.User;
    using Application.Models.User.Requests;
    using FluentValidation.TestHelper;

    public class RegisterRequestModelValidatorTests
    {
        private readonly RegisterRequestModelValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            // Arrange
            var model = this.GetValidModel();
            model.Username = string.Empty;

            // Act
            var result = this._validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Short()
        {
            // Arrange
            var model = this.GetValidModel();
            model.Username = "abc";

            // Act
            var result = this._validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Long()
        {
            // Arrange
            var model = this.GetValidModel();
            model.Username = new string('a', 21);

            // Act
            var result = this._validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Username_Has_Special_Characters()
        {
            // Arrange
            var model = this.GetValidModel();
            model.Username = "abc@123";

            // Act
            var result = this._validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        private RegisterRequestModel GetValidModel()
        {
            return new RegisterRequestModel { Username = "ValidUsername", Password = "ValidPass1!", Email = "user@centric.eu" };
        }
    }
}
