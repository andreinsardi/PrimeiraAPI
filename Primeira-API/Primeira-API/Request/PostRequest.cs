using System;
namespace Primeira_API.Request
{
    public class PostRequest
    {
 
        public int AuthorID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
    }
}
