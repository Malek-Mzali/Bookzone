using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.User
{
    public class Editor
    {
        
        public Editor()
        {
        }
        
        public Editor(int id, string website, string about, string name, string city, int multiplyer)
        {
            Id = id;
            Website = website;
            About = about;
            Name = name;
            City = city;
            Multiplyer = multiplyer;
        }

        #region Editor Fields

        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string City { get; set; }

        [Required(ErrorMessage = "Your must provide a website")]
        [RegularExpression(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage = "Website is not valid")]
        public string Website { get; set; }

        public string About { get; set; }

       [Required] public int Multiplyer { get; set; }

        #endregion   
    }
}