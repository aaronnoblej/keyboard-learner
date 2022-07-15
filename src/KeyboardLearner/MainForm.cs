using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class MainForm : Form
    {
        // PROPERTIES
        public static TitleScreen Title { get; set; }
        public static ProfileSelection Profiles { get; set; }

        public MainForm()
        {
            this.KeyPreview = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            InitializeComponent();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void TitleScreen_Load(object sender, EventArgs e)
        {
            Title = new TitleScreen();
            this.Controls.Add(Title);
        }

    }
}
