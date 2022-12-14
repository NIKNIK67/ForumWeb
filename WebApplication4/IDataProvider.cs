using WebApplication4.Models;
#nullable disable
namespace WebApplication4
{
    public interface IDataProvider
    {
        public void CreateUser(RegisterModel user);
        public bool CheckUser(string email);
        public bool CheckUser(string email, string password);
        public void CreateArticle(ArticleModel model, int UserId);
        public ArticleModel GetArticle(int id);
        public int UserId(string email);
        public List<ArticleModel> GetArticles();
        public List<CommentModel> GetComments(int ArticleId);
        public void CreateComment(CommentModel model);
        public string GetName(LoginModel model);
        public void CreateLog(string IpAddress, string Content, int CallerId);

    }
}
