using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce.Services
{
    public class ImageServices
    {
        private string _ImagePath;
            public ImageServices() { 
            
            _ImagePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
        }
        public string UploadFile(IFormFile image) { 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filepath = Path.Combine(_ImagePath, fileName);
            using (var stream = System.IO.File.Create(filepath))
            {
                image.CopyTo(stream);
            }
            return fileName;
        }
        public void DeleteFile(string fileName)
        {
            var filepath = Path.Combine(_ImagePath, fileName);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
        }
    }
}
