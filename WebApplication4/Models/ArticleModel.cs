using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication4.Models
#nullable disable
{
    public class ArticleModel
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public new string Content { get; set; }
        public string AutorName { get; set; }
        public DateTime CreationDate { get; set; }
        
    }
}
