using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    /// <summary>
    /// システムウィンドウ情報
    /// </summary>
    public class systemWindowInfo
    {
        /// <summary>
        /// ウィンドウタイトル
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// ウィンドウメッセージ
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// ウィンドウボタンOKテキスト
        /// </summary>
        public string btnOK { get; set; }
        /// <summary>
        /// ウィンドウボタンCancelテキスト
        /// </summary>
        public string btnCancel { get; set; }
        /// <summary>
        /// ウィンドウボタン選択結果コード
        /// </summary>
        public int butonResultCode { get; set; }

        public systemWindowInfo()
        {
            title = string.Empty;
            message = string.Empty;
            btnOK = Constants.btnOKText;
            btnCancel = Constants.btnCancelText;
            butonResultCode = 0;

        }
    }

}
