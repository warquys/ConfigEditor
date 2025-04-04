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

namespace ConfigEditor.CustomClass
{
    public partial class CustomSymlClassEditUC : ECSEditUserControl
    {
        private CustomSymlClassManager _manager;
        public CustomSymlClassEditUC() : base()
        {
            InitializeComponent();
        }

        public CustomSymlClassEditUC(IWriteManager manager) : base(manager)
        {
            InitializeComponent();
            BindingDataSource.DataSource = manager.CurrentObject;
            _manager = manager as CustomSymlClassManager;
        }

        public override bool BeforeSave()
        {
            if (_manager.CheckIdInvalid())
            {
                _idEdit.ErrorText = "This id is already used!";
                return false;
            }
            return base.BeforeSave();
        }
    }
}
