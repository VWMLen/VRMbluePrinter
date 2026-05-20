using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    /// <summary>
    /// 機能モジュール：モデル情報
    /// </summary>
    public class ModelInfoEdit
    {
        #region プロパティ
        public int moduleNo { get; set; }
        public string moduleTitle { get; set; }

        #endregion

        #region 固定設定関数
        /// <summary>
        /// 初期化
        /// </summary>
        public ModelInfoEdit()
        {
            //機能設定
            //表示する選択肢の設定
            moduleNo = 1;
            moduleTitle = "モデル情報";
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
                //モデル情報はプリセット機能のため既存UIを設定する
                mainWindowElement = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainModelInfoWindow);

                //サムネボタン押下時
                UnityEngine.UIElements.Button btnThumbnail = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnThumbnail);
                btnThumbnail.clicked += () =>
                {
                    SubMenuModelInfoSetting(Constants.btnThumbnail, UID);
                };
                //情報ボタン押下時
                UnityEngine.UIElements.Button btnInfomation = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnInfomation);
                btnInfomation.clicked += () =>
                {
                    SubMenuModelInfoSetting(Constants.btnInfomation, UID);
                };
                //人格許容範囲ボタン押下時
                UnityEngine.UIElements.Button btnParsonal = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnParsonal);
                btnParsonal.clicked += () =>
                {
                    SubMenuModelInfoSetting(Constants.btnParsonal, UID);
                };
                //再配布改変許容範囲ボタン押下時
                UnityEngine.UIElements.Button btnRedistMod = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnRedistMod);
                btnRedistMod.clicked += () =>
                {
                    SubMenuModelInfoSetting(Constants.btnRedistMod, UID);
                };

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
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
        /// モデル情報-各サブメニュー表示
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public resultInfo SubMenuModelInfoSetting(string btnName, UIDocument UID)
        {
            resultInfo result = new resultInfo();

            VisualElement subMenu = new VisualElement();
            VisualElement subBlank = new VisualElement();

            UIDocument staticUID = UID;

            subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);

            subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
            if (subMenu.style.display != DisplayStyle.None)
            {
                subMenu.style.display = DisplayStyle.None;
            }
            subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
            if (subMenu.style.display != DisplayStyle.None)
            {
                subMenu.style.display = DisplayStyle.None;
            }
            subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
            if (subMenu.style.display != DisplayStyle.None)
            {
                subMenu.style.display = DisplayStyle.None;
            }
            subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
            if (subMenu.style.display != DisplayStyle.None)
            {
                subMenu.style.display = DisplayStyle.None;
            }

            if (btnName.Equals(Constants.btnThumbnail))
            {
                subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
                //サブメニューサムネ
                VisualElement btnThumbnail = UID.rootVisualElement.Q<VisualElement>(Constants.btnThumFileLoad);
                //設定したアイコン情報を適応する。

                Texture2D iconImage = new Texture2D(2, 2);
                iconImage.LoadImage(modelInfo.modelInfoVRMInfo.ThumbnailData);
                btnThumbnail.style.backgroundImage = iconImage;
                btnThumbnail.RegisterCallback<MouseDownEvent>(x =>
                {
                    if (x.button == 0)  // 左クリック
                    {
                        common.setCameraControl(false);
                        //サムネ
                        setThimbanail(UID);
                        common.setCameraControl(true);
                    }
                });

            }
            else if (btnName.Equals(Constants.btnInfomation))
            {
                subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
                //サブメニューモデル情報
                TextField txtModelInfoTitle = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoTitle);
                TextField txtModelInfoVersion = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoVersion);
                TextField txtModelInfoAuthor = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoAuthor);
                TextField txtModelInfoContactInfo = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoContactInfo);
                TextField txtModelInfoReference = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoReference);

                txtModelInfoTitle.value = modelInfo.modelInfoVRMInfo.Title;
                txtModelInfoVersion.value = modelInfo.modelInfoVRMInfo.Version;
                txtModelInfoAuthor.value = modelInfo.modelInfoVRMInfo.Author;
                txtModelInfoContactInfo.value = modelInfo.modelInfoVRMInfo.ContactInfo;
                txtModelInfoReference.value = modelInfo.modelInfoVRMInfo.Reference;

                txtModelInfoTitle.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.Title = x.newValue;
                });
                txtModelInfoVersion.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.Version = x.newValue;
                });
                txtModelInfoAuthor.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.Author = x.newValue;
                });
                txtModelInfoContactInfo.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.ContactInfo = x.newValue;
                });
                txtModelInfoReference.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.Reference = x.newValue;
                });

            }
            else if (btnName.Equals(Constants.btnParsonal))
            {
                subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
                //サブメニュー人格情報
                DropdownField ddlPpr = UID.rootVisualElement.Q<DropdownField>(Constants.ddlPpr);
                DropdownField ddlVp = UID.rootVisualElement.Q<DropdownField>(Constants.ddlVp);
                DropdownField ddlSp = UID.rootVisualElement.Q<DropdownField>(Constants.ddlSp);
                DropdownField ddlCup = UID.rootVisualElement.Q<DropdownField>(Constants.ddlCup);
                TextField txtOpu = UID.rootVisualElement.Q<TextField>(Constants.txtOpu);

                ddlPpr.index = modelInfo.modelInfoVRMInfo.VRMppr;
                ddlVp.index = modelInfo.modelInfoVRMInfo.VRMvp;
                ddlSp.index = modelInfo.modelInfoVRMInfo.VRMsp;
                ddlCup.index = modelInfo.modelInfoVRMInfo.VRMcup;
                txtOpu.value = modelInfo.modelInfoVRMInfo.VRMopu;

                ddlPpr.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMppr = getAllowedUserUssageLicenseToInt(x.newValue);
                });
                ddlVp.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMvp = getAllowedUserUssageLicenseToInt(x.newValue);
                });
                ddlSp.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMsp = getAllowedUserUssageLicenseToInt(x.newValue);
                });
                ddlCup.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMcup = getAllowedUserUssageLicenseToInt(x.newValue);
                });
                txtOpu.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMopu = x.newValue;
                });

            }
            else if (btnName.Equals(Constants.btnRedistMod))
            {
                subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
                //サブメニュー再配布改変許容範囲情報
                RadioButtonGroup groupLicenseRdo = UID.rootVisualElement.Q<RadioButtonGroup>(Constants.groupLicenseRdo);

                UnityEngine.UIElements.RadioButton rdoRedistributionProhibited = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoRedistributionProhibited);
                UnityEngine.UIElements.RadioButton rdoCC0 = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCC0);
                UnityEngine.UIElements.RadioButton rdoCCBY = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBY);
                UnityEngine.UIElements.RadioButton rdoCCBYNC = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNC);
                UnityEngine.UIElements.RadioButton rdoCCBYSA = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYSA);
                UnityEngine.UIElements.RadioButton rdoCCBYNCSA = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNCSA);
                UnityEngine.UIElements.RadioButton rdoCCBYND = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYND);
                UnityEngine.UIElements.RadioButton rdoCCBYNCND = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNCND);
                UnityEngine.UIElements.RadioButton rdoOther = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoOther);

                List<UnityEngine.UIElements.RadioButton> rdoList = new List<UnityEngine.UIElements.RadioButton>();
                rdoList.Add(rdoRedistributionProhibited);
                rdoList.Add(rdoCC0);
                rdoList.Add(rdoCCBY);
                rdoList.Add(rdoCCBYNC);
                rdoList.Add(rdoCCBYSA);
                rdoList.Add(rdoCCBYNCSA);
                rdoList.Add(rdoCCBYND);
                rdoList.Add(rdoCCBYNCND);
                rdoList.Add(rdoOther);

                rdoList[modelInfo.modelInfoVRMInfo.VRMlt].value = true;

                groupLicenseRdo.RegisterValueChangedCallback(x =>
                {
                    modelInfo.modelInfoVRMInfo.VRMlt = x.newValue;
                });

                //rdoRedistributionProhibited.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 0;
                //});
                //rdoCC0.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 1;
                //});
                //rdoCCBY.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 2;
                //});
                //rdoCCBYNC.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 3;
                //});
                //rdoCCBYSA.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 4;
                //});
                //rdoCCBYNCSA.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 5;
                //});
                //rdoCCBYND.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 6;
                //});
                //rdoCCBYNCND.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 7;
                //});
                //rdoOther.RegisterValueChangedCallback(x =>
                //{
                //    modelInfo.modelInfoVRMInfo.VRMlt = 8;
                //});

            }

            if (subMenu.style.display != DisplayStyle.Flex)
            {
                subMenu.style.display = DisplayStyle.Flex;
            }
            if (subBlank.style.display != DisplayStyle.None)
            {
                subBlank.style.display = DisplayStyle.None;
            }

            return result;
        }

        /// <summary>
        /// モデル情報-サムネイル 反映処理
        /// </summary>
        /// <param name="uID"></param>
        private void setThimbanail(UIDocument uID)
        {
            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);
            VisualElement btnThumbnail = uID.rootVisualElement.Q<VisualElement>(Constants.btnThumFileLoad);

            string ImportFolderPath = UnityEngine.Application.dataPath;
            string filePath = string.Empty;

            filePath = FileDialogForWindows.FileDialog("ファイルを選択", "*.png", "*.jpg");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (filePath.Contains(".png") || filePath.Contains(".jpg"))
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                    Texture2D iconImage = new Texture2D(2, 2);
                    iconImage.LoadImage(imageBytes);
                    btnThumbnail.style.backgroundImage = iconImage;
                    modelInfo.modelInfoVRMInfo.ThumbnailData = imageBytes;
                }
            }

        }

        /// <summary>
        /// AllowedUserUssageLicenseのInt変換
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private int getAllowedUserUssageLicenseToInt(string newValue)
        {
            int returnInt = 0;
            if (newValue.Equals("Only Author"))
            {
                returnInt = 0;
            }
            else if (newValue.Equals("Explicitly Licensed Parson"))
            {
                returnInt = 1;
            }
            else if (newValue.Equals("Everyone"))
            {
                returnInt = 2;
            }
            else if (newValue.Equals("Disallow"))
            {
                returnInt = 0;
            }
            else if (newValue.Equals("Allow"))
            {
                returnInt = 1;
            }

            return returnInt;
        }
        #endregion

    }
}
