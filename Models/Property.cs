using System.ComponentModel.DataAnnotations;

namespace Homely_modified_api.Models
{
    public class Property
    {
        public Guid Id { get; set; }

        [Required, MaxLength(1000)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required, MaxLength(10)]
        public string Type { get; set; } = string.Empty;
        public string? Rent_period { get; set; }=string.Empty;

        public string Contract_type { get; set; } = string.Empty;

        public int? NumberOfRooms { get; set; }

        public int? NumberOfBaths { get; set; }

        public int? NumberOfKitchens { get; set; }

        [Required]
        public double Area { get; set; }

        public int? BuildYear { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;


        public string? city { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        public string? featuresoftheproperty { get; set; } = string.Empty;
        public String Main_image { get; set; }=string.Empty;
        public Guid clientId { get; set; }


        public virtual Client client { get; set; }
        public virtual ICollection<PropertyImage>? ImagesUrls { get; set; }

    }
}
