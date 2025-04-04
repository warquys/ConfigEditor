using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;


namespace ConfigEditor.Utils
{
    public class ECSMessageBox
    {
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        #endregion


        #region Methods
        public static DialogResult Show(string text)
        {
            return Show(text, ECSFormUtility.ProductFullName);
        }

        public static DialogResult Show(string text, string title)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }

            return XtraMessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowWarning(string text)
        {
            return ShowWarning(text, ECSFormUtility.ProductFullName);
        }

        public static DialogResult ShowWarning(string text, string title)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }

            return MessageBox.Show(text, title, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowWarningWithOk(string text)
        {
            return ShowWarningWithOk(text, ECSFormUtility.ProductFullName);
        }

        public static DialogResult ShowWarningWithOk(string text, string title)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }

            return MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowError(string text)
        {
            return ShowError(text, ECSFormUtility.ProductFullName);
        }

        public static DialogResult ShowError(string text, string title)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }

            return MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuestion(string text)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }

            return MessageBox.Show(text, ECSFormUtility.ProductFullName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowInformation(string text)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }

            return MessageBox.Show(text, ECSFormUtility.ProductFullName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult ShowResult(string text, string title, bool result)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }


            return XtraMessageBox.Show(text, title, MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
        #endregion
    }
}