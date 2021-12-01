using System;

namespace backend.Models
{
    public class FoodItems
    {
        public ReceiveFoodItems[] Food { get; set; }
        public int USU_HORCON { get; set; }
        public string USU_DATCON { get; set; }
        public int USU_NUMCAD { get; set; }
        public string USU_TPCAPT { get; set; }
    }
}