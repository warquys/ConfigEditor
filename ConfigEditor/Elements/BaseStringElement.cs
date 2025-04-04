using System;
using System.ComponentModel;


namespace ConfigEditor.Elements
{
    public abstract class BaseStringElement : BaseElement, IEquatable<BaseStringElement>, IComparable<BaseStringElement>
    {
        #region Attributes & Properties
        [DefaultValue("")]
        public virtual string Id { get; set; }


        protected sealed override int InternalId
        {
            get
            {
                return (!String.IsNullOrWhiteSpace(Id)) ? Id.GetHashCode() : String.Empty.GetHashCode();
            }
        }
        #endregion


        #region Constructors & Destructor
        protected BaseStringElement()
        {
            this.Id = String.Empty;
        }

        protected BaseStringElement(string id)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id)); }

            this.Id = id;
        }
        #endregion


        #region Methods
        #endregion


        #region Methods - IEquatable<T>
        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ ((!String.IsNullOrWhiteSpace(Id)) ? Id.GetHashCode() : String.Empty.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseStringElement);
        }

        public bool Equals(BaseStringElement other)
        {
            return other != null && (this.Id == other.Id);
        }
        #endregion


        #region Methods - IComparable<T>
        public int CompareTo(BaseStringElement other)
        {
            return (other is null) ? 1 : String.Compare(Id, other.Id, StringComparison.CurrentCulture);
        }
        #endregion


        #region Operators
        public static bool operator ==(BaseStringElement left, BaseStringElement right)
        {
            return (left is null) ? (right is null) : left.Equals(right);
        }

        public static bool operator !=(BaseStringElement left, BaseStringElement right)
        {
            return !(left == right);
        }

        public static bool operator <(BaseStringElement left, BaseStringElement right)
        {
            return (left is null) ? !(right is null) : left.CompareTo(right) < 0;
        }

        public static bool operator >(BaseStringElement left, BaseStringElement right)
        {
            return !(left is null) && left.CompareTo(right) > 0;
        }
        #endregion
    }
}