using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Document
{
    public class Document
    {
        public Document()
        {
        }
        public Document(int id, string originalTitle, int editor, int collection, string doi, string marcRecordNumber, string titlesVariants, string subtitle,
            List<string> keywords, string file, string fileFormat, string coverPage, string url, string documentType, string originalLanguage,
            string languageVariants, string translator, string accessType, string state, float price, string publicationDate, string country, 
            string physicalDescription, int volumeNb, string @abstract, string notes)
        {
            Id = id;
            OriginalTitle = originalTitle;
            IdEditor = editor;
            IdCollection = collection;
            Doi = doi;
            MarcRecordNumber = marcRecordNumber;
            TitlesVariants = titlesVariants;
            Subtitle = subtitle;
            Keyword = keywords;
            File = file;
            FileFormat = fileFormat;
            CoverPage = coverPage;
            Url = url;
            DocumentType = documentType;
            OriginalLanguage = originalLanguage;
            LanguageVariants = languageVariants;
            Translator = translator;
            AccessType = accessType;
            State = state;
            Price = price;
            PublicationDate = publicationDate;
            Country = country;
            PhysicalDescription = physicalDescription;
            VolumeNb = volumeNb;
            Abstract = @abstract;
            Notes = notes;
        }

        #region Document Fields
        public int Id { get; set; }
        [Required]public string OriginalTitle { get; set; }
        [Required]public int IdEditor { get; set; }
        [Required]public int IdCollection { get; set; }
        [Required]public int IdAuthor { get; set; }
        public string Doi { get; set; }
        public string MarcRecordNumber { get; set; }
        public string TitlesVariants { get; set; }
        public string Subtitle { get; set; }
        [Required]public List<string> Keyword { get; set; }
        [Required]public string File { get; set; }
        public string FileFormat { get; set; }
        public string CoverPage { get; set; }
        public string Url { get; set; }
        [Required]public string DocumentType { get; set; }
        [Required]public string OriginalLanguage { get; set; }
        public string LanguageVariants { get; set; }
        public string Translator { get; set; }
        public string AccessType { get; set; }
        public string State { get; set; }
        [Required]public float Price { get; set; }

        [Required(ErrorMessage = "Your must provide a Publication date")]
        [RegularExpression(@"^[12][0-9]{3}$",
            ErrorMessage = "Date is not in valid format")]
        public string PublicationDate { get; set; }
        public string Country { get; set; }
        public string PhysicalDescription { get; set; }
        public int VolumeNb { get; set; }
        public string Abstract { get; set; }
        public string Notes { get; set; }
        #endregion
    }
}