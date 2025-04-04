using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ConfigEditor.Controls;
using ConfigEditor.Interfaces;
using ConfigEditor.Managers;
using ConfigEditor.Elements;

namespace ConfigEditor.CustomClass
{
    public partial class CompletorEditUC : ECSEditUserControl
    {
        private CompletorManager _manager;

        public CompletorEditUC()
        {
            InitializeComponent();
        }
        public CompletorEditUC(IWriteManager manager) : base(manager)
        {
            InitializeComponent();
            BindingDataSource.DataSource = manager.CurrentObject;
            _manager = manager as CompletorManager;
            cbType.Properties.Items.Add(CompletorType.ByName);
            cbType.Properties.Items.Add(CompletorType.ByValue);
        }
    }
}
