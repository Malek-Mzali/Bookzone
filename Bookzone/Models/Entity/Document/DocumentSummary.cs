using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Document
{
    public class DocumentSummary
    {
        public DocumentSummary()
        {
        }

        public DocumentSummary(int id, string title, int start, int end, int idDocument)
        {
            Id = id;
            Title = title;
            Start = start;
            End = end;
            IdDocument = idDocument;
        }

        #region DocumentSummary Fields

        public int Id { get; set; }
        [Required] public int IdDocument { get; set; }
        [Required] public string Title { get; set; }
        [Required] public int Start { get; set; }
        [Required] public int End { get; set; }

        #endregion
    }
}