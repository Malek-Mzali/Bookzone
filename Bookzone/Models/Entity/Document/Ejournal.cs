using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Document
{
    public class Ejournal
    {
        public Ejournal()
        {
            
        }
        
        public Ejournal(int id, string issn, string frequency, int totalIssuesNb, string dateFirstIssue, string journalScope, string impactFactor)
        {
            Id = id;
            Issn = issn;
            Frequency = frequency;
            TotalIssuesNb = totalIssuesNb;
            DateFirstIssue = dateFirstIssue;
            JournalScope = journalScope;
            ImpactFactor = impactFactor;
        }
        
        #region Ejournal Fields

        public int Id { get; set; }
        [Required(ErrorMessage = "Your must provide a ISSN")]
        [RegularExpression(@"^[0-9]{8}$|^[0-9]{4}-[0-9]{3}[0-9xX]$",
            ErrorMessage = "ISSN is not in valid format")]
        public string Issn { get; set; }
        public string Frequency { get; set; }
        public int TotalIssuesNb { get; set; }
        public string DateFirstIssue { get; set; }
        public string JournalScope { get; set; }
        public string ImpactFactor { get; set; }

        #endregion
    }
}