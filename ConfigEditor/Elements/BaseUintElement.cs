using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ConfigEditor.Elements
{
    [Serializable]
    public abstract class BaseUintElement : BaseElement//, IEquatable<BaseUintElement>, IComparable<BaseUintElement>, IComparable
    {
        #region Properties

        [DefaultValue(UInt32.MaxValue)]
        [XmlElement("Id")]
        public virtual uint Id { get; set; }

        protected sealed override int InternalId
        {
            get { return Id.GetHashCode(); }
        }

        #endregion Properties


        #region Constructors

        protected BaseUintElement()
        {
            this.Id = UInt32.MaxValue;
        }

        protected BaseUintElement(uint id)
        {
            this.Id = id;
        }

        #endregion Constructors


        #region Methods

        public bool Equals(BaseUintElement other)
        {
            return other != null && (this.Id == other.Id);
        }

        #endregion Methods


    }
}