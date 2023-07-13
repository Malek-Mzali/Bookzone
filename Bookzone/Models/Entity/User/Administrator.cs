using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.User
{
    public class Administrator
    {
        public Administrator()
        {
        }

        public Administrator(int id, string website, string about)
        {
            Id = id;
            Website = website;
            About = about;
        }

        #region Administrator Fields

        public int Id { get; set; }

        [Required(ErrorMessage = "Your must provide a website")]
        [RegularExpression(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage = "Website is not valid")]

        public string Website { get; set; }

        public string About { get; set; }

        #endregion
    }
}