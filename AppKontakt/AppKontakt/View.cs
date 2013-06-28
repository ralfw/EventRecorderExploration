using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppKontakt
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();
        }


        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Veränderte_Daten((DataTable)this.dataGridView1.DataSource);
        }

        
        public void Anzeigen(DataTable tb)
        {
            this.dataGridView1.DataSource = tb;
        }


        public event Action<DataTable> Veränderte_Daten;

    }
}
