using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaskService.Models
{
    public class TaskObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Description{ get; set; }
        
        public DateTime StartDate { get; set; }
        [Required]
        public int StartHour { get; set; }

        [Required]
        public int EstimatedHours { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public Guid CustomerGuid { get; set; }
        public Guid TaskGuid { get; set; }        
        public Guid Employee { get; set; }

    }
}
