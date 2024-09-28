using API.Interfaces;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PhotoService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string AddPhoto(IFormFile file)
    {
        string wwwRootPath = ""/*_webHostEnvironment.ApplicationName*/;
        string imageUrl = "";

        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images");

            // if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
            // {
            //     var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

            //     if (System.IO.File.Exists(oldImagePath))
            //     {
            //         System.IO.File.Delete(oldImagePath);
            //     }
            // }

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            imageUrl = @"\images\" + fileName;

        }

        return imageUrl;
    }

    public void DeletePhoto(string fileUrl)
    {
        string wwwRootPath = /*_webHostEnvironment.WebRootPath*/"";

        if (!string.IsNullOrEmpty(fileUrl))
        {
            var oldImagePath = Path.Combine(wwwRootPath, fileUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
    }
}
