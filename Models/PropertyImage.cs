namespace Homely_modified_api.Models
{
    public class PropertyImage
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public string Url { get; set; }
    }
}
