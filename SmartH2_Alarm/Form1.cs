using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartH2_Alarm
{
    public partial class Form_Alarms : Form
    {
      

        public Form_Alarms()
        {
            InitializeComponent();
        }

        private void button_options_Click(object sender, EventArgs e)
        {
            string promptValue = Prompt.ShowDialog("Insira novo IP", "Options");
        }

        private void Form_Alarms_Load(object sender, EventArgs e)
        {

        }
    }
}
