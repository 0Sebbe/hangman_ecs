using NUnit.Framework;

[TestFixture]
public class GuessedLetterComponentTests
{
    [Test]
    public void TestLetterProperty()
    {
        // Arrange
        var component = new GuessedLetterComponent();

        // Act
        component.Letter = 'a';

        // Assert
        Assert.AreEqual('a', component.Letter);
    }
}