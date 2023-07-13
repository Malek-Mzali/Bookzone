using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Document
{
    public class Ebook
    {
        public Ebook()
        {
            
        }
        
        public Ebook(int id, string editionNum, string editionPlace, string isbn, string genre, string category, int nbPages)
        {
            Id = id;
            EditionNum = editionNum;
            EditionPlace = editionPlace;
            Isbn = isbn;
            Genre = genre;
            Category = category;
            NbPages = nbPages;
        }
        
        #region Ebook Fields

        public int Id { get; set; }
        public string EditionNum { get; set; }
        public string EditionPlace { get; set; }
        [Required(ErrorMessage = "Your must provide a ISBN")]
        [RegularExpression(@"^[0-9xX]{13}$|^[0-9xX]{10}$|^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$",
            ErrorMessage = "ISBN is not in valid format")]
        public string Isbn { get; set; }
        public string Genre { get; set; }
        public string Category { get; set; }
        public int NbPages { get; set; }

        #endregion
    }
}