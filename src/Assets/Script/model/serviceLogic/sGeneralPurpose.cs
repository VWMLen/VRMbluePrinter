using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using UniGLTF;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using System.Linq;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace vrmBluePrinter
{
    /// <summary>
    /// 汎用機能Service
    /// </summary>
    public class serviceGeneralPurpose : MonoBehaviour
    {

        public static UIDocument staticUID;



        /// <summary>
        /// モデル選択押下時処理
        /// </summary>
        /// <returns></returns>
        public resultInfo ModelSelect(ref List<modelInfo> models)
        {
            resultInfo result = new resultInfo();

            string directory = UnityEngine.Application.dataPath + "/Model";

            try
            {
                var list = Directory.GetDirectories(directory);

                foreach (var item in list)
                {
                    //common.DebackLog(item.ToString());
                    //設計図読込
                    //C:/Workspace/VRMblueprinter/Assets/Model\diver
                    string path = common.NormalizePath(item);
                    var xmlFiles = Directory.GetFiles(path, "*.xml");

                    common.DebackLog(xmlFiles[0]);
                    result = common.ModelXmlLoad(xmlFiles[0], ref models);
                    //common.DebackLog(result.ToString());
                    if (!common.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                    {
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
                    , ex
                    , "ModelSelect"
                    , Constants.ERRORCODE000
                    , Constants.ERRORTEXT000
                );
            }

            return result;
        }


        /// <summary>
        /// モデル出力
        /// </summary>
        /// <returns></returns>
        public resultInfo ModelExport()
        {
            resultInfo resultInfo = new resultInfo();

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            resultInfo = sVrmLoader.getModelInfo(ref modelInfo);
            //モデルデータが存在しない場合、正常終了で処理終了
            if (!common.ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
            {
                //メッセージを出して警告する
                common.easyAlertMessageShow("注意", "先にモデル選択をおこなってください。");
                resultInfo.resultCode = Constants.ERRORCODE000;
                resultInfo.resultText = Constants.ERRORTEXT000;
                return resultInfo;
            }

            string filePath = null;
            string ExportFolderPath = UnityEngine.Application.dataPath;

            try
            {

                filePath = FileDialogForWindows.SaveDialog("VRMモデルを保存", $"{ExportFolderPath}/{modelInfo.Name}.vrm");

                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!filePath.Contains(".vrm"))
                    {
                        filePath = filePath + ".vrm";
                    }
                    //VRMモデルを取得する。
                    GameObject vrmViewModel = GameObject.Find("modelView/VRM");

                    GameObject instance = (GameObject)Instantiate(vrmViewModel,
                                                  new Vector3(0.0f, 0.0f, 0.0f),
                                                  Quaternion.identity);
                    Export(instance, true, false, filePath, instance);

                    Destroy(instance);

                    resultInfo.resultCode = Constants.SUCCESSCODE000;
                    resultInfo.resultText = Constants.SUCCESSTEXT000;
                }

            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref resultInfo
    , ex
    , "ModelExport"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return resultInfo;
        }

        private void Export(GameObject model, bool useNormalize, bool bakeBlendShape, string path, GameObject instance)
        {
            //var bytes = useNormalize ? ExportCustom(model, false, bakeBlendShape) : ExportSimple(model);
            var bytes = ExportSimple(instance);

            File.WriteAllBytes(path, bytes);
            common.DebackLog($"export to {path}");
        }

        private byte[] ExportSimple(GameObject model)
        {
            var vrm = VRMExporter.Export(new UniGLTF.GltfExportSettings(), model, new RuntimeTextureSerializer());
            var bytes = vrm.ToGlbBytes();
            return bytes;
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

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            returnResult = sVrmLoader.getModelInfo(ref modelInfo);
            //モデルデータが存在しない場合、正常終了で処理終了
            if (!ReferenceEquals(returnResult.resultCode, Constants.SUCCESSCODE000))
            {
                if (modelInfo == null)
                {
                    //メッセージを出して警告する
                    common.easyAlertMessageShow("注意", "先にモデル選択をおこなってください。");
                    returnResult.resultCode = Constants.ERRORCODE301;
                    returnResult.resultText = Constants.ERRORTEXT301;
                    return returnResult;
                }
                else
                {
                    returnResult.resultCode = Constants.ERRORCODE000;
                    returnResult.resultText = Constants.ERRORTEXT000;
                    return returnResult;
                }
            }

            //vVrmLoader vVrmLoader = new vVrmLoader();
            RuntimeAnimatorController[] animators = vVrmLoader.getAnimatorController();

            try
            {

                //Tポーズ,歩く,手を振る,歌う,戦闘構え
                if (newValue.Equals("Tポーズ"))
                {
                    //Tポーズはダミー
                    playAnime = null;
                }
                else if (newValue.Equals("歩く"))
                {
                    playAnime = animators[1];
                }
                else if (newValue.Equals("手を振る"))
                {
                    playAnime = animators[2];
                }
                else if (newValue.Equals("歌う"))
                {
                    playAnime = animators[3];
                }
                else if (newValue.Equals("戦闘構え"))
                {
                    playAnime = animators[4];
                }
                returnResult.resultCode = Constants.SUCCESSCODE000;
                returnResult.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {

                common.ErrorResultSetting(ref returnResult
    , ex
    , "MotionMenuSetting"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return returnResult;
        }

        public resultInfo IniLoad()
        {
            resultInfo resultInfo = new resultInfo();

            string filePath = null;
            string ExportFolderPath = UnityEngine.Application.dataPath;

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            resultInfo = sVrmLoader.getModelInfo(ref modelInfo);
            GameObject vrm = null;
            resultInfo = sVrmLoader.getVrmGameObject(ref vrm);

            if (!ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
            {
                if (vrm == null)
                {
                    resultInfo.resultCode = Constants.ERRORCODE301;
                    resultInfo.resultText = Constants.ERRORTEXT301;
                    return resultInfo;
                }
            }

            try
            {

                //保存先選択
                filePath = FileDialogForWindows.FileDialog("iniファイル(モデル編集データ)を読込", ".ini");

                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!filePath.Contains(".ini"))
                    {
                        filePath = filePath + ".ini";
                    }

                    IniFileExInport iniFileExInport = new IniFileExInport();
                    List<BlendShapeClip> blendShapesclip = new List<BlendShapeClip>();
                    resultInfo = iniFileExInport.ImportModelInfo(filePath, ref modelInfo, ref blendShapesclip);

                    if (ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
                    {
                        sVrmLoader.setModelInfo(modelInfo);

                        setModelColorTexter();


                        var proxy = vrm.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();

                        //表情選択リセット
                        proxy.SetValues(new Dictionary<BlendShapeKey, float>
            {
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), 0},
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy), 0},
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry), 0},
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Sorrow), 0},
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun), 0}
            });
                        //選択表情セット
                        proxy.SetValues(new Dictionary<BlendShapeKey, float>
                    {
                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), 1f}
            });

                        var avatar = proxy.BlendShapeAvatar;
                        if (avatar == null)
                        {
                            avatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
                            proxy.BlendShapeAvatar = avatar;
                        }

                        foreach (var blendShape in blendShapesclip)
                        {
                            BlendShapeClip existingClip = avatar.Clips.Find(clip => clip.Key.Name == blendShape.name);
                            if (existingClip != null)
                            {
                                existingClip.Values = blendShape.Values;

                            }
                        }
                        proxy.Reinitialize();
                    }
                }
                else
                {
                    resultInfo.resultCode = Constants.ERRORCODE000;
                    resultInfo.resultText = Constants.ERRORTEXT000;
                }


            }
            catch (Exception ex)
            {

                common.ErrorResultSetting(ref resultInfo
    , ex
    , "IniLoad"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return resultInfo;
        }

        /// <summary>
        /// iniファイル出力
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public resultInfo IniSave()
        {
            resultInfo resultInfo = new resultInfo();

            string filePath = null;
            string ExportFolderPath = UnityEngine.Application.dataPath;

            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            resultInfo = sVrmLoader.getModelInfo(ref modelInfo);

            if (ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
            {
                try
                {
                    IniFileExInport iniFileExInport = new IniFileExInport();

                    VRMBlendShapeProxy proxy = GameObject.Find("modelView/VRM").GetComponent<VRMBlendShapeProxy>();
                    BlendShapeAvatar iniBlendShapeAvatar = proxy.BlendShapeAvatar;
                    List<BlendShapeClip> clips = iniBlendShapeAvatar.Clips;

                    //保存先選択
                    filePath = FileDialogForWindows.SaveDialog("iniファイル(モデル編集データ)を保存", $"{ExportFolderPath}/{modelInfo.Name}_save.ini");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (!filePath.Contains(".ini"))
                        {
                            filePath = filePath + ".ini";
                        }

                        iniFileExInport.IniFileExport(modelInfo, clips, filePath);
                    }
                    else
                    {
                        resultInfo.resultCode = Constants.ERRORCODE000;
                        resultInfo.resultText = Constants.ERRORTEXT000;
                    }
                }
                catch (Exception ex)
                {
                    common.ErrorResultSetting(ref resultInfo
        , ex
        , "IniSave"
        , Constants.ERRORCODE000
        , Constants.ERRORTEXT000
    );
                }

            }
            else
            {
                if (modelInfo == null)
                {
                    resultInfo.resultCode = Constants.ERRORCODE301;
                    resultInfo.resultText = Constants.ERRORTEXT301;
                }
                else
                {
                    resultInfo.resultCode = Constants.ERRORCODE000;
                    resultInfo.resultText = Constants.ERRORTEXT000;
                }
            }

            return resultInfo;
        }


        /// <summary>
        /// iniロード時用カラー反映
        /// </summary>
        private void setModelColorTexter()
        {
            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);

            //カラーリスト作成
            List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
            foreach (modelInfoColor colorInfo in modelInfo.modelInfoColor)
            {
                UnityEngine.Color backColor = common.ConvertCodeToColor(colorInfo.Color);
                UnityEngine.Color color = backColor;
                colors.Add(color);
            }

            Texture2D newTexture = CreateTempTexture(colors);
            newTexture.Apply();

            //エミッションリスト作成
            Texture2D newEmissionMap = CreateTempEmissionMap(modelInfo.modelInfoColor);
            newEmissionMap.Apply();

            foreach (modelInfoSettingMesh meshInfo in modelInfo.modelInfoSettingMesh)
            {
                setMaterial(meshInfo, newTexture, newEmissionMap);
            }

        }

        #region 作成
        /// <summary>
        /// 特定の色で埋めたテクスチャを取得
        /// </summary>
        private Texture2D CreateTempTexture(List<UnityEngine.Color> colors)
        {
            common.DebackLog("CreateTempTexture");

            const int Size = 512;

            int colorCount = colors.Count;
            //カラー数が奇数の場合+1
            if (colorCount % 2 != 0)
            {
                colorCount++;
                colors.Add(UnityEngine.Color.white);
            }

            var texture = new Texture2D(Size * (colorCount / 2), Size * (colorCount / 2), TextureFormat.RGB24, false);

            //ベースカラーホワイト設定
            for (int x = 0; x < Size * (colorCount / 2); x++)
            {
                for (int y = 0; y < Size * (colorCount / 2); y++)
                {
                    texture.SetPixel(x, y, UnityEngine.Color.white);
                }
            }
            //Z順に色を設定する。
            int index = 0;
            for (int yBlock = 0; yBlock < colorCount / 2; yBlock++)
            {
                for (int xBlock = 0; xBlock < 2; xBlock++)
                {
                    if (index < colors.Count)
                    {
                        var color = colors[index];
                        for (int x = xBlock * Size; x < (xBlock + 1) * Size; x++)
                        {
                            for (int y = yBlock * Size; y < (yBlock + 1) * Size; y++)
                            {
                                texture.SetPixel(x, y, color);
                            }
                        }
                        index++;
                    }
                }
            }

            return texture;
        }

        /// <summary>
        /// エミッションテクスチャ作成
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Texture2D CreateTempEmissionMap(List<modelInfoColor> colors)
        {
            common.DebackLog("CreateTempEmissionMap");

            const int Size = 512;

            int colorCount = colors.Count;
            //カラー数が奇数の場合+1
            if (colorCount % 2 != 0)
            {
                colorCount++;
            }

            var texture = new Texture2D(Size * (colorCount / 2), Size * (colorCount / 2), TextureFormat.RGB24, false);

            //ベースカラーブラック設定
            for (int x = 0; x < Size * (colorCount / 2); x++)
            {
                for (int y = 0; y < Size * (colorCount / 2); y++)
                {
                    texture.SetPixel(x, y, UnityEngine.Color.black);
                }
            }

            int index = 0;
            for (int yBlock = 0; yBlock < colorCount / 2; yBlock++)
            {
                for (int xBlock = 0; xBlock < 2; xBlock++)
                {
                    if (index < colors.Count)
                    {
                        var color = colors[index];
                        for (int x = xBlock * Size; x < (xBlock + 1) * Size; x++)
                        {
                            for (int y = yBlock * Size; y < (yBlock + 1) * Size; y++)
                            {
                                if (color.Emission)
                                {
                                    texture.SetPixel(x, y, UnityEngine.Color.white);
                                }

                            }
                        }
                        index++;
                    }
                }
            }

            return texture;
        }

        /// <summary>
        /// マテリアルをモデルに設定する
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="texture2D"></param>
        private void setMaterial(modelInfoSettingMesh mesh, Texture2D texture2D, Texture2D emissionMap)
        {
            string meshName = mesh.MeshName;
            foreach (modelInfoSettingMaterial material in mesh.Materials)
            {
                string materialName = material.MaterialName;
                int materialIcon = material.MaterialIcon;

                if (materialIcon == 0)
                {
                    GameObject target;
                    target = GameObject.Find($"modelView/VRM/{meshName}");

                    foreach (Material materialTarget in target.GetComponent<Renderer>().materials)
                    {
                        if (materialTarget.name.Contains(materialName))
                        {
                            materialTarget.SetTexture("_MainTex", texture2D);
                            materialTarget.SetTexture("_EmissionMap", emissionMap);
                            materialTarget.SetTextureScale("_MainTex", new Vector2(1f, -1f));
                            materialTarget.SetTextureScale("_EmissionMap", new Vector2(1f, -1f));
                        }
                    }
                }
            }
        }

        #endregion

        #region legacy

        /// <summary>
        /// モデル情報時、メインメニュー設定処理
        /// </summary>
        /// <returns></returns>
        //        public resultInfo ddlModelInfomationPanel(modelInfo modelInfo, ref VisualElement mainMenuPanel, ref UIDocument UID)
        //        {
        //            common.DebackLog("ddlModelInfomationPanel");
        //            resultInfo result = new resultInfo();
        //            try
        //            {
        //                mainMenuPanel = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainModelInfoWindow);
        //                //mainMenuPanel = ddlModelInfomationPanelSetting(mainMenuPanel);
        //                common.DebackLog(mainMenuPanel.ToString());

        //                UIDocument sendUID = UID;

        //                //サムネボタン押下時
        //                UnityEngine.UIElements.Button btnThumbnail = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnThumbnail);
        //                btnThumbnail.clicked += () =>
        //                {
        //                    SubMenuModelInfoSetting(Constants.btnThumbnail, sendUID);
        //                };
        //                //情報ボタン押下時
        //                UnityEngine.UIElements.Button btnInfomation = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnInfomation);
        //                btnInfomation.clicked += () =>
        //                {
        //                    SubMenuModelInfoSetting(Constants.btnInfomation, sendUID);
        //                };
        //                //人格許容範囲ボタン押下時
        //                UnityEngine.UIElements.Button btnParsonal = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnParsonal);
        //                btnParsonal.clicked += () =>
        //                {
        //                    SubMenuModelInfoSetting(Constants.btnParsonal, sendUID);
        //                };
        //                //再配布改変許容範囲ボタン押下時
        //                UnityEngine.UIElements.Button btnRedistMod = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnRedistMod);
        //                btnRedistMod.clicked += () =>
        //                {
        //                    SubMenuModelInfoSetting(Constants.btnRedistMod, sendUID);
        //                };

        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;
        //            }
        //            catch (Exception ex)
        //            {
        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , "ddlModelInfomationPanel"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }

        //            return result;
        //        }

        /// <summary>
        /// モデル情報時、メインメニュー設定処理
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// カラー時、メインメニュー設定処理
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="mainMenu"></param>
        /// <returns></returns>
        //        public resultInfo DdlModelColorPanel(modelInfo modelInfo, ref VisualElement mainMenu, UIDocument UID)
        //        {
        //            resultInfo result = new resultInfo();
        //            int count = 1;
        //            try
        //            {
        //                mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainColorWindow);
        //                foreach (modelInfoColor color in modelInfo.modelInfoColor)
        //                {
        //                    VisualElement colorPanel = new VisualElement();
        //                    colorPanel.name = $"colorPanel{count}";
        //                    colorPanel.style.paddingLeft = 20;
        //                    colorPanel.style.paddingRight = 20;
        //                    colorPanel.style.paddingTop = 10;
        //                    colorPanel.style.paddingBottom = 10;

        //                    VisualElement btnEditColor = new VisualElement();
        //                    btnEditColor.name = $"btnEditColor{count}";
        //                    btnEditColor.style.height = 30;

        //                    UnityEngine.Color backColor = common.ConvertCodeToColor(color.Color);
        //                    btnEditColor.style.backgroundColor = new StyleColor(backColor);
        //                    //btnEditColor.style.backgroundColor = "";
        //                    //btnEditColor.clicked += () =>
        //                    //{
        //                    //    SubMenuColorSetting();
        //                    //};
        //                    //モデル選択時イベント設定
        //                    colorPanel.RegisterCallback<MouseDownEvent>(x =>
        //                    {
        //                        if (x.button == 0)  // 左クリック
        //                        {
        //                            SubMenuColorSetting(color, UID, colorPanel.name);
        //                        }

        //                    });

        //                    UnityEngine.UIElements.Label lblColor = new UnityEngine.UIElements.Label();
        //                    lblColor.name = $"lblColor{count}";
        //                    lblColor.text = $"カラー{count}";
        //                    lblColor.style.fontSize = 20;

        //                    colorPanel.Add(btnEditColor);
        //                    colorPanel.Add(lblColor);

        //                    mainMenu.Add(colorPanel);

        //                    count++;
        //                }
        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;
        //            }
        //            catch (Exception ex)
        //            {
        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , "DdlModelColorPanel"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }

        //            return result;
        //        }

        /// <summary>
        /// モデル情報-各サブメニュー表示
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        //public resultInfo SubMenuModelInfoSetting(string btnName, UIDocument UID)
        //{
        //    resultInfo result = new resultInfo();

        //    VisualElement subMenu = new VisualElement();
        //    VisualElement subBlank = new VisualElement();

        //    staticUID = UID;

        //    subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);

        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getModelInfo(ref modelInfo);

        //    subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
        //    if (subMenu.style.display != DisplayStyle.None)
        //    {
        //        subMenu.style.display = DisplayStyle.None;
        //    }
        //    subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
        //    if (subMenu.style.display != DisplayStyle.None)
        //    {
        //        subMenu.style.display = DisplayStyle.None;
        //    }
        //    subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
        //    if (subMenu.style.display != DisplayStyle.None)
        //    {
        //        subMenu.style.display = DisplayStyle.None;
        //    }
        //    subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
        //    if (subMenu.style.display != DisplayStyle.None)
        //    {
        //        subMenu.style.display = DisplayStyle.None;
        //    }

        //    if (btnName.Equals(Constants.btnThumbnail))
        //    {
        //        subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
        //        //サブメニューサムネ
        //        VisualElement btnThumbnail = UID.rootVisualElement.Q<VisualElement>(Constants.btnThumFileLoad);
        //        //設定したアイコン情報を適応する。

        //        Texture2D iconImage = new Texture2D(2, 2);
        //        iconImage.LoadImage(modelInfo.modelInfoVRMInfo.ThumbnailData);
        //        btnThumbnail.style.backgroundImage = iconImage;
        //        btnThumbnail.RegisterCallback<MouseDownEvent>(x =>
        //        {
        //            if (x.button == 0)  // 左クリック
        //            {
        //                //サムネ
        //                setThimbanail(UID);
        //            }
        //        });

        //    }
        //    else if (btnName.Equals(Constants.btnInfomation))
        //    {
        //        subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
        //        //サブメニューモデル情報
        //        TextField txtModelInfoTitle = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoTitle);
        //        TextField txtModelInfoVersion = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoVersion);
        //        TextField txtModelInfoAuthor = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoAuthor);
        //        TextField txtModelInfoContactInfo = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoContactInfo);
        //        TextField txtModelInfoReference = UID.rootVisualElement.Q<TextField>(Constants.txtModelInfoReference);

        //        txtModelInfoTitle.value = modelInfo.modelInfoVRMInfo.Title;
        //        txtModelInfoVersion.value = modelInfo.modelInfoVRMInfo.Version;
        //        txtModelInfoAuthor.value = modelInfo.modelInfoVRMInfo.Author;
        //        txtModelInfoContactInfo.value = modelInfo.modelInfoVRMInfo.ContactInfo;
        //        txtModelInfoReference.value = modelInfo.modelInfoVRMInfo.Reference;

        //        txtModelInfoTitle.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.Title = x.newValue;
        //        });
        //        txtModelInfoVersion.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.Version = x.newValue;
        //        });
        //        txtModelInfoAuthor.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.Author = x.newValue;
        //        });
        //        txtModelInfoContactInfo.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.ContactInfo = x.newValue;
        //        });
        //        txtModelInfoReference.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.Reference = x.newValue;
        //        });

        //    }
        //    else if (btnName.Equals(Constants.btnParsonal))
        //    {
        //        subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
        //        //サブメニュー人格情報
        //        DropdownField ddlPpr = UID.rootVisualElement.Q<DropdownField>(Constants.ddlPpr);
        //        DropdownField ddlVp = UID.rootVisualElement.Q<DropdownField>(Constants.ddlVp);
        //        DropdownField ddlSp = UID.rootVisualElement.Q<DropdownField>(Constants.ddlSp);
        //        DropdownField ddlCup = UID.rootVisualElement.Q<DropdownField>(Constants.ddlCup);
        //        TextField txtOpu = UID.rootVisualElement.Q<TextField>(Constants.txtOpu);

        //        ddlPpr.index = modelInfo.modelInfoVRMInfo.VRMppr;
        //        ddlVp.index = modelInfo.modelInfoVRMInfo.VRMvp;
        //        ddlSp.index = modelInfo.modelInfoVRMInfo.VRMsp;
        //        ddlCup.index = modelInfo.modelInfoVRMInfo.VRMcup;
        //        txtOpu.value = modelInfo.modelInfoVRMInfo.VRMopu;

        //        ddlPpr.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMppr = getAllowedUserUssageLicenseToInt(x.newValue);
        //        });
        //        ddlVp.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMvp = getAllowedUserUssageLicenseToInt(x.newValue);
        //        });
        //        ddlSp.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMsp = getAllowedUserUssageLicenseToInt(x.newValue);
        //        });
        //        ddlCup.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMcup = getAllowedUserUssageLicenseToInt(x.newValue);
        //        });
        //        txtOpu.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMopu = x.newValue;
        //        });

        //    }
        //    else if (btnName.Equals(Constants.btnRedistMod))
        //    {
        //        subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
        //        //サブメニュー再配布改変許容範囲情報
        //        RadioButtonGroup groupLicenseRdo = UID.rootVisualElement.Q<RadioButtonGroup>(Constants.groupLicenseRdo);

        //        UnityEngine.UIElements.RadioButton rdoRedistributionProhibited = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoRedistributionProhibited);
        //        UnityEngine.UIElements.RadioButton rdoCC0 = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCC0);
        //        UnityEngine.UIElements.RadioButton rdoCCBY = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBY);
        //        UnityEngine.UIElements.RadioButton rdoCCBYNC = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNC);
        //        UnityEngine.UIElements.RadioButton rdoCCBYSA = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYSA);
        //        UnityEngine.UIElements.RadioButton rdoCCBYNCSA = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNCSA);
        //        UnityEngine.UIElements.RadioButton rdoCCBYND = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYND);
        //        UnityEngine.UIElements.RadioButton rdoCCBYNCND = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoCCBYNCND);
        //        UnityEngine.UIElements.RadioButton rdoOther = groupLicenseRdo.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoOther);

        //        List<UnityEngine.UIElements.RadioButton> rdoList = new List<UnityEngine.UIElements.RadioButton>();
        //        rdoList.Add(rdoRedistributionProhibited);
        //        rdoList.Add(rdoCC0);
        //        rdoList.Add(rdoCCBY);
        //        rdoList.Add(rdoCCBYNC);
        //        rdoList.Add(rdoCCBYSA);
        //        rdoList.Add(rdoCCBYNCSA);
        //        rdoList.Add(rdoCCBYND);
        //        rdoList.Add(rdoCCBYNCND);
        //        rdoList.Add(rdoOther);
        //        rdoList[modelInfo.modelInfoVRMInfo.VRMlt].value = true;

        //        rdoRedistributionProhibited.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 0;
        //        });
        //        rdoCC0.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 1;
        //        });
        //        rdoCCBY.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 2;
        //        });
        //        rdoCCBYNC.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 3;
        //        });
        //        rdoCCBYSA.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 4;
        //        });
        //        rdoCCBYNCSA.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 5;
        //        });
        //        rdoCCBYND.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 6;
        //        });
        //        rdoCCBYNCND.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 7;
        //        });
        //        rdoOther.RegisterValueChangedCallback(x =>
        //        {
        //            modelInfo.modelInfoVRMInfo.VRMlt = 8;
        //        });

        //    }

        //    if (subMenu.style.display != DisplayStyle.Flex)
        //    {
        //        subMenu.style.display = DisplayStyle.Flex;
        //    }
        //    if (subBlank.style.display != DisplayStyle.None)
        //    {
        //        subBlank.style.display = DisplayStyle.None;
        //    }

        //    return result;
        //}

        //private int getAllowedUserUssageLicenseToInt(string newValue)
        //{
        //    int returnInt = 0;
        //    if (newValue.Equals("Only Author"))
        //    {
        //        returnInt = 0;
        //    }
        //    else if (newValue.Equals("Explicitly Licensed Parson"))
        //    {
        //        returnInt = 1;
        //    }
        //    else if (newValue.Equals("Everyone"))
        //    {
        //        returnInt = 2;
        //    }
        //    else if (newValue.Equals("Disallow"))
        //    {
        //        returnInt = 0;
        //    }
        //    else if (newValue.Equals("Allow"))
        //    {
        //        returnInt = 1;
        //    }

        //    return returnInt;
        //}

        /// <summary>
        /// モデル情報-サムネイル 反映処理
        /// </summary>
        /// <param name="uID"></param>
        //private void setThimbanail(UIDocument uID)
        //{
        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getModelInfo(ref modelInfo);
        //    VisualElement btnThumbnail = uID.rootVisualElement.Q<VisualElement>(Constants.btnThumFileLoad);

        //    string ImportFolderPath = UnityEngine.Application.dataPath;
        //    string filePath = string.Empty;

        //    filePath = FileDialogForWindows.FileDialog("ファイルを選択", "*.png", "*.jpg");

        //    if (!string.IsNullOrEmpty(filePath))
        //    {
        //        if (filePath.Contains(".png") || filePath.Contains(".jpg"))
        //        {
        //            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

        //            Texture2D iconImage = new Texture2D(2, 2);
        //            iconImage.LoadImage(imageBytes);
        //            btnThumbnail.style.backgroundImage = iconImage;
        //            modelInfo.modelInfoVRMInfo.ThumbnailData = imageBytes;
        //        }
        //    }

        //}

        ///// <summary>
        ///// カラーサブメニューセッティング
        ///// </summary>
        ///// <param name="color"></param>
        ///// <param name="UID"></param>
        ///// <returns></returns>
        //public resultInfo SubMenuColorSetting(modelInfoColor color, UIDocument UID, string colorName)
        //{

        //    resultInfo result = new resultInfo();
        //    VisualElement subMenu = new VisualElement();
        //    VisualElement subBlank = new VisualElement();

        //    staticUID = UID;

        //    subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
        //    subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubColorWindow);

        //    VisualElement pnlColorView = new VisualElement();
        //    UnityEngine.UIElements.Slider sliRslider = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Slider sliGslider = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Slider sliBslider = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Slider tglEmission = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Button btnApply = new UnityEngine.UIElements.Button();
        //    TextField txtSelectTarget = new TextField();

        //    pnlColorView = UID.rootVisualElement.Q<VisualElement>(Constants.pnlColorView);
        //    sliRslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliRslider);
        //    sliGslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliGslider);
        //    sliBslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliBslider);
        //    tglEmission = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.tglEmission);
        //    btnApply = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnApply);
        //    txtSelectTarget = UID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);

        //    // スライダーの変更イベントにリスナーを登録
        //    sliRslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
        //    sliGslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
        //    sliBslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
        //    tglEmission.RegisterValueChangedCallback(OnColorSliderValueChanged);


        //    int[] color3 = common.ConvertHexToDecimal(color.Color);

        //    sliRslider.value = color3[0];
        //    sliGslider.value = color3[1];
        //    sliBslider.value = color3[2];

        //    tglEmission.value = 0;
        //    if (color.Emission)
        //    {
        //        tglEmission.value = 1;
        //    }

        //    //適用ボタンのイベントリスナー登録
        //    btnApply.clicked += () =>
        //    {
        //        setModelColorTexter();
        //    };

        //    //色変え反映用テキストフィールド
        //    txtSelectTarget.name = "txtSelectTarget";
        //    txtSelectTarget.value = colorName;
        //    txtSelectTarget.visible = false;


        //    if (subMenu.style.display != DisplayStyle.Flex)
        //    {
        //        subMenu.style.display = DisplayStyle.Flex;
        //    }
        //    if (subBlank.style.display != DisplayStyle.None)
        //    {
        //        subBlank.style.display = DisplayStyle.None;
        //    }


        //    return result;
        //}

        /// <summary>
        /// アイコン時、メインメニュー設定処理
        /// </summary>
        /// <returns></returns>
        //        public resultInfo DdlModelIconPanel(modelInfo modelInfo, ref VisualElement mainMenu, UIDocument UID)
        //        {
        //            resultInfo result = new resultInfo();
        //            try
        //            {
        //                //アタッチ
        //                mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainIconWindow);
        //                VisualElement subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
        //                VisualElement subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubIconWindows);
        //                if (subMenu.style.display != DisplayStyle.Flex)
        //                {
        //                    subMenu.style.display = DisplayStyle.Flex;
        //                }
        //                if (subBlank.style.display != DisplayStyle.None)
        //                {
        //                    subBlank.style.display = DisplayStyle.None;
        //                }

        //                VisualElement btnIconSet = UID.rootVisualElement.Q<VisualElement>(Constants.btnIconSet);
        //                UnityEngine.UIElements.Button btnIconLoad = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnIconLoad);
        //                UnityEngine.UIElements.Slider sliIconX = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconX);
        //                UnityEngine.UIElements.Slider sliIconY = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconY);
        //                UnityEngine.UIElements.Button btnIconApply = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnIconApply);

        //                //メインメニュー
        //                //アイコン読込設定
        //                setIconImage(UID, modelInfo.IconData);

        //                //サブメニュー
        //                //ロードボタン設定
        //                //ロードボタンのイベントリスナー登録
        //                btnIconLoad.clicked += () =>
        //                {
        //                    setIconTexter(UID);
        //                };
        //                ////スライダーX設定
        //                ////スライダーY設定
        //                //// スライダーの変更イベントにリスナーを登録
        //                //sliIconX.RegisterValueChangedCallback(OnColorIconSliderValueChanged);
        //                //sliIconY.RegisterValueChangedCallback(OnColorIconSliderValueChanged);
        //                //適用ボタン設定
        //                btnIconApply.clicked += () =>
        //                {
        //                    setIconApply(UID);
        //                };


        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;
        //            }
        //            catch (Exception ex)
        //            {
        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , "DdlModelIconPanel"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }

        //            return result;

        //        }

        /// <summary>
        /// アイコン適用
        /// </summary>
        /// <param name="uID"></param>
        //private void setIconApply(UIDocument uID)
        //{
        //    //設定したアイコン情報を適応する。
        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getModelInfo(ref modelInfo);

        //    UnityEngine.UIElements.Slider sliIconX = uID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconX);
        //    UnityEngine.UIElements.Slider sliIconY = uID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliIconY);
        //    modelInfo.modelInfoIconScroll.IconScrollX = (int)sliIconX.value;
        //    modelInfo.modelInfoIconScroll.IconScrollY = (int)sliIconY.value;

        //    foreach (modelInfoSettingMesh mesh in modelInfo.modelInfoSettingMesh)
        //    {
        //        foreach (modelInfoSettingMaterial material in mesh.Materials)
        //        {
        //            if (material.MaterialIcon == 1)
        //            {
        //                GameObject target;
        //                target = GameObject.Find($"modelView/VRM/{mesh.MeshName}");

        //                foreach (Material materialTarget in target.GetComponent<Renderer>().materials)
        //                {
        //                    if (materialTarget.name.Contains(material.MaterialName))
        //                    {
        //                        Destroy(materialTarget.mainTexture);
        //                        Texture2D iconImage = new Texture2D(2, 2);
        //                        iconImage.LoadImage(modelInfo.IconData);
        //                        materialTarget.SetTexture("_MainTex", iconImage);

        //                        materialTarget.SetFloat("_UvAnimScrollX", modelInfo.modelInfoIconScroll.IconScrollX);
        //                        materialTarget.SetFloat("_UvAnimScrollY", modelInfo.modelInfoIconScroll.IconScrollY);

        //                        materialTarget.SetFloat("_UvAnimScrollX", modelInfo.modelInfoIconScroll.IconScrollX);
        //                        materialTarget.SetFloat("_UvAnimScrollY", modelInfo.modelInfoIconScroll.IconScrollY);

        //                        //materialTarget.SetTextureScale("_MainTex", new Vector2(1f, -1f));
        //                        //_faceMaterial.SetColor("_EmissionColor", color4);

        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private resultInfo setIconTexter(UIDocument uID)
        //        {
        //            resultInfo resultInfo = new resultInfo();
        //            string ImportFolderPath = UnityEngine.Application.dataPath;
        //            string filePath = string.Empty;

        //            try
        //            {
        //                ////System.Windows.Formを使用する
        //                //using (OpenFileDialog SaveFileDialog = new OpenFileDialog())
        //                //{
        //                //    SaveFileDialog.InitialDirectory = ImportFolderPath;
        //                //    SaveFileDialog.Filter = "image files|*.png|*.jpg";
        //                //    SaveFileDialog.FilterIndex = 2;
        //                //    if (SaveFileDialog.ShowDialog() == DialogResult.OK)
        //                //    {
        //                //        filePath = SaveFileDialog.FileName;
        //                //    }
        //                //    SaveFileDialog.Dispose();
        //                //}
        //                filePath = FileDialogForWindows.FileDialog("ファイルを選択", "*.png", "*.jpg");

        //                if (!string.IsNullOrEmpty(filePath))
        //                {
        //                    if (filePath.Contains(".png") || filePath.Contains(".jpg"))
        //                    {
        //                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

        //                        setIconImage(uID, imageBytes);
        //                        resultInfo.resultCode = Constants.SUCCESSCODE000;
        //                        resultInfo.resultText = Constants.SUCCESSTEXT000;

        //                        modelInfo modelInfo = new modelInfo();
        //                        sVrmLoader sVrmLoader = new sVrmLoader();
        //                        sVrmLoader.getModelInfo(ref modelInfo);
        //                        modelInfo.IconData = imageBytes;
        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                common.ErrorResultSetting(ref resultInfo
        //    , ex
        //    , "setIconTexter"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }

        //            return resultInfo;
        //        }

        /// <summary>
        /// 画像をメインメニューへ反映
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="imageByte"></param>
        //private void setIconImage(UIDocument uID, byte[] imageByte)
        //{
        //    VisualElement btnIconSet = uID.rootVisualElement.Q<VisualElement>(Constants.btnIconSet);

        //    //メインメニュー
        //    //アイコン読込設定
        //    var iconImage = new Texture2D(2, 2);
        //    iconImage.LoadImage(imageByte);
        //    btnIconSet.style.backgroundImage = iconImage;
        //}

        /// <summary>
        /// ボタン共通設定
        /// </summary>
        /// <param name="button"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        //private void buttonSetting(ref UnityEngine.UIElements.Button button, string name, string text)
        //{
        //    button.name = name;
        //    button.text = text;
        //    button.style.fontSize = 20;
        //    button.style.color = new StyleColor(UnityEngine.Color.black);
        //    button.style.marginTop = 40;
        //    button.style.marginLeft = 40;
        //    button.style.marginRight = 40;
        //}



        /// <summary>
        /// 色をプレビューへ反映
        /// </summary>
        /// <param name="evt"></param>
        //private void OnColorSliderValueChanged(ChangeEvent<float> evt)
        //{
        //    //modelInfo modelInfo = new modelInfo();
        //    //sVrmLoader sVrmLoader = new sVrmLoader();
        //    //sVrmLoader.getModelInfo(ref modelInfo);

        //    VisualElement pnlColorView = staticUID.rootVisualElement.Q<VisualElement>(Constants.pnlColorView);

        //    common.DebackLog($"evt.newValue{evt.newValue}");

        //    string colorCode = getColor();

        //    //プレビューに反映
        //    UnityEngine.Color backColor = common.ConvertCodeToColor(colorCode);
        //    pnlColorView.style.backgroundColor = backColor;

        //}

        //private string getColor()
        //{
        //    UnityEngine.UIElements.Slider sliRslider = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Slider sliGslider = new UnityEngine.UIElements.Slider();
        //    UnityEngine.UIElements.Slider sliBslider = new UnityEngine.UIElements.Slider();
        //    sliRslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliRslider);
        //    sliGslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliGslider);
        //    sliBslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliBslider);

        //    common.DebackLog($"スライダー値：{sliRslider.value}:{sliGslider.value}:{sliBslider.value}");

        //    return common.RgbToHex(sliRslider.value, sliGslider.value, sliBslider.value);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private bool getEmission()
        //{
        //    bool returnEmission = false;

        //    UnityEngine.UIElements.Slider tglEmission = new UnityEngine.UIElements.Slider();
        //    tglEmission = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.tglEmission);

        //    if (tglEmission.value == 0)
        //    {
        //        returnEmission = false;
        //    }
        //    else
        //    {
        //        returnEmission = true;
        //    }

        //    return returnEmission;
        //}

        ///// <summary>
        ///// メインメニューボタン反映
        ///// </summary>
        //private void setColorBtn()
        //{
        //    TextField txtSelectTarget = new TextField();
        //    txtSelectTarget = staticUID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);
        //    //valueからIDを取得する
        //    VisualElement colorPanel = new VisualElement();
        //    string targetName = $"btnEditColor{getTargetId()}";
        //    colorPanel = staticUID.rootVisualElement.Q<VisualElement>(targetName);
        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getModelInfo(ref modelInfo);

        //    int colorId = int.Parse(getTargetId()) - 1;

        //    UnityEngine.Color backColor = common.ConvertCodeToColor(modelInfo.modelInfoColor[colorId].Color);

        //    colorPanel.style.backgroundColor = new StyleColor(backColor);

        //    common.DebackLog($"colorPanel.name：{colorPanel.name}");
        //    common.DebackLog($"colorPanel.style.backgroundColor：{colorPanel.style.backgroundColor}");
        //}

        /// <summary>
        /// モデルへ色反映
        /// </summary>
        //public void setModelColorTexter()
        //{
        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getModelInfo(ref modelInfo);

        //    //スライダー値をmodelInfoに反映
        //    int targetId = int.Parse(getTargetId()) - 1;
        //    bool emission = getEmission();
        //    string colorCode = getColor();
        //    modelInfo.modelInfoColor[targetId].Color = colorCode;
        //    modelInfo.modelInfoColor[targetId].Emission = emission;
        //    sVrmLoader.setModelInfo(modelInfo);

        //    //メインメニューへ色反映
        //    setColorBtn();

        //    //カラーリスト作成
        //    List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
        //    foreach (modelInfoColor colorInfo in modelInfo.modelInfoColor)
        //    {
        //        //int[] color3 = common.ConvertHexToDecimal(colorInfo.Color);
        //        //float[] floats = new float[3];
        //        //floats[0] = float.Parse($"{color3[0]}");
        //        //floats[1] = float.Parse($"{color3[1]}");
        //        //floats[2] = float.Parse($"{color3[2]}");
        //        UnityEngine.Color backColor = common.ConvertCodeToColor(colorInfo.Color);

        //        //UnityEngine.Color color = new UnityEngine.Color(floats[0] / 255f, floats[1] / 255f, floats[2] / 255f);
        //        UnityEngine.Color color = backColor;

        //        colors.Add(color);
        //    }

        //    Texture2D newTexture = CreateTempTexture(colors);
        //    newTexture.Apply();

        //    //エミッションリスト作成
        //    Texture2D newEmissionMap = CreateTempEmissionMap(modelInfo.modelInfoColor);
        //    newEmissionMap.Apply();

        //    foreach (modelInfoSettingMesh meshInfo in modelInfo.modelInfoSettingMesh)
        //    {
        //        setMaterial(meshInfo, newTexture, newEmissionMap);
        //        //_faceMaterial.SetColor("_EmissionColor", color4);
        //    }
        //    //Destroy(newTexture);
        //    //Destroy(newEmissionMap);
        //}

        /// <summary>
        /// マテリアルをモデルに設定する
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="texture2D"></param>
        //private void setMaterial(modelInfoSettingMesh mesh, Texture2D texture2D, Texture2D emissionMap)
        //{
        //    string meshName = mesh.MeshName;
        //    foreach (modelInfoSettingMaterial material in mesh.Materials)
        //    {
        //        string materialName = material.MaterialName;
        //        int materialIcon = material.MaterialIcon;

        //        if (materialIcon == 0)
        //        {
        //            GameObject target;
        //            target = GameObject.Find($"modelView/VRM/{meshName}");

        //            foreach (Material materialTarget in target.GetComponent<Renderer>().materials)
        //            {
        //                if (materialTarget.name.Contains(materialName))
        //                {
        //                    Destroy(materialTarget.mainTexture);
        //                    materialTarget.SetTexture("_MainTex", texture2D);
        //                    materialTarget.SetTexture("_EmissionMap", emissionMap);
        //                    materialTarget.SetTextureScale("_MainTex", new Vector2(1f, -1f));
        //                    materialTarget.SetTextureScale("_EmissionMap", new Vector2(1f, -1f));
        //                    //_faceMaterial.SetColor("_EmissionColor", color4);
        //                }
        //            }
        //        }
        //    }
        //}

        //private string getTargetId()
        //{
        //    TextField txtSelectTarget = new TextField();
        //    txtSelectTarget = staticUID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);
        //    return txtSelectTarget.value.Substring(txtSelectTarget.value.Length - 1, 1);
        //}

        /// <summary>
        /// 特定の色で埋めたテクスチャを取得
        /// </summary>
        //private Texture2D CreateTempTexture(List<UnityEngine.Color> colors)
        //{
        //    common.DebackLog("CreateTempTexture");

        //    const int Size = 512;

        //    int colorCount = colors.Count;
        //    //カラー数が奇数の場合+1
        //    if (colorCount % 2 != 0)
        //    {
        //        colorCount++;
        //        colors.Add(UnityEngine.Color.white);
        //    }
        //    ////応急的
        //    ////カラー順逆転
        //    //List < UnityEngine.Color > reverseColors = new List<UnityEngine.Color>();
        //    //for (int i = colorCount - 1; 0 <= i;i--)
        //    //{
        //    //    reverseColors.Add(colors[i]);
        //    //}
        //    //colors = reverseColors;

        //    var texture = new Texture2D(Size * (colorCount / 2), Size * (colorCount / 2), TextureFormat.RGB24, false);

        //    //ベースカラーホワイト設定
        //    for (int x = 0; x < Size * (colorCount / 2); x++)
        //    {
        //        for (int y = 0; y < Size * (colorCount / 2); y++)
        //        {
        //            texture.SetPixel(x, y, UnityEngine.Color.white);
        //        }
        //    }
        //    //Z順に色を設定する。
        //    int index = 0;
        //    for (int yBlock = 0; yBlock < colorCount / 2; yBlock++)
        //    {
        //        for (int xBlock = 0; xBlock < 2; xBlock++)
        //        {
        //            if (index < colors.Count)
        //            {
        //                var color = colors[index];
        //                for (int x = xBlock * Size; x < (xBlock + 1) * Size; x++)
        //                {
        //                    for (int y = yBlock * Size; y < (yBlock + 1) * Size; y++)
        //                    {
        //                        texture.SetPixel(x, y, color);
        //                    }
        //                }
        //                index++;
        //            }
        //        }
        //    }

        //    return texture;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        //public static Texture2D CreateTempEmissionMap(List<modelInfoColor> colors)
        //{
        //    common.DebackLog("CreateTempEmissionMap");

        //    const int Size = 512;

        //    int colorCount = colors.Count;
        //    //カラー数が奇数の場合+1
        //    if (colorCount % 2 != 0)
        //    {
        //        colorCount++;
        //    }

        //    var texture = new Texture2D(Size * (colorCount / 2), Size * (colorCount / 2), TextureFormat.RGB24, false);

        //    //ベースカラーブラック設定
        //    for (int x = 0; x < Size * (colorCount / 2); x++)
        //    {
        //        for (int y = 0; y < Size * (colorCount / 2); y++)
        //        {
        //            texture.SetPixel(x, y, UnityEngine.Color.black);
        //        }
        //    }

        //    int index = 0;
        //    for (int yBlock = 0; yBlock < colorCount / 2; yBlock++)
        //    {
        //        for (int xBlock = 0; xBlock < 2; xBlock++)
        //        {
        //            if (index < colors.Count)
        //            {
        //                var color = colors[index];
        //                for (int x = xBlock * Size; x < (xBlock + 1) * Size; x++)
        //                {
        //                    for (int y = yBlock * Size; y < (yBlock + 1) * Size; y++)
        //                    {
        //                        if (color.Emission)
        //                        {
        //                            texture.SetPixel(x, y, UnityEngine.Color.white);
        //                        }

        //                    }
        //                }
        //                index++;
        //            }
        //        }
        //    }

        //    return texture;
        //}

        //        /// <summary>
        //        /// 表情時時、メインメニュー設定処理
        //        /// </summary>
        //        /// <param name="modelInfo"></param>
        //        /// <param name="mainMenu"></param>
        //        /// <param name="UID"></param>
        //        /// <returns></returns>
        //        public resultInfo DdlModelBlendShapePanel(modelInfo modelInfo, ref VisualElement mainMenu, UIDocument UID)
        //        {
        //            resultInfo result = new resultInfo();

        //            try
        //            {
        //                //アタッチ
        //                mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlmenuListBlendShapesWindows);
        //                VisualElement subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
        //                VisualElement subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubmenuListBlendShapeWindows);

        //                //RadioButtonGroup rdoGBlendShapes = UID.rootVisualElement.Q<RadioButtonGroup>("testXXX");
        //                UnityEngine.UIElements.Button btnNeutral = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnNeutral);
        //                UnityEngine.UIElements.Button btnJoy = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnJoy);
        //                UnityEngine.UIElements.Button btnAngry = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnAngry);
        //                UnityEngine.UIElements.Button btnSorrow = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnSorrow);
        //                UnityEngine.UIElements.Button btnFun = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnFun);

        //                btnNeutral.clicked += () =>
        //                {
        //                    SubMenuBlendShapeSetting(UID, Constants.btnNeutral);
        //                };
        //                btnJoy.clicked += () =>
        //                {
        //                    SubMenuBlendShapeSetting(UID, Constants.btnJoy);
        //                };
        //                btnAngry.clicked += () =>
        //                {
        //                    SubMenuBlendShapeSetting(UID, Constants.btnAngry);
        //                };
        //                btnSorrow.clicked += () =>
        //                {
        //                    SubMenuBlendShapeSetting(UID, Constants.btnSorrow);
        //                };
        //                btnFun.clicked += () =>
        //                {
        //                    SubMenuBlendShapeSetting(UID, Constants.btnFun);
        //                };

        //                if (subMenu.style.display != DisplayStyle.Flex)
        //                {
        //                    subMenu.style.display = DisplayStyle.Flex;
        //                }
        //                if (subBlank.style.display != DisplayStyle.None)
        //                {
        //                    subBlank.style.display = DisplayStyle.None;
        //                }

        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;

        //            }
        //            catch (Exception ex)
        //            {

        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , "DdlModelBlendShapePanel"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }


        //            return result;
        //        }

        //        /// <summary>
        //        /// サブメニュー、表情設定
        //        /// </summary>
        //        /// <param name="UID"></param>
        //        /// <param name="newValue"></param>
        //        /// <returns></returns>
        //        public resultInfo SubMenuBlendShapeSetting(UIDocument UID, string target)
        //        {
        //            resultInfo result = new resultInfo();
        //            GameObject targetObject = null;
        //            sVrmLoader sVrmLoader = new sVrmLoader();
        //            sVrmLoader.getVrmGameObject(ref targetObject);

        //            VRMBlendShapeProxy proxy = targetObject.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();

        //            int preset = 0;

        //            if (target.Equals(Constants.btnNeutral))
        //            {
        //                preset = (int)BlendShapePreset.Neutral;
        //            }
        //            else if (target.Equals(Constants.btnJoy))
        //            {
        //                preset = (int)BlendShapePreset.Joy;
        //            }
        //            else if (target.Equals(Constants.btnAngry))
        //            {
        //                preset = (int)BlendShapePreset.Angry;
        //            }
        //            else if (target.Equals(Constants.btnSorrow))
        //            {
        //                preset = (int)BlendShapePreset.Sorrow;
        //            }
        //            else if (target.Equals(Constants.btnFun))
        //            {
        //                preset = (int)BlendShapePreset.Fun;
        //            }

        //            //var getBlendShape = proxy.GetValues();

        //            //proxy.SetValues(new Dictionary<BlendShapeKey, float>{
        //            //    { getBlendShape.GetEnumerator((BlendShapePreset)preset),1f)}
        //            //});

        //            //表情選択リセット
        //            proxy.SetValues(new Dictionary<BlendShapeKey, float>
        //            {
        //                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), 0},
        //                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy), 0},
        //                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry), 0},
        //                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Sorrow), 0},
        //                {BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun), 0}
        //            });
        //            //選択表情セット
        //            proxy.SetValues(new Dictionary<BlendShapeKey, float>
        //            {
        //                {BlendShapeKey.CreateFromPreset((BlendShapePreset)preset), 1f} 
        //            });

        //            try
        //            {
        //                List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
        //                meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

        //                VisualElement pnlSubmenuListBlendShapeWindows = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubmenuListBlendShapeWindows);
        //                TextField txtTargetType = UID.rootVisualElement.Q<TextField>(Constants.txtTargetType);
        //                ScrollView scrShapeKey = UID.rootVisualElement.Q<ScrollView>(Constants.scrShapeKey);
        //                scrShapeKey.Clear();

        //                txtTargetType.value = target;

        //                int count = 0;

        //                foreach (SkinnedMeshRenderer render in meshRenderers)
        //                {
        //                    if (render.sharedMesh != null && render.sharedMesh.blendShapeCount > 0)
        //                    {
        //                        for (int i = 0; i < render.sharedMesh.blendShapeCount; i++)
        //                        {
        //                            UnityEngine.UIElements.Label lblBlendShape = new UnityEngine.UIElements.Label();
        //                            UnityEngine.UIElements.Slider fSlideBlendShape = new UnityEngine.UIElements.Slider();
        //                            TextField txtBlendShape = new TextField();
        //                            VisualElement time = new VisualElement();
        //                            time.style.flexDirection = FlexDirection.Column;

        //                            lblBlendShape.name = string.Format(Constants.lblBlendShape, count.ToString());
        //                            lblBlendShape.text = render.sharedMesh.GetBlendShapeName(i);
        //                            lblBlendShape.style.fontSize = 20;
        //                            fSlideBlendShape.name = string.Format(Constants.fSlideBlendShape, count.ToString());
        //                            fSlideBlendShape.lowValue = 0;
        //                            fSlideBlendShape.highValue = 100f;
        //                            fSlideBlendShape.value = render.GetBlendShapeWeight(i);
        //                            fSlideBlendShape.showInputField = true;
        //                            fSlideBlendShape.style.fontSize = 20;
        //                            fSlideBlendShape.style.paddingLeft = 20;
        //                            fSlideBlendShape.style.paddingRight = 20;
        //                            fSlideBlendShape.RegisterValueChangedCallback(x =>
        //                            {
        //                                OnBlendShapeSliderValueChanged(target, UID);
        //                            });

        //                            time.Add(lblBlendShape);
        //                            time.Add(fSlideBlendShape);

        //                            scrShapeKey.Add(time);

        //                            count++;
        //                        }
        //                    }
        //                }

        //                pnlSubmenuListBlendShapeWindows.Add(scrShapeKey);

        //                result.resultCode = Constants.SUCCESSCODE000;
        //                result.resultText = Constants.SUCCESSTEXT000;
        //            }
        //            catch (Exception ex)
        //            {
        //                common.ErrorResultSetting(ref result
        //    , ex
        //    , "SubMenuBlendShapeSetting"
        //    , Constants.ERRORCODE000
        //    , Constants.ERRORTEXT000
        //);
        //            }


        //            return result;
        //        }

        ///// <summary>
        ///// ブレンドシェイプキー操作時処理
        ///// </summary>
        ///// <param name="target"></param>
        ///// <param name="UI"></param>
        //private void OnBlendShapeSliderValueChanged(string target,UIDocument UI)
        //{
        //    GameObject targetObject = null;
        //    sVrmLoader sVrmLoader = new sVrmLoader();
        //    sVrmLoader.getVrmGameObject(ref targetObject);
        //    modelInfo modelInfo = new modelInfo();
        //    sVrmLoader.getModelInfo(ref modelInfo);

        //    VRMBlendShapeProxy proxy = targetObject.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();

        //    int preset = 0;

        //    if (target.Equals(Constants.btnNeutral))
        //    {
        //        preset = (int)BlendShapePreset.Neutral;
        //    }
        //    else if (target.Equals(Constants.btnJoy))
        //    {
        //        preset = (int)BlendShapePreset.Joy;
        //    }
        //    else if (target.Equals(Constants.btnAngry))
        //    {
        //        preset = (int)BlendShapePreset.Angry;
        //    }
        //    else if (target.Equals(Constants.btnSorrow))
        //    {
        //        preset = (int)BlendShapePreset.Sorrow;
        //    }
        //    else if (target.Equals(Constants.btnFun))
        //    {
        //        preset = (int)BlendShapePreset.Fun;
        //    }

        //    List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
        //    meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

        //    //proxy.SetValues(new Dictionary<BlendShapeKey, float>
        //    //{
        //    //    {BlendShapeKey.CreateFromPreset((BlendShapePreset)preset), 1f}
        //    //});

        //    //proxy.SetValues(new Dictionary<BlendShapeKey, float>
        //    //{
        //    //    {BlendShapeKey.CreateUnknown((BlendShapePreset)preset), 1f}
        //    //});

        //    int count = 0;

        //    List<Tuple<string, float>> list = new List<Tuple<string, float>>();

        //    foreach (SkinnedMeshRenderer render in meshRenderers)
        //    {
        //        if (render.sharedMesh != null && render.sharedMesh.blendShapeCount > 0)
        //        {
        //            for (int i = 0; i < render.sharedMesh.blendShapeCount; i++)
        //            {
        //                UnityEngine.UIElements.Slider fSlideBlendShape = new UnityEngine.UIElements.Slider();
        //                fSlideBlendShape = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(string.Format(Constants.fSlideBlendShape, count.ToString()));

        //                string blendshapeName = render.sharedMesh.GetBlendShapeName(i);
        //                float blendshapeValue = fSlideBlendShape.value;

        //                //モデルへの適応
        //                render.SetBlendShapeWeight(i, blendshapeValue);

        //                //proxy.AccumulateValue(BlendShapeKey.CreateFromPreset((BlendShapePreset)preset), 1.0f);

        //                //proxy.SetValues(new Dictionary<BlendShapeKey, float>
        //                //{
        //                //    {BlendShapeKey.CreateUnknown(blendshapeName), blendshapeValue}
        //                //});

        //                list.Add(Tuple.Create(blendshapeName, blendshapeValue));

        //                count++;
        //            }
        //        }
        //    }

        //    ////BlendShapeClipへの適用
        //    //common.CreateBlendShapeClip(modelInfo, (BlendShapePreset)preset, list);


        //    ////proxy.Apply();
        //}
        #endregion

    }
}
