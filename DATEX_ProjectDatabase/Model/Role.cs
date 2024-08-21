namespace DATEX_ProjectDatabase.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        // Navigation property
        public ICollection<Employee> Employees { get; set; }
    }
}
