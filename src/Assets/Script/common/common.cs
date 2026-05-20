using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using UniGLTF.MeshUtility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VRM;

namespace vrmBluePrinter
{
    public class common
    {
        #region XML読込
        /// <summary>
        /// 設計図XML読込
        /// </summary>
        /// <param name="XMLDirectory"></param>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public static resultInfo ModelXmlLoad(string XMLDirectory, ref List<modelInfo> modelInfo)
        {
            resultInfo result = new resultInfo();

            try
            {
                //1.引数XMLディレクトリを元にXMLファイルを読み込む
                XDocument doc = XDocument.Load(XMLDirectory);

                string fileName = Path.GetFileName(XMLDirectory);
                string directory = XMLDirectory.Replace(fileName, "");

                //2.読み込んだXMLファイルを元に設定する。
                modelInfo modelSetting = new modelInfo();
                List<modelInfoColor> modelColorSettings = new List<modelInfoColor>();
                modelInfoIconScroll modelIconScrollSetting = new modelInfoIconScroll();
                modelInfoVRMInfo modelInfoVRMInfoSetting = new modelInfoVRMInfo();
                List<modelInfoSettingMesh> modelInfoSettingMeshes = new List<modelInfoSettingMesh>();


                //モデル情報
                List<XElement> xmlSetting = new List<XElement>();
                var xmlVrm = doc.Root.Element(Constants.XML_VRM);
                modelSetting.Name = GetElementValue(xmlVrm, Constants.XML_VRM_name);
                modelSetting.Icon = GetElementValue(xmlVrm, Constants.XML_VRM_icon);
                modelSetting.IconData = LoadPictureData($"{directory}\\{GetElementValue(xmlVrm, Constants.XML_VRM_icon)}");
                modelSetting.Directory = XMLDirectory;

                modelSetting.VrmDirectory = $"{directory}{modelSetting.Name}.vrm";

                //カラー設定
                foreach (var color in xmlVrm.Element(Constants.XML_VRM_Colors).Elements(Constants.XML_VRM_Colors_Color))
                {
                    modelInfoColor modelColorSetting = new modelInfoColor();
                    modelColorSetting.Color = GetElementValue(color, Constants.XML_VRM_Colors_Color_color);
                    bool emission = false;
                    if (!bool.TryParse(GetElementValue(color, Constants.XML_VRM_Colors_Color_emission), out emission))
                    {
                        throw new XmlTagNotFoundException($"'{Constants.XML_VRM_Colors_Color_emission}' 有効な内容が設定されていません。。");
                    }
                    modelColorSetting.Emission = emission;

                    modelColorSettings.Add(modelColorSetting);
                }
                modelSetting.modelInfoColor = modelColorSettings;

                //アイコンスクロール設定
                var xmlIconScrollSetting = xmlVrm.Element(Constants.XML_IconScroll);
                modelIconScrollSetting.IconScrollX = GetElementValueAsInt(xmlIconScrollSetting, Constants.XML_IconScroll_iconScrollX);
                modelIconScrollSetting.IconScrollY = GetElementValueAsInt(xmlIconScrollSetting, Constants.XML_IconScroll_iconScrollY);
                modelSetting.modelInfoIconScroll = modelIconScrollSetting;

                //VRM設定
                var xmlVRMSettings = xmlVrm.Element(Constants.XML_VRMSettings);
                modelInfoVRMInfoSetting.Thumbnail = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMthumbnail);
                modelInfoVRMInfoSetting.ThumbnailData = LoadPictureData($"{directory}\\{GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMthumbnail)}");
                modelInfoVRMInfoSetting.Title = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMtitle);
                modelInfoVRMInfoSetting.Version = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMversion);
                modelInfoVRMInfoSetting.Author = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMauthor);
                modelInfoVRMInfoSetting.ContactInfo = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMcontactInfo);
                modelInfoVRMInfoSetting.Reference = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMreference);
                modelInfoVRMInfoSetting.VRMppr = GetElementValueAsInt(xmlVRMSettings, Constants.XML_VRMSettings_VRMpp);
                modelInfoVRMInfoSetting.VRMvp = GetElementValueAsInt(xmlVRMSettings, Constants.XML_VRMSettings_VRMpd);
                modelInfoVRMInfoSetting.VRMsp = GetElementValueAsInt(xmlVRMSettings, Constants.XML_VRMSettings_VRMsp);
                modelInfoVRMInfoSetting.VRMcup = GetElementValueAsInt(xmlVRMSettings, Constants.XML_VRMSettings_VRMcup);
                modelInfoVRMInfoSetting.VRMopu = GetElementValue(xmlVRMSettings, Constants.XML_VRMSettings_VRMopu);
                modelInfoVRMInfoSetting.VRMlt = GetElementValueAsInt(xmlVRMSettings, Constants.XML_VRMSettings_VRMit);
                modelSetting.modelInfoVRMInfo = modelInfoVRMInfoSetting;

                //メッシュ設定
                foreach (var meshe in xmlVrm.Element(Constants.XML_Meshes).Elements(Constants.XML_Meshes_Mesh))
                {
                    modelInfoSettingMesh meshSetting = new modelInfoSettingMesh();
                    meshSetting.MeshName = GetElementValue(meshe, Constants.XML_Meshes_Mesh_MeshName);
                    meshSetting.Materials = new List<modelInfoSettingMaterial>();
                    var xmlMaterials = meshe.Element(Constants.XML_Meshes_Mesh_Materials);

                    foreach (var materialDetail in xmlMaterials.Elements(Constants.XML_Meshes_Mesh_Materials_Material))
                    {
                        DebackLog(materialDetail.ToString());
                        modelInfoSettingMaterial materials = new modelInfoSettingMaterial();
                        materials.MaterialName = GetElementValue(materialDetail, Constants.XML_Meshes_Mesh_Materials_Material_MaterialName);
                        materials.MaterialIcon = GetElementValueAsInt(materialDetail, Constants.XML_Meshes_Mesh_Materials_Material_MaterialIcon);
                        meshSetting.Materials.Add(materials);
                    }

                    modelInfoSettingMeshes.Add(meshSetting);
                }
                modelSetting.modelInfoSettingMesh = modelInfoSettingMeshes;

                //3,引数に設定する。
                modelInfo.Add(modelSetting);

                //4.正常終了を設定、返却する。
                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;

            }
            catch (FileNotFoundException fnfEx)
            {
                //設計図XMLが読み込めなかった場合
                common.ErrorResultSetting(ref result
, fnfEx
, "ModelXmlLoad"
, Constants.ERRPRCODE100
, Constants.ERRORTEXT100
);
            }
            catch (XmlTagNotFoundException xtnfe)
            {
                //設計図XML項目が読み込めなかった場合
                common.ErrorResultSetting(ref result
, xtnfe
, "ModelXmlLoad"
, Constants.ERRORCODE101
, Constants.ERRORTEXT101
);
            }
            catch (Exception ex)
            {
                //上記以外異常が発生した場合
                common.ErrorResultSetting(ref result
, ex
, "ModelXmlLoad"
, Constants.ERRORCODE000
, Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// 設計図XML読込
        /// </summary>
        /// <param name="XMLDirectory"></param>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public static resultInfo ModelXmlLoad(string XMLDirectory, ref modelInfo modelInfo)
        {
            resultInfo result = new resultInfo();
            List<modelInfo> listModelInfo = new List<modelInfo>();
            result = ModelXmlLoad(XMLDirectory, ref listModelInfo);

            modelInfo = listModelInfo[0];

            return result;
        }

        /// <summary>
        /// XMLエレメント値取得
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        /// <exception cref="XmlTagNotFoundException"></exception>
        private static string GetElementValue(XElement parent, string elementName)
        {
            var element = parent?.Element(elementName);
            if (element == null)
            {
                throw new XmlTagNotFoundException($"'{elementName}' タグが存在しません。");
            }
            return element.Value; // タグが存在する場合の値を返す
        }

        /// <summary>
        /// XMLエレメント値取得数字版
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        /// <exception cref="XmlTagNotFoundException"></exception>
        private static int GetElementValueAsInt(XElement parent, string elementName)
        {
            var value = GetElementValue(parent, elementName);
            if (!int.TryParse(value, out int result))
            {
                throw new XmlTagNotFoundException($"'{elementName}' タグの値が整数に変換できません。");
            }
            return result; // タグの値を整数として返す
        }

        /// <summary>
        /// XML画像ファイル読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="XmlTagNotFoundException"></exception>
        private static byte[] LoadPictureData(string path)
        {
            if (!File.Exists(path))
            {
                throw new XmlTagNotFoundException($"指定されたファイルが見つかりません: {path}");
            }

            return File.ReadAllBytes(path); // アイコンファイルをバイナリデータとして読み込む
        }

        #endregion


        #region vrm読込

        /// <summary>
        /// オブジェクトに対して設計図情報をインプットする。
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="vrmObject"></param>
        /// <returns></returns>
        public static resultInfo vrmLoadStatus(modelInfo modelInfo  ,GameObject vrmObject)
        {
            resultInfo result = new resultInfo();
            result.resultCode = Constants.SUCCESSCODE000;
            result.resultText = Constants.SUCCESSTEXT000;
            try
            {
                var VRMMeta = vrmObject.transform.Find("VRM").GetComponent<VRMMeta>();
                //サムネイル
                Texture2D thumbnai = new Texture2D(2,2);
                thumbnai.LoadImage(modelInfo.modelInfoVRMInfo.ThumbnailData);
                VRMMeta.Meta.Thumbnail = thumbnai;
                //Infomation
                VRMMeta.Meta.Title = modelInfo.modelInfoVRMInfo.Title;
                VRMMeta.Meta.Version = modelInfo.modelInfoVRMInfo.Version;
                VRMMeta.Meta.Author = modelInfo.modelInfoVRMInfo.Author;
                VRMMeta.Meta.ContactInformation = modelInfo.modelInfoVRMInfo.ContactInfo;
                VRMMeta.Meta.Reference = modelInfo.modelInfoVRMInfo.Reference;
                //アバターの人格に関する許容範囲
                VRMMeta.Meta.AllowedUser = (AllowedUser)modelInfo.modelInfoVRMInfo.VRMppr;
                VRMMeta.Meta.ViolentUssage = (UssageLicense)modelInfo.modelInfoVRMInfo.VRMvp;
                VRMMeta.Meta.SexualUssage = (UssageLicense)modelInfo.modelInfoVRMInfo.VRMsp;
                VRMMeta.Meta.CommercialUssage = (UssageLicense)modelInfo.modelInfoVRMInfo.VRMcup;
                VRMMeta.Meta.OtherPermissionUrl = modelInfo.modelInfoVRMInfo.VRMopu;
                //再配布・改変に関する許諾範囲
                VRMMeta.Meta.LicenseType = (LicenseType)modelInfo.modelInfoVRMInfo.VRMlt;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "vrmLoadStatus"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 16進数3種を10進数3種へ変換
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static int[] ConvertHexToDecimal(string hexString)
        {
            hexString = hexString.Replace("#", "");

            if (string.IsNullOrEmpty(hexString) || hexString.Length != 6)
            {
                return new int[] { 0, 0, 0 }; // 6桁でない場合は0を返す
            }

            int[] decimalValues = new int[3];

            for (int i = 0; i < 3; i++)
            {
                string hexPair = hexString.Substring(i * 2, 2);
                if (int.TryParse(hexPair, System.Globalization.NumberStyles.HexNumber, null, out int decimalValue))
                {
                    decimalValues[i] = decimalValue;
                }
                else
                {
                    decimalValues[i] = 0; // 変換できなかった場合は0を返す
                }
            }

            return decimalValues;
        }

        /// <summary>
        /// 10進数を16進数カラーコードに変換する
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string RgbToHex(float r, float g, float b)
        {
            //// 各値を0から255の範囲にクリッピング
            //r = Math.Max(0, Math.Min(255, r));
            //g = Math.Max(0, Math.Min(255, g));
            //b = Math.Max(0, Math.Min(255, b));

            // 16進数に変換し、#を付加して返却
            return $"#{(int)r:X2}{(int)g:X2}{(int)b:X2}";
        }

        /// <summary>
        /// 16進数カラーコードをUnityEngine.Colorに変換する
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static UnityEngine.Color ConvertCodeToColor(string colorCode)
        {
            int[] color3 = common.ConvertHexToDecimal(colorCode);
            float[] floats = new float[3];
            floats[0] = float.Parse($"{color3[0]}");
            floats[1] = float.Parse($"{color3[1]}");
            floats[2] = float.Parse($"{color3[2]}");
            UnityEngine.Color color = new UnityEngine.Color(floats[0] / 255f, floats[1] / 255f, floats[2] / 255f);

            return color;
        }

        /// <summary>
        /// 実行結果エラーリザルト用テキスト置き換え
        /// </summary>
        /// <param name="target"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ResultTextReplace(string target, string replace)
        {
            return target.Replace("{0}", replace);
        }

        /// <summary>
        /// パスの正規化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(string path)
        {
            // パスを正しく結合し、正規化
            string combinedPath = Path.GetFullPath(path);
            return combinedPath;
        }

        /// <summary>
        /// 簡易警告メッセージウィンドウ表示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void easyAlertMessageShow(string title,string message)
        {
            GameObject systemWindow = GameObject.Find("SystemWindow");
            vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();

            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            systemWindowInfo.title = title;
            systemWindowInfo.message = message;

            vSystemWindow.SettingMessage(systemWindowInfo);
            vSystemWindow.showMessageWindowAlert();
        }

        /// <summary>
        /// ブレンドシェイプキー取得
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<SkinnedMeshRenderer> getModelSkinnedMeshRenderer(GameObject model)
        {
            List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
            List<SkinnedMeshRenderer> skinnedMeshRenderersWithBlendShapes = new List<SkinnedMeshRenderer>();

            model.GetComponentsInChildren<SkinnedMeshRenderer>(true, skinnedMeshRenderers);

            foreach (var smr in skinnedMeshRenderers)
            {
                // BlendShapesが存在するか確認
                if (smr.sharedMesh != null && smr.sharedMesh.blendShapeCount > 0)
                {
                    skinnedMeshRenderersWithBlendShapes.Add(smr);
                }
            }

            return skinnedMeshRenderersWithBlendShapes;
        }

        public static resultInfo CreateBlendShapeClip(modelInfo modelInfo,BlendShapePreset type, List<Tuple<string, float>> shapeList)
        {
            resultInfo result = new resultInfo();

            //List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
            //meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

            //proxy.BlendShapeAvatar.Clips;

            //// BlendShapeClipを作成
            //BlendShapeClip blendShapeClip = ScriptableObject.CreateInstance<BlendShapeClip>();
            //blendShapeClip.CopyFrom(src);
            //copy.Prefab = null;
            //copy.Values = ReplaceBlendShapeBinding(copy.Values).ToArray();



            //// BlendShapeの名前を設定
            //blendShapeClip.name = string.Format("BlendShape.{0}", type.ToString());

            //foreach (Tuple<string, float> shape in shapeList)
            //{
            //    blendShapeClip.SetBlendShapeWeight(shape.Item1, shape.Item2);
            //}

            //common.DebackLog(modekInfo.VrmDirectory);

            return result;
        }

        #region 機能リスト
        public static resultInfo setDropDownField(ref DropdownField dropdownField,ref List<moduleList> moduleList)
        {
            resultInfo result = new resultInfo();

            try
            {
                moduleList = new List<moduleList>();
                //プリセット機能読込
                //モデル情報
                ModelInfoEdit modelInfo = new();
                moduleList moduleList1 = new moduleList(modelInfo.moduleNo, modelInfo.moduleTitle, "ModelInfoEdit", modelInfo);
                moduleList.Add(moduleList1);

                //カラー
                ModelColorEdit modelColor = new();
                moduleList moduleList2 = new moduleList(modelColor.moduleNo, modelColor.moduleTitle, "ModelColorEdit", modelColor);
                moduleList.Add(moduleList2);

                //アイコン
                ModelIconEdit modelIcon = new();
                moduleList moduleList3 = new moduleList(modelIcon.moduleNo, modelIcon.moduleTitle, "ModelIconEdit", modelIcon);
                moduleList.Add(moduleList3);

                //表情
                ModelBlendShapeEdit modelBlendShape = new();
                moduleList moduleList4 = new moduleList(modelBlendShape.moduleNo, modelBlendShape.moduleTitle, "ModelBlendShapeEdit", modelBlendShape);
                moduleList.Add(moduleList4);

                //exModule
                //mod拡張性の実装予定

                //並び替え
                moduleList.Sort((x, y) => x.moduleNo.CompareTo(y.moduleNo));

                //DropdownField設定
                dropdownField.choices.Clear();
                dropdownField.choices.Add("選択");
                foreach (moduleList moduleItem in moduleList)
                {
                    dropdownField.choices.Add(moduleItem.moduleTitle);
                }

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
                , ex
, $"setDropDownField"
, Constants.ERRORCODE000
, Constants.ERRORTEXT000
);
            }

            return result;
        }

        #endregion

        public static void setCameraControl(bool control)
        {
            GameObject cameraControl = GameObject.Find("modelView");
            var comp = cameraControl.GetComponent<vVrmLoader>();
            comp.CameraMove = control;
            if (control == true)
            {
                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                mainCamera.transform.position = new Vector3(-0.34f, 0.61f, -7.43f);
                mainCamera.transform.rotation = new Quaternion(0f, 0f, 0f,0f);
                var cameraSize = mainCamera.GetComponent<Camera>();
                cameraSize.orthographicSize = 5;

            }
        }

        #region デバッグ
        /// <summary>
        /// デバッグログ出力
        /// </summary>
        /// <param name="text"></param>
        public static void DebackLog(string text)
        {
            Debug.Log(text);
        }

        /// <summary>
        /// エラー情報詳細出力設定
        /// </summary>
        /// <param name="result"></param>
        /// <param name="ex"></param>
        /// <param name="functionName"></param>
        /// <param name="ErrprCode"></param>
        /// <param name="ErrorText"></param>
        public static void ErrorResultSetting(ref resultInfo result,Exception ex,string functionName,string ErrprCode,string ErrorText)
        {
            DebackLog(functionName);
            DebackLog(ex.Message);
            DebackLog(ex.StackTrace);
            result.resultCode = ErrprCode;
            result.resultText = common.ResultTextReplace(ErrorText, ex.Message);
        }

        public static resultInfo CheckUI(UIDocument UI)
        {
            resultInfo result = new resultInfo();

            try
            {
                var uicheck1 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlMainBlankWindow);

                var uicheck2 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlMainModelInfoWindow);
                var uicheck3 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnThumbnail);
                var uicheck4 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnInfomation);
                var uicheck5 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnParsonal);
                var uicheck6 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnRedistMod);

                var uicheck7 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlMainColorWindow);

                var uicheck8 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlMainIconWindow);
                var uicheck9 = UI.rootVisualElement.Q<VisualElement>(Constants.btnIconSet);

                //var uicheck10 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlmenuListBlendShapesWindows);
                //var uicheck11 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>("btnNeutral");
                //var uicheck12 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnJoy);
                //var uicheck13 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnAngry);
                //var uicheck14 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnSorrow);
                //var uicheck15 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnFun);

                var uicheck16 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlMainOptionWindow);

                var uicheck17 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);

                var uicheck18 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
                var uicheck19 = UI.rootVisualElement.Q<VisualElement>(Constants.btnThumFileLoad);

                var uicheck20 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
                var uicheck21 = UI.rootVisualElement.Q<VisualElement>(Constants.txtModelInfoTitle);
                var uicheck22 = UI.rootVisualElement.Q<VisualElement>(Constants.txtModelInfoVersion);
                var uicheck23 = UI.rootVisualElement.Q<VisualElement>(Constants.txtModelInfoAuthor);
                var uicheck24 = UI.rootVisualElement.Q<VisualElement>(Constants.txtModelInfoContactInfo);
                var uicheck25 = UI.rootVisualElement.Q<VisualElement>(Constants.txtModelInfoReference);

                var uicheck26 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
                var uicheck27 = UI.rootVisualElement.Q<DropdownField>(Constants.ddlPpr);
                var uicheck28 = UI.rootVisualElement.Q<DropdownField>(Constants.ddlVp);
                var uicheck29 = UI.rootVisualElement.Q<DropdownField>(Constants.ddlSp);
                var uicheck30 = UI.rootVisualElement.Q<DropdownField>(Constants.ddlCup);
                var uicheck31 = UI.rootVisualElement.Q<TextField>(Constants.txtOpu);

                var uicheck32 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
                var uicheck33 = UI.rootVisualElement.Q<RadioButtonGroup>(Constants.groupLicenseRdo);
                var uicheck34 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoRedistributionProhibited);
                var uicheck35 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCC0);
                var uicheck36 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBY);
                var uicheck37 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBYNC);
                var uicheck38 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBYSA);
                var uicheck39 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBYNCSA);
                var uicheck40 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBYND);
                var uicheck41 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoCCBYNCND);
                var uicheck42 = UI.rootVisualElement.Q<RadioButton>(Constants.rdoOther);

                var uicheck43 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlSubColorWindow);
                var uicheck44 = UI.rootVisualElement.Q<VisualElement>(Constants.pnlColorView);
                var uicheck45 = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliRslider);
                var uicheck46 = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliGslider);
                var uicheck47 = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliBslider);
                var uicheck48 = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.tglEmission);
                var uicheck49 = UI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnApply);
                var uicheck50 = UI.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);



                //#region サブメニューアイコン
                //public const string pnlSubIconWindows = "submenuListModelIconWindows";
                //public const string btnIconLoad = "btnIconLoad";
                //public const string sliIconX = "sliIconX";
                //public const string sliIconY = "sliIconY";
                //public const string btnIconApply = "btnIconApply";
                //#endregion

                //#region サブメニュー表情
                //public const string pnlSubmenuListBlendShapeWindows = "submenuListBlendShapeWindows";
                //public const string lblBlendShape = "lblBlendShape{0}";
                //public const string fSlideBlendShape = "fSlideBlendShape{0}";
                //public const string txtBlendShape = "txtBlendShape{0}";
                //#endregion



                //#endregion
                //#region モーションプレビュー
                //public const string ddlMotion = "DdlMotion";
                //public const string btnPlay = "BtnPlay";
                //public const string btnStop = "BtnStop";
            }
            catch (Exception ex)
            {
                ErrorResultSetting(
                    ref result,
                    ex,
                    "CheckUI"
                    ,Constants.ERRORCODE000
                    ,Constants.ERRORTEXT000);
                
            }

            return result;
        }

        public static void TextureOutput(Texture2D texture2D,string name)
        {
            string directory = UnityEngine.Application.dataPath + "/bkModel";

            byte[] bytes = texture2D.EncodeToPNG();
            string path = Path.Combine(directory, name);
            File.WriteAllBytes(path, bytes);
            Debug.Log("Texture saved to: " + path);
        }

        #endregion

    }

}
