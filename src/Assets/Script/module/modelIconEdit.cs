using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    /// <summary>
    /// 機能モジュール：アイコン情報
    /// </summary>
    public class ModelIconEdit
    {
        #region プロパティ
        public int moduleNo { get; set; }
        public string moduleTitle { get; set; }

        #endregion

        #region 固定設定関数
        /// <summary>
        /// 初期化
        /// </summary>
        public ModelIconEdit()
        {
            //機能設定
            //表示する選択肢の設定
            moduleNo = 3;
            moduleTitle = "アイコン";
        }

        /// <summary>
        /// メインウィンドウ表示設定
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="mainWindowElement"></param>
        /// <returns></returns>
        public resultInfo mainWindowSetting(UIDocument UID,ref VisualElement mainWindowElement)
        {
            //メインウィンドウの表示内容設定
            resultInfo result = new resultInfo();

            try
            {
                modelInfo modelInfo = new modelInfo();
                vVrmLoader loader = new vVrmLoader();
                loader.getModelInfo(ref modelInfo);
                result = DdlModelIconPanel(modelInfo,ref mainWindowElement, UID);
            }
            catch (System.Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , $"mainWindowSetting no:{moduleNo} title:{moduleTitle}"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }


            return result;
        }

        //        /// <summary>
        //        /// サブウィンドウ表示設定
        //        /// </summary>
        //        /// <param name="UID"></param>
        //        /// <param name="subWindowElement"></param>
        //        /// <returns></returns>
        //        public resultInfo subWindowSetting(UIDocument UID, ref VisualElement subWindowElement)
        //        {
        //            //サブウィンドウの表示内容設定
        //            resultInfo result = new resultInfo();

        //            try
        //            {
        //                //サブウィンドウはメインウィンドウでのボタン押下で生成するため
        //                //ここでは処理を記述しない。

        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;
        //            }
        //            catch (System.Exception ex)
        //            {
        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , $"subWindowSetting no:{moduleNo} title:{moduleTitle}"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }

        //            return result;
        //        }

        #endregion

        #region 独自機能関数
        /// <summary>
        /// アイコン時、メインメニュー設定処理
        /// </summary>
        /// <returns></returns>
        public resultInfo DdlModelIconPanel(modelInfo modelInfo, ref VisualElement mainMenu, UIDocument UID)
        {
            resultInfo result = new resultInfo();
            try
            {
                //アタッチ
                mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainIconWindow);
                VisualElement subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
                VisualElement subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubIconWindows);
                if (subMenu.style.display != DisplayStyle.Flex)
                {
                    subMenu.style.display = DisplayStyle.Flex;
                }
                if (subBlank.style.display != DisplayStyle.None)
                {
                    subBlank.style.display = DisplayStyle.None;
                }

                VisualElement btnIconSet = UID.rootVisualElement.Q<VisualElement>(Constants.btnIconSet);
                UnityEngine.UIElements.Button btnIconLoad = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnIconLoad);
                UnityEngine.UIElements.Slider sliIconX = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconX);
                UnityEngine.UIElements.Slider sliIconY = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconY);
                UnityEngine.UIElements.Button btnIconApply = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnIconApply);

                //メインメニュー
                //アイコン読込設定
                setIconImage(UID, modelInfo.IconData);

                //サブメニュー
                //ロードボタン設定
                //ロードボタンのイベントリスナー登録
                btnIconLoad.clicked += () =>
                {
                    common.setCameraControl(false);
                    setIconTexter(UID);
                    common.setCameraControl(true);
                };
                ////スライダーX設定
                ////スライダーY設定
                //// スライダーの変更イベントにリスナーを登録
                //sliIconX.RegisterValueChangedCallback(OnColorIconSliderValueChanged);
                //sliIconY.RegisterValueChangedCallback(OnColorIconSliderValueChanged);
                //適用ボタン設定
                btnIconApply.clicked += () =>
                {
                    setIconApply(UID);
                };


                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "DdlModelIconPanel"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;

        }

        /// <summary>
        /// 画像をメインメニューへ反映
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="imageByte"></param>
        private void setIconImage(UIDocument uID, byte[] imageByte)
        {
            VisualElement btnIconSet = uID.rootVisualElement.Q<VisualElement>(Constants.btnIconSet);

            //メインメニュー
            //アイコン読込設定
            var iconImage = new Texture2D(2, 2);
            iconImage.LoadImage(imageByte);
            btnIconSet.style.backgroundImage = iconImage;
        }

        private resultInfo setIconTexter(UIDocument uID)
        {
            resultInfo resultInfo = new resultInfo();
            string ImportFolderPath = UnityEngine.Application.dataPath;
            string filePath = string.Empty;
            sVrmLoader sVrmLoader = new sVrmLoader();
            GameObject mainGameObject = null;
            sVrmLoader.getVrmGameObject(ref mainGameObject);
            //sVrmLoader.setCameraMove(ref mainGameObject,false);
            //sVrmLoader.setVrmGameObject(mainGameObject);
            try
            {
                filePath = FileDialogForWindows.FileDialog("ファイルを選択", "*.png", "*.jpg");

                if (!string.IsNullOrEmpty(filePath))
                {
                    if (filePath.Contains(".png") || filePath.Contains(".jpg"))
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                        setIconImage(uID, imageBytes);
                        resultInfo.resultCode = Constants.SUCCESSCODE000;
                        resultInfo.resultText = Constants.SUCCESSTEXT000;

                        modelInfo modelInfo = new modelInfo();

                        sVrmLoader.getModelInfo(ref modelInfo);
                        modelInfo.IconData = imageBytes;
                    }
                }

            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref resultInfo
    , ex
    , "setIconTexter"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }
            finally
            {
                //sVrmLoader.setCameraMove(ref mainGameObject, true);
                //sVrmLoader.setVrmGameObject(mainGameObject);
            }

            return resultInfo;
        }

        /// <summary>
        /// アイコン適用
        /// </summary>
        /// <param name="uID"></param>
        private void setIconApply(UIDocument uID)
        {
            //設定したアイコン情報を適応する。
            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);

            UnityEngine.UIElements.Slider sliIconX = uID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconX);
            UnityEngine.UIElements.Slider sliIconY = uID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconY);
            modelInfo.modelInfoIconScroll.IconScrollX = (int)sliIconX.value;
            modelInfo.modelInfoIconScroll.IconScrollY = (int)sliIconY.value;

            foreach (modelInfoSettingMesh mesh in modelInfo.modelInfoSettingMesh)
            {
                foreach (modelInfoSettingMaterial material in mesh.Materials)
                {
                    if (material.MaterialIcon == 1)
                    {
                        GameObject target;
                        target = GameObject.Find($"modelView/VRM/{mesh.MeshName}");

                        foreach (Material materialTarget in target.GetComponent<Renderer>().materials)
                        {
                            if (materialTarget.name.Contains(material.MaterialName))
                            {
                                //Destroy(materialTarget.mainTexture);
                                Texture2D iconImage = new Texture2D(2, 2);
                                iconImage.LoadImage(modelInfo.IconData);
                                materialTarget.SetTexture("_MainTex", iconImage);

                                materialTarget.SetFloat("_UvAnimScrollX", modelInfo.modelInfoIconScroll.IconScrollX);
                                materialTarget.SetFloat("_UvAnimScrollY", modelInfo.modelInfoIconScroll.IconScrollY);

                                materialTarget.SetFloat("_UvAnimScrollX", modelInfo.modelInfoIconScroll.IconScrollX);
                                materialTarget.SetFloat("_UvAnimScrollY", modelInfo.modelInfoIconScroll.IconScrollY);

                                //materialTarget.SetTextureScale("_MainTex", new Vector2(1f, -1f));
                                //_faceMaterial.SetColor("_EmissionColor", color4);

                            }
                        }
                    }
                }
            }
        }

        #endregion

    }
}
