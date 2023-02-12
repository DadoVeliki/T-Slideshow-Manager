using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace TSC_Slideshow_Manager
{
    public partial class Dodavanje : Form
    {
        public Spajanje spajanje;
        public Dodavanje(Spajanje spajanje)
        {
            InitializeComponent();
            this.spajanje = spajanje;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ime = naziv.Text;
            string ipAdresa=ip.Text;
            spajanje.cnn.Open();
            SqlCommand naredba;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";
            sql = $"INSERT INTO uredaji (nazivUredaja,ipUredaja) VALUES ('{ime}','{ipAdresa}')";
            naredba = new SqlCommand(sql, spajanje.cnn);
            adapter.InsertCommand = new SqlCommand(sql, spajanje.cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            spajanje.cnn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
