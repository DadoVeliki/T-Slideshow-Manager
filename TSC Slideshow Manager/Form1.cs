using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSC_Slideshow_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ime = textBox1.Text;
            string lozinka=textBox2.Text;
            Spajanje spajanje= new Spajanje();
            spajanje.unos(ime,lozinka);
            if (spajanje.stanje)
            {
                Form2 form2 = new Form2(spajanje,ime,lozinka);
                form2.ShowDialog();
                this.Close();
            }
            
        }
    }
}
