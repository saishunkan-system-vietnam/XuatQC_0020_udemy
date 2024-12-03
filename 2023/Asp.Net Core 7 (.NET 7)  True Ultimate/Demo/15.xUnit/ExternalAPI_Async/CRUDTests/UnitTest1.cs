namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestAddMath()
        {
            // Arrange
            int inputA = 15; int inputB = 20;
            int expected = inputA + inputB;

            // Act
            MyMath mm = new MyMath();
            int actual = mm.Add(inputA, inputB);

            // Assert
            Assert.Equal(expected, actual);

        }
    }
}