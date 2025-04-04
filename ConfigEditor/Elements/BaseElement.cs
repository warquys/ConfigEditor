using System;


namespace ConfigEditor.Elements
{
	public abstract class BaseElement : IEquatable<BaseElement>, IComparable<BaseElement>, IComparable
	{
		#region Attributes

		private readonly int _hashType;

		protected abstract int InternalId { get; }

		#endregion Attributes


		#region Constructors

		protected BaseElement()
		{
			_hashType = this.GetType().GetHashCode();
		}

		#endregion Constructors


		#region Methods

		public override bool Equals(object obj)
		{
			return Equals(obj as BaseElement);
		}

		public bool Equals(BaseElement other)
		{
			return other != null && (this.InternalId == other.InternalId);
		}

		public override int GetHashCode()
		{
			return _hashType ^ InternalId;
		}

		#endregion Methods

		/*
		public virtual int CompareTo(object obj)
		{
			return CompareTo(obj as BaseElement);
		}

		public int CompareTo(BaseElement obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return this.ToString().CompareTo(obj.ToString());
		}
		*/


		#region Methods - IComparable<T>

		public int CompareTo(object other)
		{
			dynamic obj = other;
			return (other != null && other is BaseElement) ? CompareTo(obj) : 1;
		}

		public int CompareTo(BaseElement other)
		{
			return (other is null) ? 1 : ToString().CompareTo(other.ToString());
		}
        #endregion Methods - IComparable<T>


        #region Operators

        public static bool operator ==(BaseElement left, BaseElement right)
		{
			dynamic objRight = right;
			dynamic objleft = left;

			return (objleft is null) ? (objRight is null) : objleft.Equals(objRight);
		}

		public static bool operator !=(BaseElement left, BaseElement right)
		{
			return !(left == right);
		}

		public static bool operator <(BaseElement left, BaseElement right)
		{
			dynamic objRight = right;
			dynamic objleft = left;
			return (objleft is null) ? !(objleft is null) : objleft.CompareTo(objRight) < 0;
		}

		public static bool operator >(BaseElement left, BaseElement right)
		{
			dynamic objRight = right;
			dynamic objleft = left;
			return !(objleft is null) && objleft.CompareTo(objRight) > 0;
		}

		#endregion Operators

	}
}