using System;

namespace Bookzone.Models.Entity.Document
{
    public class DocumentComment
    {
        public DocumentComment()
        {
        }

        public DocumentComment(int id, int idDocument, int idUser, string Username, string Userphoto, string text, DateTime date)
        {
            Id = id;
            IdDocument = idDocument;
            IdUser = idUser;
            UserName = Username;
            UserPhoto = Userphoto;
            Text = text;
            Date = date;
        }

        public int Id  { get; set; }
        public int IdDocument  { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }


    }
}