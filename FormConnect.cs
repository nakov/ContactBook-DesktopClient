using System.Windows.Forms;

namespace ContactBook_DesktopClient
{
    public partial class FormConnect : Form
    {
        public FormConnect()
        {
            InitializeComponent();
        }

        public string ApiUrl { get => this.textBoxApiUrl.Text; }
    }
}
