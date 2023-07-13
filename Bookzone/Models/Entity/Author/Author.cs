using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Author
{
    public class Author
    {


        public Author()
        {
            
        }

        #region Author Fields

        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }


        #endregion
    }
}