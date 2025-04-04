using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ConfigEditor.Forms;

namespace ConfigEditor
{
    public partial class MasterForm : DevExpress.XtraEditors.XtraForm
    {
        public MasterForm()
        {
            InitializeComponent();
            this.FormClosing += MasterForm_FormClosing;
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var lstToClose = new List<Form>();
            foreach (Form form in Application.OpenForms)
            {
                if (form is ECSChildForm)
                {
                    var canceled = (form as ECSChildForm).CancelClose;
                    if (canceled)
                    {
                        e.Cancel = true;
                        form.Focus();
                        foreach (var frm in lstToClose)
                        {
                            frm.Close();
                        }
                        return;
                    }
                    else
                    {
                        lstToClose.Add(form);
                    }
                }
            }
        }
    }
}