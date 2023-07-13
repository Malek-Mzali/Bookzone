using System.ComponentModel.DataAnnotations;

namespace Bookzone.Models.Entity.Theme
{
    public class Theme
    {
        public Theme(int id, string title, string shortTitle, string description, string icon)
        {
            Id = id;
            Title = title;
            ShortTitle = shortTitle;
            Description = description;
            Icon = icon;
        }

        public Theme()
        {
        }

        #region Theme Fields

        public int Id { get; set; }
        [Required] public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        #endregion   
    }
}