using System.ComponentModel.DataAnnotations;
#nullable disable
namespace WebApplication4.Models
{
    public class CommentModel
    {
        public List<CommentModel> ChildComments { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int ParentId { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
