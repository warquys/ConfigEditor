using ConfigEditor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigEditor.Elements
{
    public class CompletorValue : BaseUintElement, IConvertible
    {
        [ECSDisplayColumn("Value", 1, 20)]
        [XmlElement("Value")]
        public string Value { get; set; }

        [ECSDisplayColumn("Help", 1, 20)]
        [XmlElement("Help")]
        public string Help { get; set; }
        public CompletorValue()
        {

        }

        public CompletorValue(string value)
        {
            Id = SymlEditorConfig.Singleton.GetCompletorValueId();
            Value = value;
        }

        public CompletorValue(string value, string help)
        {
            Id = SymlEditorConfig.Singleton.GetCompletorValueId();
            Value = value;
            Help = help;    
        }
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Help))
                return Value;
            else
                return $"{Value} ({Help})";
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            return Value;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
