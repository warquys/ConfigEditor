using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Elements
{
    public class DelStatus : BaseUintElement
    {
        private enum DelStatusEnum : uint
        {
            Error = 0,
            Success = 6300,
        }


        /// <summary>
        /// 0 = Error when executing the delete statement directly from the client
        /// </summary>
        public static DelStatus Error { get { return new DelStatus { Id = (uint)DelStatusEnum.Error }; } }

        /// <summary>
        /// 6300
        /// </summary>
        public static DelStatus Success { get { return new DelStatus { Id = (uint)DelStatusEnum.Success }; } }

    }
}
