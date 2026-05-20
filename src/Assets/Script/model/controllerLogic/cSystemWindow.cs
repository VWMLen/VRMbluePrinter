using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    public class cSystemWindow
    {
        /// <summary>
        /// メッセージウィンドウ設定
        /// </summary>
        /// <param name="systemWindowInfo"></param>
        /// <param name="labelTitle"></param>
        /// <param name="labelMessage"></param>
        /// <param name="btnOK"></param>
        /// <param name="btnCancel"></param>
        /// <returns></returns>
        public resultInfo SettingMessage(ref systemWindowInfo systemWindowInfo,ref Label labelTitle,ref Label labelMessage,ref Button btnOK,ref Button btnCancel)
        {
            resultInfo resultInfo = new resultInfo();

            sSystemWindow sSystemWindow = new sSystemWindow();
            resultInfo = sSystemWindow.SettingMessage(ref systemWindowInfo,ref labelTitle,ref labelMessage,ref btnOK,ref btnCancel);

            return resultInfo;
        }
    }
}
