using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(int roleId);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int roleId);
        void Save();
    }
}
