using DATEX_ProjectDatabase.Models;

namespace DATEX_ProjectDatabase.Model
{
    public class ProjectData
    {
        public ProjectExternal Project { get; set; }
        public int? ResourceCount { get; set; }
        public List<string> Skills { get; set; }
    }
}
