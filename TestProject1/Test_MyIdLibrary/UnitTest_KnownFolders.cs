using MyIdLibrary.Models;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;

namespace TestProject1.Test_MyIdLibrary;

[TestClass]
public class UnitTest_KnownFolders
{
    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Test_DataDir()
    {
        var path = KnownFolders.DataDir;
        Assert.IsTrue(Path.IsPathFullyQualified(path));
        Assert.IsTrue( Directory.Exists(path));
        Assert.IsFalse(File.Exists(path));
    }

    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Test_DataFile()
    {
        var path = KnownFolders.DataFile;
        if (path != "")
        {
            Assert.IsTrue(Path.IsPathFullyQualified(path));
            Assert.IsFalse(Directory.Exists(path));
        }
    }

    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Test_GetPath()
    {
        string path = KnownFolders.GetPath(KnownFolder.Downloads);
        Assert.IsTrue(Path.IsPathFullyQualified(path));
        Assert.IsTrue(Directory.Exists(path));
        Assert.IsFalse(File.Exists(path));
    }
}
