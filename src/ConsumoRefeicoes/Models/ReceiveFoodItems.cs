using System;

namespace backend.Models
{
    public class ReceiveFoodItems
    {
        public string Descricao { get; set; }
        public int USU_CODREF { get; set; }
        public int USU_NUMCAD { get; set; }
        public int USU_QTDREF { get; set; }

        public int USU_NUMEMP { get; set; }
        public int USU_TIPCOL { get; set; }
        public string USU_DATCON { get; set; }
        public string USU_HORCON { get; set; }
        public string USU_TPCAPT { get; set; }

    }
}