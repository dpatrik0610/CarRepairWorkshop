using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairWorkshop.Shared
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
