#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.User
{
    public class Individual
    {
        public Individual()
        {
        }

        public Individual(int id, string? firstname, string? lastname, string? gender, string? dateofBirth,
            string? profession, string? organization)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
            DateofBirth = dateofBirth;
            Profession = profession;
            Organization = organization;
        }

        #region Individual Fields

        public int Id { get; set; }

        [Required(ErrorMessage = "Your must provide a First name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can contain only letters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Your must provide a Last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can contain only letters")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Your must choose a Gender")]
        [RegularExpression(@"^Male$|^Female$", ErrorMessage = "Choose a valid gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Your must provide a Birth date")]
        public string? DateofBirth { get; set; }

        public string? Profession { get; set; }
        public string? Organization { get; set; }

        #endregion
    }
}