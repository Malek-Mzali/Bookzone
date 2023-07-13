namespace Bookzone.Models.Entity.User
{
    public class UserInfo
    {
        public Administrator AdministratorGroup { get; set; }
        public Editor EditorGroup { get; set; }
        public Organization OrganizationGroup { get; set; }
        public Users UsersGroup { get; set; }
        public Individual IndividualGroup { get; set; }
    }
}