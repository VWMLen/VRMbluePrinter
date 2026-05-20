using System;

namespace vrmBluePrinter
{
    public class XmlTagNotFoundException : Exception
    {
        public XmlTagNotFoundException() { }

        public XmlTagNotFoundException(string message) : base(message) { }

        public XmlTagNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
