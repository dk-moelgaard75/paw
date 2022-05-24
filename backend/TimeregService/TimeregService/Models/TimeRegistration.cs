using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeregService.Models
{
    public class TimeRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid TaskGuid { get; set; }
        public Guid EmployeeGuid { get; set; }
        public double Hours { get; set; }

        public DateTime RegDate { get; set; }
    }
}
