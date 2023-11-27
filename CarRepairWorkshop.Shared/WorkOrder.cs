using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.Shared
{
    public class WorkOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required] public Guid CustomerId { get; set; }

        [Required]
        [NotNull]
        [RegularExpression(@"^[A-Z]{3}-[0-9]{3}$", ErrorMessage = "License plate must be in the format XXX-123.")]
        public string LicensePlate { get; set; }

        [Required]
        [NotNull]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "Date of production cannot be lower than 1900 and above today's date.")]
        public DateTime DateOfProduction { get; set; } = DateTime.Now;

        [Required]
        public RepairCategory RepairCategory { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [NotNull]
        [Range(1, 10, ErrorMessage = "Value must be in range 1 - 10")]
        public int DamageSeverity { get; set; }

        private JobStatus _status = JobStatus.Recorded;
        [Required]
        public JobStatus Status
        {
            get { return _status; }
            set {
                if (value >= _status || _status == JobStatus.Recorded) {
                    _status = value;
                }
            }
        }
    }
}
