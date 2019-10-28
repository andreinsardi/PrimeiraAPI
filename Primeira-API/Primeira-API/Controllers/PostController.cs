using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Primeira_API.Request;
using Primeira_API.Result;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Primeira_API.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        public readonly Appsettings _appSettings;

        public PostController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get() {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<PostResult> result = new List<PostResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT postID, AuthorID, Title, Text, Created FROM Post", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new PostResult
                        {
                            PostID = dataReader.GetInt32(0),
                            AuthorID = dataReader.GetInt32(1),
                            Title = dataReader.GetString(2),
                            Text = dataReader.GetString(3),
                            Created = dataReader.GetDateTime(4)

                        });

                    }
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }


        }

        [HttpPost]
        public IActionResult Post([FromBody]PostRequest request)
        {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            PostResult result = new PostResult();
            DateTime date = DateTime.Now;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Post (Title, Text, Created, AuthorID) VALUES ( @Title, @Text, @Created, @AuthorID)", conn))
                {

                    cmd.Parameters.AddWithValue("@Title", request.Title);
                    cmd.Parameters.AddWithValue("@Text", request.Text);
                    cmd.Parameters.AddWithValue("@Created", date);
                    cmd.Parameters.AddWithValue("@AuthorID", request.AuthorID);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        result.PostID = (int)(ulong)cmd2.ExecuteScalar();

                    }

                    result.AuthorID = request.AuthorID;
                    result.Title = request.Title;
                    result.Text = request.Text;
                    result.Created = date;

                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpPut("{PostID}")]
        public IActionResult Put(int PostID, [FromBody]PostRequest request)
        {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            PostResult result = new PostResult();
            DateTime date = DateTime.Now;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE Post set Title = @Title, Text = @Text, Created = @Created, AuthorID = @AuthorID WHERE PostID = @PostID", conn))
                {

                    cmd.Parameters.AddWithValue("@PostID", PostID);
                    cmd.Parameters.AddWithValue("@Title", request.Title);
                    cmd.Parameters.AddWithValue("@Text", request.Text);
                    cmd.Parameters.AddWithValue("@Created", date);
                    cmd.Parameters.AddWithValue("@AuthorID", request.AuthorID);

                    cmd.ExecuteNonQuery();

                    result.PostID = PostID;
                    result.AuthorID = request.AuthorID;
                    result.Title = request.Title;
                    result.Text = request.Text;
                    result.Created = date;

                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        [HttpDelete("{PostID}")]
        public IActionResult Delete(int PostID)
        {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("delete from Post where PostID = @PostId", conn))
                {
                    cmd.Parameters.AddWithValue("@PostId", PostID);

                    cmd.ExecuteNonQuery();
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpGet("{PostID}")]
        public IActionResult Get(int PostID)
        {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            PostResult result = new PostResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT postID, AuthorID, Title, Text, Created FROM Post where postID = " + PostID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        result.PostID = dataReader.GetInt32(0);
                        result.AuthorID = dataReader.GetInt32(1);
                        result.Title = dataReader.GetString(2);
                        result.Text = dataReader.GetString(3);
                        result.Created = dataReader.GetDateTime(4);
                    }
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }


        }

    }
}
