using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
        }

        public void DeleteRole(int roleId)
        {
            var role = _context.Roles.Find(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
