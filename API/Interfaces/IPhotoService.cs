using System;

namespace API.Interfaces;

public interface IPhotoService
{
    String AddPhoto(IFormFile file);
    void DeletePhoto(string fileUrl);
}
