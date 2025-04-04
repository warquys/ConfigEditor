using System;

namespace ConfigEditor.Extension
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ExtensionAttributeAttribute : Attribute
    {
        public string Name { get; }

        public string AuthorName { get; }

        public ExtensionAttributeAttribute(string name, string authorName)
        {
            Name = name;
            AuthorName = authorName;
        }

    }
}