using Bookzone.Models.Entity.Document;

namespace Ebook.Models.Entity.Document
{
    public class DocumentInfo
    {
        public Bookzone.Models.Entity.Document.Document DocumentGroup { get; set; }
        public Bookzone.Models.Entity.Document.Ebook EbookGroup { get; set; }
        public Ejournal EjournalGroup { get; set; }
        
    }
}