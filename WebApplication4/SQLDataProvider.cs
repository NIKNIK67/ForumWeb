using WebApplication4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApplication4.Controllers;
using System.Reflection;
#nullable disable
namespace WebApplication4
{
    public class SQLDataProvider : IDataProvider
    {
        public static string ConnectionString;
          
        public bool CheckUser(string email)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT Count(*) FROM Users WHERE Email='{email}'";
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            if (result == 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckUser(string email, string password)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT Count(*) FROM Users WHERE password=HASHBYTES ( 'SHA2_256', @password ) And email=@email";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            conn.Open();
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            if (result > 0)
            { 
                return true;
            }
            return false;
        }
        public void CreateArticle(ArticleModel model,int UserId)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"INSERT INTO Articles (Header,Content,AuthorId,CreationDate) Values (@header,@content,@id,@date)";
            command.Parameters.AddWithValue("@header", model.Header);
            command.Parameters.AddWithValue("@content", model.Content);
            command.Parameters.AddWithValue("@id", UserId);
            command.Parameters.AddWithValue("@date", DateTime.UtcNow);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }
        public void CreateUser(RegisterModel user)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"INSERT INTO Users (name, email, password,CreationDate) VALUES (@name,@email,HASHBYTES ( 'SHA2_256', @password ),@date)";
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email.ToLower());
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@date", DateTime.UtcNow);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

        }

        public ArticleModel GetArticle(int id)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT Users.Name, Articles.Header,Articles.Content,Articles.ArticleId FROM Articles Inner join Users On AuthorId = Users.Id WHERE ArticleId=@id";
            command.Parameters.AddWithValue("@id",id);
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            ArticleModel model = new ArticleModel();
            if(dataReader.HasRows)
            model = new ArticleModel() { Header = dataReader.GetString(1), AutorName = dataReader.GetString(0), Content = dataReader.GetString(2), Id = dataReader.GetInt32(3)};
            conn.Close();
            return model;
        }

        public List<ArticleModel> GetArticles()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT Articles.Header,Articles.Content,Users.Name,Articles.ArticleId,Articles.CreationDate FROM Articles Inner join Users On AuthorId = Users.Id";
            conn.Open();
            List<ArticleModel> models = new List<ArticleModel>();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                models.Add(new ArticleModel() { Header = dataReader.GetString(0), Content = dataReader.GetString(1), AutorName = dataReader.GetString(2),Id = dataReader.GetInt32(3),CreationDate=dataReader.GetDateTime(4)}) ;
            }
            conn.Close();
            return models;
        }

        public List<CommentModel> GetComments(int ArticleId)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT Comments.*,Users.name FROM Comments Inner join Users On AuthorId = Users.Id Where ArticleId =@ArticleId";
            command.Parameters.AddWithValue("@ArticleId", ArticleId);
            SqlDataReader dataReader = command.ExecuteReader();
            List<CommentModel> result = new List<CommentModel>();
            while (dataReader.Read())
            {
                result.Add(new CommentModel() { Id = dataReader.GetInt32(0), ParentId = dataReader.GetInt32(3), ArticleId = dataReader.GetInt32(4), AuthorId = dataReader.GetInt32(1), Content = dataReader.GetString(2), AuthorName = dataReader.GetString(6),CreationDate=dataReader.GetDateTime(5) });
            }
            conn.Close();
            return result;
        }  
        public void CreateComment(CommentModel model)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "INSERT INTO Comments (AuthorId,Content,ParentId,ArticleId,CreationDate) Values (@AuthorId,@Content,@ParentId,@ArticleId,@date)";
            command.Parameters.AddWithValue("@AuthorId",model.AuthorId);
            command.Parameters.AddWithValue("@Content",model.Content);
            command.Parameters.AddWithValue("@ParentId",model.ParentId);
            command.Parameters.AddWithValue("@ArticleId",model.ArticleId);
            command.Parameters.AddWithValue("@date",DateTime.UtcNow);
            command.ExecuteNonQuery();
            conn.Close();

        }
        public int UserId(string email)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT Id FROM Users WHERE Email = @Email";
            command.Parameters.AddWithValue("@Email", email);
            conn.Open();
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }
        public string GetName(LoginModel model)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT name FROM Users WHERE Email = @Email";
            command.Parameters.AddWithValue("@Email", model.Email);
            conn.Open();
            string result = Convert.ToString(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        public void CreateLog(string IpAddress, string Content, int CallerId = -1)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = $"INSERT INTO Logs (CallerIpAddressV4,CallerId,Content) values (@CallerIpAddressV4,@CallerId,@Content)";
            command.Parameters.AddWithValue("@CallerIpAddressV4", IpAddress);
            command.Parameters.AddWithValue("@CallerId", CallerId);
            command.Parameters.AddWithValue("@Content", Content);
            conn.Open();
            command.ExecuteScalar();
            conn.Close();
        }
    }
}
