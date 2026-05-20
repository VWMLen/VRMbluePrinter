using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    public class vModelSelectWindow : MonoBehaviour
    {

        [SerializeField]
        private UIDocument modelSelectWindow;

        [SerializeField]
        private GameObject modelViewer;

        private VisualElement allPanel;
        private VisualElement mainPanel;
        private List<UnityEngine.UIElements.Button> modelList;

        #region モデル設計書選択
        const string panelAll = "modelSelecter";
        const string panelMain = "mainSelecter";
        #endregion

        public modelInfo selectModelInfo;

        const string dummy = "dummy";

        // Start is called before the first frame update
        void Start()
        {
            allPanel = modelSelectWindow.rootVisualElement.Q<VisualElement>(panelAll);
            mainPanel = modelSelectWindow.rootVisualElement.Q<VisualElement>(panelMain);
            mainPanel.Clear();
            selectModelInfo = null;
            allPanel.visible = false;
            mainPanel.visible = false;

        }

        /// <summary>
        /// 設計図モデル一覧表示処理
        /// </summary>
        /// <param name="modelInfos"></param>
        /// <returns></returns>
        public resultInfo settingModelListView(List<modelInfo> modelInfos)
        {
            resultInfo result = new resultInfo();
            List<VisualElement> btns = new List<VisualElement>();

            mainPanel.Clear();

            cModelSelectWindow cModelSelectWindow = new cModelSelectWindow();
            result = cModelSelectWindow.settingModelListView(modelInfos, ref btns);

            if (!common.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
            {
                return result;
            }

            try
            {
                foreach (var item in btns)
                {
                    //モデル選択時イベント設定
                    item.RegisterCallback<MouseDownEvent>(x =>
                    {
                        if (x.button == 0)  // 左クリック
                        {
                            selectModelInfo = getSelectModelInfo(x);
                            mainPanel.visible = false;
                            allPanel.visible = false;
                            common.DebackLog($"モデルが選択されました");
                            common.DebackLog($"selectModelInfo:{selectModelInfo}");
                            new sVrmLoader().setModelInfo(selectModelInfo);
                            //sVrmLoader.modelInfo = selectModelInfo;
                            sVrmLoader.LoadVRMModel(selectModelInfo.Directory);
                            common.setCameraControl(true);
                        }
                        
                    });

                }
                foreach (var btnItem in btns)
                {
                    mainPanel.Add(btnItem);
                }
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
            mainPanel.visible = true;
            allPanel.visible = true;

            return result;
        }

        private modelInfo getSelectModelInfo(MouseDownEvent x)
        {
            common.DebackLog($"x:{x.ToString()}");
            common.DebackLog($"x.currentTarget:{x.currentTarget.ToString()}");
            var target = x.target as VisualElement;
            var match = Regex.Match(target.name, @"(\d{1,2})$");
            int no = int.Parse(match.Value);

            modelInfo result = new modelInfo();
            resultInfo resultInfo = new resultInfo();

            List<modelInfo> xmlList = new List<modelInfo>();
            try
            {
                TextField directory = new TextField();
                directory = modelSelectWindow.rootVisualElement.Q<TextField>($"directory{no}");
                common.DebackLog(directory.ToString());
                //TextField directory = modelSelectWindow.rootVisualElement.Q<TextField>($"directory{count}");
                common.DebackLog($"directory:{directory.value}");
                resultInfo = common.ModelXmlLoad(directory.value, ref xmlList);

                result = xmlList[0];
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref resultInfo, ex, "getSelectModelInfo",Constants.ERRORCODE000, Constants.ERRORTEXT000);
            }   


            return result;
        }
    }
}
