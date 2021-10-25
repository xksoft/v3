// HtmlParsing V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

#region

using System;
using System.Diagnostics;

#endregion

namespace xEngine.Plugin.HtmlParsing
{
    /// <summary>
    ///     Represents an HTML attribute.
    /// </summary>
    [DebuggerDisplay("Name: {OriginalName}, Value: {Value}")]
    public class HtmlAttribute : IComparable
    {
        #region Constructors

        internal HtmlAttribute(HtmlDocument ownerdocument)
        {
            Ownerdocument = ownerdocument;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        ///     Compares the current instance with another attribute. Comparison is based on attributes' name.
        /// </summary>
        /// <param name="obj">An attribute to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the names comparison.</returns>
        public int CompareTo(object obj)
        {
            var att = obj as HtmlAttribute;
            if (att == null)
            {
                throw new ArgumentException("obj");
            }
            return Name.CompareTo(att.Name);
        }

        #endregion

        #region Private Methods

        private string GetRelativeXpath()
        {
            if (OwnerNode == null)
                return Name;

            var i = 1;
            foreach (var node in OwnerNode.Attributes)
            {
                if (node.Name != Name) continue;

                if (node == this)
                    break;

                i++;
            }
            return "@" + Name + "[" + i + "]";
        }

        #endregion

        #region Fields

        internal int Lineposition;
        internal int Namelength;
        internal int Namestartindex;
        internal HtmlDocument Ownerdocument; // attribute can exists without a node
        internal HtmlNode Ownernode;
        internal int Streamposition;
        internal int Valuelength;
        internal int Valuestartindex;
        internal string _name;
        private AttributeValueQuote _quoteType = AttributeValueQuote.DoubleQuote;
        internal string _value;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the line number of this attribute in the document.
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        ///     Gets the column number of this attribute in the document.
        /// </summary>
        public int LinePosition
        {
            get { return Lineposition; }
        }

        /// <summary>
        ///     Gets the qualified name of the attribute.
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = Ownerdocument.Text.Substring(Namestartindex, Namelength);
                }
                return _name.ToLower();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _name = value;
                if (Ownernode != null)
                {
                    Ownernode.Innerchanged = true;
                    Ownernode.Outerchanged = true;
                }
            }
        }

        /// <summary>
        ///     Name of attribute with original case
        /// </summary>
        public string OriginalName
        {
            get { return _name; }
        }

        /// <summary>
        ///     Gets the HTML document to which this attribute belongs.
        /// </summary>
        public HtmlDocument OwnerDocument
        {
            get { return Ownerdocument; }
        }

        /// <summary>
        ///     Gets the HTML node to which this attribute belongs.
        /// </summary>
        public HtmlNode OwnerNode
        {
            get { return Ownernode; }
        }

        /// <summary>
        ///     Specifies what type of quote the data should be wrapped in
        /// </summary>
        public AttributeValueQuote QuoteType
        {
            get { return _quoteType; }
            set { _quoteType = value; }
        }

        /// <summary>
        ///     Gets the stream position of this attribute in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition
        {
            get { return Streamposition; }
        }

        /// <summary>
        ///     Gets or sets the value of the attribute.
        /// </summary>
        public string Value
        {
            get
            {
                if (_value == null)
                {
                    _value = Ownerdocument.Text.Substring(Valuestartindex, Valuelength);
                }
                return _value;
            }
            set
            {
                _value = value;
                if (Ownernode != null)
                {
                    Ownernode.Innerchanged = true;
                    Ownernode.Outerchanged = true;
                }
            }
        }

        internal string XmlName
        {
            get { return HtmlDocument.GetXmlName(Name); }
        }

        internal string XmlValue
        {
            get { return Value; }
        }

        /// <summary>
        ///     Gets a valid XPath string that points to this Attribute
        /// </summary>
        public string XPath
        {
            get
            {
                var basePath = (OwnerNode == null) ? "/" : OwnerNode.XPath + "/";
                return basePath + GetRelativeXpath();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a duplicate of this attribute.
        /// </summary>
        /// <returns>The cloned attribute.</returns>
        public HtmlAttribute Clone()
        {
            var att = new HtmlAttribute(Ownerdocument);
            att.Name = Name;
            att.Value = Value;
            return att;
        }

        /// <summary>
        ///     Removes this attribute from it's parents collection
        /// </summary>
        public void Remove()
        {
            Ownernode.Attributes.Remove(this);
        }

        #endregion
    }

    /// <summary>
    ///     An Enum representing different types of Quotes used for surrounding attribute values
    /// </summary>
    public enum AttributeValueQuote
    {
        /// <summary>
        ///     A single quote mark '
        /// </summary>
        SingleQuote,

        /// <summary>
        ///     A double quote mark "
        /// </summary>
        DoubleQuote
    }
}