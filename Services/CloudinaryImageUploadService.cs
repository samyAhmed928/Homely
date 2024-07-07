
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Homely_modified_api.Services
{
    public class CloudinaryImageUploadService : IImageUploadService
    {
        const string Cloud = "dekjizkhv";
        const string ApiKey = "517674178117951";
        const string ApiSecret = "ACfy1ENdr3PKd8hQwGYZDhxgYhA";
        private readonly Cloudinary _cloudinary;
        public CloudinaryImageUploadService()
        {
            var account = new Account(Cloud, ApiKey, ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }
        public async Task<List<string>> Upload_images(ICollection<IFormFile> files)
        {
            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                var image_url = await Upload_single_image(file);
                imageUrls.Add(image_url);
            }

            return imageUrls;
        }

        public async Task<string> Upload_single_image(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var uploadparams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, memoryStream),
            };

            var result = _cloudinary.Upload(uploadparams);

            if (result.Error != null)
            {
                throw new Exception($"Cloudinary error occured: {result.Error.Message}");
            }

            string Image_url= result.SecureUrl.ToString();
            return Image_url;


        }
    }
}
