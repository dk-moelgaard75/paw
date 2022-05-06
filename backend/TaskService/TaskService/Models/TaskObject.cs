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
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int EstimatedDays { get; set; }
        
        public DateTime EndDate { get; set; }
        [Required]
        public Guid CustomerGuid { get; set; }
        public Guid UID { get; set; }

    }
}
