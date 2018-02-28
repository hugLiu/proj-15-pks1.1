using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PKS.Utils;

namespace PKS.SubmissionTool.Index
{
    public partial class frmShowOperationDesc : Form
    {
        public frmShowOperationDesc()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            var resName = "PKS.SubmissionTool.Index.Models.IndexOperationDesc.txt";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
            this.txtDesc.Text = Encoding.UTF8.GetString(stream.ToByteArray());
            this.txtDesc.SelectionStart = 0;
            this.txtDesc.SelectionLength = 0;
        }
    }
}
