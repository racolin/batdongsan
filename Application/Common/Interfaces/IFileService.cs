using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;
public interface IFileService
{
    Task<string> SaveFileCommand(IFormFile file, string folder, string? name);
    string? RenameFileCommand(string folder, string oldFilename, string newFilename);
    bool DeleteFileCommand(string filename, string folder);
}