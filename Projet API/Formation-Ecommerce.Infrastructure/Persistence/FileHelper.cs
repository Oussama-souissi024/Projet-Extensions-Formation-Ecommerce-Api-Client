using Formation_Ecommerce_11_2025.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Formation_Ecommerce_11_2025.Infrastructure.Persistence
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _webHost;
        public FileHelper(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? UploadFile(IFormFile file, string folder)
        {
            if (file != null)
            {
                var fileDir = Path.Combine(_webHost.WebRootPath, folder);

                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }

                var fileName = Guid.NewGuid() + "-" + file.FileName;
                var filePath = Path.Combine(fileDir, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    {
                        file.CopyTo(fileStream);
                        return fileName;
                    }
                    catch
                    {
                        Console.WriteLine("Cannot upload this file");
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        public bool DeleteFile(string imageURL, string folder)
        {
            try
            {
                var filePath = Path.Combine(_webHost.WebRootPath, folder, imageURL);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                else
                {
                    Console.WriteLine("File Not Found");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
