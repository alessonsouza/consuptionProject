using System;

namespace backend.Models
{
    public class Users
    {
        public int NumCadastro { get; set; }
        public string Name { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime DataInicio { get; set; }
        public int HORACC { get; set; }
        public int SEQACC { get; set; }
    }
}