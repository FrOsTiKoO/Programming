using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationApp
{
    public class Student
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int? Age { get; set; }

        public Group Group { get; set; }
    }
}
