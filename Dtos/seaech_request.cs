namespace Homely_modified_api.Dtos
{
    public class seaech_request
    {
        public decimal? Price { get; set; }

        public string? Type { get; set; } = string.Empty;

        public string? Contract_type { get; set; } = string.Empty;

        public double? Area { get; set; }

        public int? BuildYear { get; set; }

        public string Key_word { get; set; } = string.Empty;
        
    }
}
