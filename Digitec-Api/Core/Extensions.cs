using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitec_Api.Core
{
    internal static class Extensions
    {
        internal static HtmlNode TryFirst(this IEnumerable<HtmlNode> list)
        {
            try
            {
                return list.First();
            }
            catch
            {
                var _virtualDocument = new HtmlDocument();
                return _virtualDocument.CreateElement("dmy");
            }
        }
    }
}
