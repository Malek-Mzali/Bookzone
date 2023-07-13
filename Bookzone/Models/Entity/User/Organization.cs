using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.User

{
    public class Organization
    {
        public Organization()
        {
        }

        public Organization(string name, string shortName, string website, string about, string type,
            List<string> ipAdress)
        {
            Name = name;
            ShortName = shortName;
            Website = website;
            About = about;
            Type = type;
            IpAdress = ipAdress;
        }

        #region Organization Fields

        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public string ShortName { get; set; }

        [Required(ErrorMessage = "Your must provide a website")]
        [RegularExpression(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage = "Website is not valid")]
        public string Website { get; set; }

        public string About { get; set; }

        [Required] public string Type { get; set; }

        [Required] public List<string> IpAdress { get; set; }

        #endregion
    }
}