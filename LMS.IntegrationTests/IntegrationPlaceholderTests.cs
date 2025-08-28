namespace LMS.IntegrationTests
{
    public class IntegrationPlaceholderTests
    {
        [Fact]
        public void Test1()
        {
            int a = 1;
            int b = 2;

            var result = a + b;

            Assert.Equal(3, result);

        }
    }
}
