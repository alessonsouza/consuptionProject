using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using NITGEN.SDK.NBioBSP;

namespace backend.Models
{
    public class UserDigital
    {
        public int NUMCAD { get; set; }
        public string NOMFUN { get; set; }
        // public byte[] DESDIG { get; set; }

        private byte[] _desdig;

        public byte[] DESDIG
        {
            get
            {
                return _desdig;
            }

            set
            {
                _desdig = value;
                DESDIGCONVERT = ConvertDigital(value);
            }
        }

        public UserDigitalReceived[] DESDIGAUX { get; set; }
        public string DESDIGCONVERT { get; set; }
        public int FABBIO { get; set; }
        public int TIPTEM { get; set; }

        private string ConvertDigital(byte[] value)
        {
            string _DESDIGCONVERT = Encoding.Default.GetString(value);
            // string _DESDIGCONVERT = Convert.ToBase64String(value);


            return _DESDIGCONVERT;
        }

        public string GetDigitalHash()
        {

            string _DESDIGCONVERT = Encoding.ASCII.GetString(DESDIG, 0, DESDIG.Length);

            //string _DESDIGCONVERT = Encoding.Default.GetString(DESDIG);

            // Console.WriteLine(digital);
            return _DESDIGCONVERT;

        }


    }
}