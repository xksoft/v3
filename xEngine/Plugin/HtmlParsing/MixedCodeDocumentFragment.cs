// HtmlParsing V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

namespace xEngine.Plugin.HtmlParsing
{
    /// <summary>
    ///     Represents a base class for fragments in a mixed code document.
    /// </summary>
    public abstract class MixedCodeDocumentFragment
    {
        #region Constructors

        internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
        {
            Doc = doc;
            Type = type;
            switch (type)
            {
                case MixedCodeDocumentFragmentType.Text:
                    Doc.Textfragments.Append(this);
                    break;

                case MixedCodeDocumentFragmentType.Code:
                    Doc.Codefragments.Append(this);
                    break;
            }
            Doc._fragments.Append(this);
        }

        #endregion

        #region Fields

        internal MixedCodeDocument Doc;
        internal int Index;
        internal int Length;
        internal int Lineposition;
        internal MixedCodeDocumentFragmentType Type;
        private string _fragmentText;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the fragement text.
        /// </summary>
        public string FragmentText
        {
            get
            {
                if (_fragmentText == null)
                {
                    _fragmentText = Doc.Text.Substring(Index, Length);
                }
                return FragmentText;
            }
            internal set { _fragmentText = value; }
        }

        /// <summary>
        ///     Gets the type of fragment.
        /// </summary>
        public MixedCodeDocumentFragmentType FragmentType
        {
            get { return Type; }
        }

        /// <summary>
        ///     Gets the line number of the fragment.
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        ///     Gets the line position (column) of the fragment.
        /// </summary>
        public int LinePosition
        {
            get { return Lineposition; }
        }

        /// <summary>
        ///     Gets the fragment position in the document's stream.
        /// </summary>
        public int StreamPosition
        {
            get { return Index; }
        }

        #endregion
    }
}