using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WarBender {
    internal static class XmlExtensions {
        public static Exception InvalidXml(this XObject obj, string message) {
            var baseUri = obj.Document?.BaseUri;
            if (baseUri != null && Uri.TryCreate(baseUri, UriKind.RelativeOrAbsolute, out var uri)) {
                message += " in " + (uri.IsFile ? uri.LocalPath : uri.ToString());
            }

            var lineInfo = (IXmlLineInfo)obj;
            if (lineInfo.HasLineInfo()) {
                message += " at line " + lineInfo.LineNumber;
                message += ", column " + lineInfo.LinePosition;
            }

            return new InvalidDataException(message);
        }
    }
}
