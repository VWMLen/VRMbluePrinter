using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    /// <summary>
    /// 汎用機能Constoller
    /// </summary>
    internal class controllerGeneralPurpose
    {
        public controllerGeneralPurpose()
        {
        }

        /// <summary>
        /// 機能選択コントローラー
        /// </summary>
        /// <param name="ddlEvt"></param>
        /// <returns></returns>
        public resultInfo DllMenu(string ddlEvtValue, ref UnityEngine.UIElements.VisualElement mainMenu, ref UIDocument UID, List<moduleList> moduleLists)
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();
            UnityEngine.UIElements.VisualElement preMainMenu = mainMenu;

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            returnResult = sVrmLoader.getModelInfo(ref modelInfo);
            //モデルデータが存在しない場合、異常終了で処理終了
            if (!common.ReferenceEquals(returnResult.resultCode, Constants.SUCCESSCODE000))
            {
                returnResult.resultCode = Constants.ERRORCODE301;
                returnResult.resultText = Constants.ERRORTEXT301;
                return returnResult;
            }

            foreach (moduleList moduleItem in moduleLists)
            {
                if (ddlEvtValue.Equals(moduleItem.moduleTitle))
                {
                    var module = moduleItem.moduleClass;

                    Type moduleType = module.GetType();

                    // メソッドを取得
                    var methodInfo = moduleType.GetMethod("mainWindowSetting");
                    if (methodInfo != null)
                    {
                        // 引数を準備
                        object[] parameters = { UID, mainMenu };

                        // メソッドを呼び出す
                        var returnValue = methodInfo.Invoke(module, parameters);
                        mainMenu = (VisualElement)parameters[1];
                        returnResult = (resultInfo)returnValue;
                    }

                }
            }

            #region 削除予定
            //switch (ddlEvtValue)
            //{
            //    //モデル情報
            //    case Constants.ddlGpMenu01:

            //        //common.DebackLog($" is {Constants.ddlGpMenu01}");
            //        //returnResult = serviceGeneralPurpose.ddlModelInfomationPanel(modelInfo,ref mainMenu,ref UID);
            //        break;
            //    //カラー
            //    case Constants.ddlGpMenu02:
            //        common.DebackLog($" is {Constants.ddlGpMenu02}");
            //        returnResult = serviceGeneralPurpose.DdlModelColorPanel(modelInfo,ref mainMenu,UID);
            //        break;
            //    //表情
            //    case Constants.ddlGpMenu03:
            //        common.DebackLog($" is {Constants.ddlGpMenu03}");
            //        returnResult = serviceGeneralPurpose.DdlModelBlendShapePanel(modelInfo, ref mainMenu,UID);
            //        break;
            //    //アイコン
            //    case Constants.ddlGpMenu04:
            //        common.DebackLog($" is {Constants.ddlGpMenu04}");
            //        returnResult = serviceGeneralPurpose.DdlModelIconPanel(modelInfo, ref mainMenu, UID);
            //        break;
            //    default:
            //        common.DebackLog("other");
            //        break;
            //}
            #endregion

            if (common.ReferenceEquals(returnResult.resultCode, Constants.SUCCESSCODE000))
            {
                //preMainMenu.visible = false;
                //mainMenu.visible = true;
                //ve.style.display = DisplayStyle.Flex;   // 表示
                //ve.style.display = DisplayStyle.None;   // 非表示
                preMainMenu.style.display = DisplayStyle.None;   // 表示
                mainMenu.style.display = DisplayStyle.Flex;   // 非表示

            }


            return returnResult;
        }

        /// <summary>
        /// モデル選択押下時処理
        /// </summary>
        /// <returns></returns>
        public resultInfo ModelSelect(ref List<modelInfo> models)
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();

            returnResult = serviceGeneralPurpose.ModelSelect(ref models);

            return returnResult;
        }

        public resultInfo ModelExport()
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();

            returnResult = serviceGeneralPurpose.ModelExport();

            return returnResult;
        }

        /// <summary>
        /// モデルプレビューモーション選択
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="playAnime"></param>
        /// <returns></returns>
        public resultInfo MotionMenuSetting(string newValue, vVrmLoader vVrmLoader, ref RuntimeAnimatorController playAnime)
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();

            returnResult = serviceGeneralPurpose.MotionMenuSetting(newValue, vVrmLoader, ref playAnime);

            return returnResult;
        }

        public resultInfo IniSave()
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();

            returnResult = serviceGeneralPurpose.IniSave();

            return returnResult;
        }

        public resultInfo IniLoad()
        {
            resultInfo returnResult = new resultInfo();
            serviceGeneralPurpose serviceGeneralPurpose = new serviceGeneralPurpose();

            returnResult = serviceGeneralPurpose.IniLoad();

            return returnResult;
        }
    }
}
