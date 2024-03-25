using MyIdLibrary.Models;

namespace TestProject1.Test_MyIdLibrary;

[TestClass]
public class UnitTest_SecretModel
{
    [TestMethod]
    public void Test_NewObject()
    {
        SecretModel secretModel = new SecretModel();
        Assert.IsTrue(Guid.TryParse(secretModel.Id.ToString(), out var id));
        Assert.IsNull(secretModel.Synced);
        Assert.IsTrue(Math.Abs((DateTime.UtcNow - secretModel.Created).TotalSeconds) < 1);
        Assert.IsTrue(Math.Abs((DateTime.UtcNow - secretModel.Modified).TotalSeconds) < 1);
    }
}
