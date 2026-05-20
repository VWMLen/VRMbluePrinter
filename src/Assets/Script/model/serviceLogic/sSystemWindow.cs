using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    public class sSystemWindow
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
        public resultInfo SettingMessage(ref systemWindowInfo systemWindowInfo,ref Label labelTitle,ref Label labelMessage,ref UnityEngine.UIElements.Button btnOK,ref UnityEngine.UIElements.Button btnCancel)
        {
            resultInfo resultInfo = new resultInfo();

            try
            {
                if (!string.IsNullOrEmpty(systemWindowInfo.title))
                {
                    labelTitle.text = systemWindowInfo.title;
                }

                if (!string.IsNullOrEmpty(systemWindowInfo.message))
                {
                    labelMessage.text = systemWindowInfo.message;
                }
                else
                {
                    labelMessage.text = string.Empty;
                }

                if (!string.IsNullOrEmpty(systemWindowInfo.btnOK))
                {
                    btnOK.text = systemWindowInfo.btnOK;
                }

                if (!string.IsNullOrEmpty(systemWindowInfo.btnCancel))
                {
                    btnCancel.text = systemWindowInfo.btnCancel;
                    btnCancel.visible = true;
                }
                else
                {
                    btnCancel.visible = false;
                }

                resultInfo.resultCode = Constants.SUCCESSCODE000;
                resultInfo.resultText = Constants.SUCCESSTEXT000;

            }
            catch (Exception ex)
            {
                resultInfo.resultCode = Constants.ERRORCODE000;
                resultInfo.resultText = common.ResultTextReplace(Constants.ERRORTEXT000, ex.Message);
            }


            return resultInfo;
        }
    }
}
