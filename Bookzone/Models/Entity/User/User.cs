using System;
using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.User
{
    public class Users
    {
        public Users()
        {
        }

        public Users(int id, int code, string email, bool emailConfirmed, string emailConfirmationCode,
            DateTime codeExpirationDate, string password, string phone, string country, string adress,
            string postalCode, string role, string photo, DateTime date)
        {
            Id = id;
            Code = code;
            Email = email;
            EmailConfirmed = emailConfirmed;
            EmailConfirmationCode = emailConfirmationCode;
            CodeExpirationDate = codeExpirationDate;
            Password = password;
            Phone = phone;
            Country = country;
            Address = adress;
            PostalCode = postalCode;
            Role = role;
            Photo = photo;
            Date = date;
        }

        #region User Fields
        public int Id { get; set; }
        public int Code { get; set; }
        [Required(ErrorMessage = "Your must provide an E-mail")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
            ErrorMessage = "E-mail field is not valid")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailConfirmationCode { get; set; }
        public DateTime CodeExpirationDate { get; set; }
        [Required(ErrorMessage = "Your must provide a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Your must provide a Phone number")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Phone field is not valid")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Your must choose a Country")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Your must provide a valid address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Your must provide a Postal Code")]
        public string PostalCode { get; set; }
        public string Role { get; set; }
        public string Photo { get; set; }
        public DateTime Date { get; set; }
        #endregion
    }
}