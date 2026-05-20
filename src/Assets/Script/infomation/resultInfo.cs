using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace vrmBluePrinter
{
    /// <summary>
    /// リザルト情報
    /// </summary>
    public class resultInfo
    {
        /// <summary>
        /// リザルトコード
        /// </summary>
        public string resultCode { get; set; }

        /// <summary>
        /// リザルトテキスト
        /// </summary>
        public string resultText { get; set; }

        public resultInfo()
        {
            resultCode = string.Empty;
            resultText = string.Empty;
        }
    }

}
