// HtmlParsing V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Text;

namespace xEngine.Plugin.HtmlParsing
{
    internal class EncodingFoundException : Exception
    {
        #region Constructors

        internal EncodingFoundException(Encoding encoding)
        {
            Encoding = encoding;
        }

        #endregion

        #region Properties

        internal Encoding Encoding { get; private set; }

        #endregion

        #region Fields

        #endregion
    }
}