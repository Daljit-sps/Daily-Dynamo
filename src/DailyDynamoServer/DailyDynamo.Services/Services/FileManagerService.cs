using DailyDynamo.Shared.Utilities;
using Microsoft.AspNetCore.Http;

namespace DailyDynamo.Services.Services;

public class FileManagerService(string webRootPath)
{
    private readonly string directory = Path.Combine(webRootPath, AppSettingsUtility.Settings.AssetPaths.ProfileImagesDirectoryPath);

    public async Task<string> SaveFile(IFormFile file, string fileName)
    {
        Directory.CreateDirectory(directory);

        string fullPath = Path.Combine(directory, fileName);

        using FileStream fs = new(fullPath, FileMode.Create);
        await file.CopyToAsync(fs);

        return Path.Combine(AppSettingsUtility.Settings.AssetPaths.ProfileImagesDirectoryPath, fileName);
    }

    public void Delete(string fileName)
    {
        var filePath = Path.Combine(webRootPath, fileName);
        File.Delete(filePath);
    }
}
