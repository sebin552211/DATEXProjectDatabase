namespace DATEX_ProjectDatabase.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public DateTime LastLoginTime { get; set; }

        // Navigation property
        public Role Role { get; set; }
    }
}
