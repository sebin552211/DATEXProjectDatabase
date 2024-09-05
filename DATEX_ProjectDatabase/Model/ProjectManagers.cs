using DATEX_ProjectDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATEX_ProjectDatabase.Model
{
    public class ProjectManagers
    {

        [Key]
        public int ProjectManagerId { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }

        // Foreign key reference to Project based on ProjectCode
     
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
