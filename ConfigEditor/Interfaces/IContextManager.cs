using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Interfaces
{
    public interface IContextManager<TContext> : IContextManager
    {
        TContext Context { get; set; }
    }

    /// <summary>
    /// Use this when you need the Detail Factory to handle a method that returns a control when given an element and a context
    /// </summary>
    public interface IContextManager
    {
        object ContextObject { get; }
    }
}
