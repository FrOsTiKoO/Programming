using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MigrationApp
{
    public class Group
    {
        public int GroupId { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
