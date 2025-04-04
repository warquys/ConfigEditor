using ConfigEditor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Utils
{
    public static class ElementExtensions
    {
        #region Methods

        public static bool IsNew(this BaseUintElement element)
        {
            // Safe design
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            return element.Id == UInt32.MaxValue;
        }

        public static bool IsNew(this BaseStringElement element)
        {
            // Safe design
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            return String.IsNullOrEmpty(element.Id);
        }

        #endregion Methods
    }
}
