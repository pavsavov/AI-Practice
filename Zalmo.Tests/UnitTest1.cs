using System;
using System.IO;
using Zalmo.Infrastructure;
using Zalmo.Core;

namespace Zalmo.Tests;

public class MetadataServiceTests
{
    [Fact]
    public void GetPhotoTakenDate_ReturnsFileCreation_WhenNoExif()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            var expected = File.GetCreationTime(tempFile);
            // Act
            var actual = ImageMetadataService.GetPhotoTakenDate(tempFile);
            // Assert
            Assert.Equal(expected.Date, actual.Date);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}

public class ConfigPersistenceServiceTests
{
    [Fact]
    public void SaveAndLoadConfig_PersistsAndRestoresConfig()
    {
        // Arrange
        var config = new AppConfig { BatchSize = 5, FolderNamingPattern = "dd.MM.yyyy" };
        ConfigPersistenceService.SaveConfig(config);
        // Act
        var loaded = ConfigPersistenceService.LoadConfig();
        // Assert
        Assert.Equal(5, loaded.BatchSize);
        Assert.Equal("dd.MM.yyyy", loaded.FolderNamingPattern);
        // Cleanup
        if (File.Exists("appconfig.json"))
            File.Delete("appconfig.json");
    }
}
