using System;
namespace Primeira_API.Result
{
    public class PostResult
    {
        public int PostID { get; set; }
        public int AuthorID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }

    }
}
