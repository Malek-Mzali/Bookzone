using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Collection
{
    public class Collection
    {
        public Collection(int id, int idEditor, int idTheme, string title, string shortTitle, string description)
        {
            Id = id;
            IdEditor = idEditor;
            IdTheme = idTheme;
            Title = title;
            ShortTitle = shortTitle;
            Description = description;
        }

        public Collection()
        {
            
        }

        #region Collection Fields

        public int Id { get; set; }
        [Required] public int IdEditor { get; set; }
        [Required] public int IdTheme { get; set; }
        [Required] public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }


        #endregion
    }
}