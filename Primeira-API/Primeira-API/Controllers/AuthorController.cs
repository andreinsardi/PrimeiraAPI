using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Primeira_API.Request;
using Primeira_API.Result;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Primeira_API.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {

        public readonly Appsettings _appSettings;

        public AuthorController(Appsettings appSettings) {
            _appSettings = appSettings;
        }


        [HttpGet("{AuthorID}")]
        public IActionResult Get(int AuthorID)
        {
            /*
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorResult result = new AuthorResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AuthorID, Name FROM Author where AuthorID =" + AuthorID, conn)) 
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {

                        result.AuthorID = dataReader.GetInt32(0);
                        result.Name = dataReader.GetString(1);

                    }
                }

                    return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally {
                conn.Dispose();
                conn.Close();
            }

            */

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorPostResult result = null;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT Author.AuthorID, Author.Name, Post.postID, Post.Title, Post.Text, Post.Created FROM Author inner join Post on (Author.AuthorID = Post.AuthorID) where Author.AuthorID =" + AuthorID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (result == null) {

                            result = new AuthorPostResult
                            {
                                AuthorID = dataReader.GetInt32(0),
                                Name = dataReader.GetString(1)
                            };

                            result.Posts = new List<PostResult>();
                        }

                        result.Posts.Add(new PostResult
                        {
                            AuthorID = dataReader.GetInt32(0),
                            PostID = dataReader.GetInt32(2),
                            Title = dataReader.GetString(3),
                            Text = dataReader.GetString(4),
                            Created = dataReader.GetDateTime(5) 
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


        [HttpGet]
        public IActionResult Get() {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<AuthorResult> result = new List<AuthorResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AuthorID, Name FROM Author", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new AuthorResult {
                            AuthorID = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1)
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
        public IActionResult Post([FromBody]AuthorRequest request) {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorResult result = new AuthorResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("insert into Author (Name) Values (@name)", conn))
                {
                    cmd.Parameters.AddWithValue("@name", request.Name);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        result.AuthorID = (int)(ulong)cmd2.ExecuteScalar();

                    }

                }

                result.Name = request.Name;

                    return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally {
                conn.Dispose();
                conn.Close();
            }
           

            

        }

        [HttpPut("{AuthorID}")]
        public IActionResult Put(int AuthorID, [FromBody]AuthorRequest request) {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorResult result = new AuthorResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("update Author set name = @name where AuthorID = @AuthorId", conn))
                {
                    cmd.Parameters.AddWithValue("@name", request.Name);
                    cmd.Parameters.AddWithValue("@AuthorId", AuthorID);

                    cmd.ExecuteNonQuery();
                }

                result.AuthorID = AuthorID;
                result.Name = request.Name;

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

        [HttpDelete("{AuthorID}")]
        public IActionResult Delete(int AuthorID) {

            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorResult result = new AuthorResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("delete from Author where AuthorID = @AuthorId", conn))
                {
                    cmd.Parameters.AddWithValue("@AuthorId", AuthorID);

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

    }
}
