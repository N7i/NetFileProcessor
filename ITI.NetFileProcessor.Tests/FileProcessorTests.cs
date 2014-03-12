using NUnit.Framework;
using System;
using System.IO;

namespace ITI.NetFileProcessor.Tests
{
    [TestFixture]
    public class FileProcessorTests
    {
        static string GenerateUniqPath() 
        { 
            return Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString()); 
        }

        static void CreateDirectory(string rootPath, int folderCount, int filesCount, int withHiddenFileCount)
        {
            int hiddenFileIdx = 0;

            for(var i=0; i<folderCount; ++i) {
                Directory.CreateDirectory(Path.Combine(rootPath, i.ToString()));
            }

            for (var i=0; i<filesCount; ++i) {
                if (hiddenFileIdx < withHiddenFileCount)
                {
                    var path = Path.Combine(rootPath, i.ToString(), "42.tmp");
                    File.Create(path);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                    hiddenFileIdx++;
                }
                else
                {
                    File.Create(Path.Combine(rootPath, i.ToString(), "42.tmp"));
                }
            }
        }

        [Test]
        public void TestTheAnswer()
        {
            //Arrange,
            var rootPath = GenerateUniqPath();
            Console.WriteLine(rootPath);
            CreateDirectory(rootPath, 15, 15, 10);

            // Act,
            FileProcessor processor = new FileProcessor();
            DirectoryResult result = processor.Process(rootPath);

            // Assert
            Assert.That(result.FileCount == 15);
            Assert.That(result.DirectoryCount == 15);
            Assert.That(result.HiddenFileCount == 10);
        }
    }
}
