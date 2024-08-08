using Application.Common.Interfaces;
using System.IO;

namespace WebUI.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public async Task<string> SaveFileCommand(IFormFile file, string folder, string? name)
    {
        var extension = Path.GetExtension(file.FileName);

        var filename = (string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name) + extension;

        folder = Path.Combine(_environment.WebRootPath, "upload", folder);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var path = Path.Combine(folder, filename);
        if (File.Exists(path))
        {
            throw new Exception("Tên file đã được sử dụng, vui lòng đổi tên khác!");
        }
        using (var stream = File.Create(path))
        {
            await file.CopyToAsync(stream);
        }

        return filename;
    }

    public string? RenameFileCommand(string folder, string oldFileName, string? newFileName)
    {
        if (oldFileName.Equals(newFileName)) {
            return newFileName;
        }
        newFileName = string.IsNullOrEmpty(newFileName) ? Guid.NewGuid().ToString() : newFileName;
        folder = Path.Combine(_environment.WebRootPath, "upload", folder);
        if (File.Exists(Path.Combine(folder, newFileName)))
        {
            throw new Exception("Tên file đã được sử dụng, vui lòng đổi tên khác!");
        }
        if (Directory.Exists(folder) && File.Exists(Path.Combine(folder, oldFileName)))
        {
            File.Move(Path.Combine(folder, oldFileName), Path.Combine(folder, newFileName));
            File.Delete(Path.Combine(folder, oldFileName));
            return newFileName;
        }
        return null;
    }

    public bool DeleteFileCommand(string filename, string folder)
    {

        folder = Path.Combine(_environment.WebRootPath, "upload", folder);

        var path = Path.Combine(folder, filename);

        if (Directory.Exists(folder) && File.Exists(Path.Combine(path)))
        {
            File.Delete(path);
            return true;
        }

        return false;
    }
}