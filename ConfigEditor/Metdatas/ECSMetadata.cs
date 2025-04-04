using System;
using System.Collections.Generic;


namespace ConfigEditor.Metadatas
{
    public struct ECSMetadata
    {
        #region Attributes & Properties
        public Type Type { get; private set; }

        public string Text { get; private set; }
        public string IconKey { get; private set; }

        public bool HaveText { get { return !String.IsNullOrWhiteSpace(Text); } }

        public bool HaveIcon { get { return !String.IsNullOrWhiteSpace(IconKey); } }

        public bool AddControlText { get; private set; }
        #endregion


        #region Constructors & Destructor
        public ECSMetadata(Type type) : this(type, String.Empty)
        {

        }

        public ECSMetadata(Type type, string text) : this(type, text, String.Empty)
        {

        }

        public ECSMetadata(Type type, string text, string iconKey, bool addControlText = false)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            Type = type;
            Text = text;
            IconKey = iconKey;
            AddControlText = addControlText;
        }
        #endregion


        #region Methods
        public override bool Equals(object obj)
        {
            if (!(obj is ECSMetadata))
            {
                return false;
            }

            var metadata = (ECSMetadata)obj;
            return EqualityComparer<Type>.Default.Equals(Type, metadata.Type);
        }

        public override int GetHashCode()
        {
            return 2049151605 + EqualityComparer<Type>.Default.GetHashCode(Type);
        }

        public static bool operator ==(ECSMetadata metadata1, ECSMetadata metadata2)
        {
            return metadata1.Equals(metadata2);
        }

        public static bool operator !=(ECSMetadata metadata1, ECSMetadata metadata2)
        {
            return !(metadata1 == metadata2);
        }
        #endregion
    }
}