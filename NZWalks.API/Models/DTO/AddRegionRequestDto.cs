using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be a minimum of 3 characters!")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters!")]
        public string Code { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Name has to be a minimum of 1 characters!")]
        [MaxLength(30, ErrorMessage = "Name has to be a maximum of 30 characters!")]
        public string Name { get; set; }
        public string? RegionIamgeURL { get; set; }
    }
}
