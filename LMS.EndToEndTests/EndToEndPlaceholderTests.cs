namespace LMS.EndToEndTests
{
    public class EndToEndPlaceholderTests
    {
        [Fact]
        public void Test1()
        {
            string hello = "Hello";
            string world = "World";

            var result = $"{hello} {world}!";

            Assert.Equal("Hello World!", result);
            Assert.NotEqual("Hello World", result);
            Assert.Contains("World", result);            
        }
    }
}
