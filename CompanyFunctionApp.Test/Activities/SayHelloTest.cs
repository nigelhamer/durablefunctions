namespace CompanyFunctionApp.Test.Activities
{
    using CompanyFunctionApp.Activities;

    using Xunit;

    public class SayHelloTest
    {
        [Fact]
        public void SayHello_returns_greeting()
        {
            var result = SayHello.Run("John");
            Assert.Equal("Hello John!", result);
        }
    }
}
