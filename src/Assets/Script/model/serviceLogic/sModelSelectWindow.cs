using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    public class sModelSelectWindow : MonoBehaviour
    {

        /// <summary>
        /// 設計図モデル一覧表示
        /// </summary>
        /// <param name="modelInfos"></param>
        /// <param name="btns"></param>
        /// <returns></returns>
        public resultInfo settingModelListView(List<modelInfo> modelInfos, ref List<VisualElement> btns)
        {
            resultInfo result = new resultInfo();
            try
            {
                btns = settingButtonModel(modelInfos);

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
                    , ex
                    , "settingModelListView"
                    , Constants.ERRORCODE000
                    , Constants.ERRORTEXT000
                );
            }

            return result;
        }

        /// <summary>
        /// 設計図モデルボタン設定
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        private List<VisualElement> settingButtonModel(List<modelInfo> modelInfo)
        {
            int count = 1;
            List<VisualElement> returnSettingButtonModel = new List<VisualElement>();
            foreach (modelInfo modelInfoItem in modelInfo)
            {
                //UnityEngine.UIElements.Button btn = new UnityEngine.UIElements.Button();
                VisualElement btn = new VisualElement();
                string btnName = $"btn{count}";
                btn.name = btnName;
                Texture2D image = new Texture2D(400, 400);
                image.LoadImage(modelInfoItem.modelInfoVRMInfo.ThumbnailData);
                btn.style.backgroundImage = image;
                btn.style.width = 400;
                btn.style.height = 400;
                btn.style.marginTop = 20;
                btn.style.marginBottom = 2;
                btn.style.marginLeft = 60;
                btn.style.marginRight = 4;


                Label lbl = new Label();
                lbl.text = modelInfoItem.Name;
                lbl.style.backgroundColor = Color.white;
                lbl.style.marginTop = 327;
                lbl.style.marginRight = 30;
                lbl.style.marginLeft = 30;

                TextField directory = new TextField();
                directory.name = $"directory{count}";
                common.DebackLog(directory.name);
                directory.value = $"{modelInfoItem.Directory}";
                directory.style.display = DisplayStyle.None;

                btn.Add(lbl);
                btn.Add(directory);

                returnSettingButtonModel.Add(btn);

                count++;
            }

            return returnSettingButtonModel;
        }
    }
}
