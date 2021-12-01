using System.Collections.Generic;

namespace backend.Models
{
    public class User
    {
        public int Matricula { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Situacao { get; set; }
        public List<Group> Groups { get; set; }

    }
}