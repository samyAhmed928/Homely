namespace Homely_modified_api.Services
{
    public interface IImageUploadService
    {
        Task<List<string>> Upload_images(ICollection<IFormFile> files);
        Task<string> Upload_single_image(IFormFile file);

    }
}
