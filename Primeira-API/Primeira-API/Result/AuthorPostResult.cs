using System;
using System.Collections.Generic;

namespace Primeira_API.Result
{
    public class AuthorPostResult
    {
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public List<PostResult> Posts { get; set; }
     }
}
