using System.ComponentModel.DataAnnotations;

namespace bedandbreakfast1
{
    public class Guest
    {
        [Required]
        [RegularExpression(@"^[\p{L}\s]{2,50}$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9]{8,15}$")]
        public string Phone { get; set; }
    }
}
