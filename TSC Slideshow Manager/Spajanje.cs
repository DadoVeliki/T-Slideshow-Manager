using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
namespace TSC_Slideshow_Manager
{
    public class Spajanje
    {
        public SqlConnection cnn;
        public bool stanje = false;
        public Spajanje() {

            
        }
        public static Spajanje instance = null;
        public static Spajanje GetInstance()
        {
                if (instance == null)
                {
                    instance = new Spajanje();
                }
                return instance;
            
        }
        public void unos(string ime, string lozinka)
        {
            try
            {
                string cs = @"Data Source=localhost\sqldado;Initial Catalog=slideshow;User ID=" + ime + ";Password=" + lozinka;
                cnn = new SqlConnection(cs);
                cnn.Open();
                cnn.Close();
                stanje = true;
            }
            catch { }
        }

    }
}
