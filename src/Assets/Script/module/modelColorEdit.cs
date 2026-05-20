using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    /// <summary>
    /// 機能モジュール：モデル情報
    /// </summary>
    public class ModelColorEdit
    {
        #region プロパティ
        public int moduleNo { get; set; }
        public string moduleTitle { get; set; }
        #endregion

        #region 変数
        private UIDocument staticUID;
        #endregion

        #region 固定設定関数
        /// <summary>
        /// 初期化
        /// </summary>
        public ModelColorEdit()
        {
            //機能設定
            //表示する選択肢の設定
            moduleNo = 2;
            moduleTitle = "カラー";
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
                result = DdlModelColorPanel(ref mainWindowElement, UID);
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
        /// カラー時、メインメニュー設定処理
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="mainMenu"></param>
        /// <returns></returns>
        private resultInfo DdlModelColorPanel(ref VisualElement mainMenu, UIDocument UID)
        {
            resultInfo result = new resultInfo();
            int count = 1;
            try
            {
                modelInfo modelInfo = new modelInfo();
                vVrmLoader vVrmLoader = new vVrmLoader();
                result = vVrmLoader.getModelInfo(ref modelInfo);

                if (object.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                {
                    mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlMainColorWindow);
                    foreach (modelInfoColor color in modelInfo.modelInfoColor)
                    {
                        VisualElement colorPanel = new VisualElement();
                        colorPanel.name = $"colorPanel{count}";
                        colorPanel.style.paddingLeft = 20;
                        colorPanel.style.paddingRight = 20;
                        colorPanel.style.paddingTop = 10;
                        colorPanel.style.paddingBottom = 10;

                        VisualElement btnEditColor = new VisualElement();
                        btnEditColor.name = $"btnEditColor{count}";
                        btnEditColor.style.height = 30;

                        UnityEngine.Color backColor = common.ConvertCodeToColor(color.Color);
                        btnEditColor.style.backgroundColor = new StyleColor(backColor);
                        //モデル選択時イベント設定
                        colorPanel.RegisterCallback<MouseDownEvent>(x =>
                        {
                            if (x.button == 0)  // 左クリック
                            {
                                SubMenuColorSetting(color, UID, colorPanel.name);
                            }

                        });

                        UnityEngine.UIElements.Label lblColor = new UnityEngine.UIElements.Label();
                        lblColor.name = $"lblColor{count}";
                        lblColor.text = $"カラー{count}";
                        lblColor.style.fontSize = 20;

                        colorPanel.Add(btnEditColor);
                        colorPanel.Add(lblColor);

                        mainMenu.Add(colorPanel);

                        count++;
                    }
                }
                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "DdlModelColorPanel"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// カラーサブメニューセッティング
        /// </summary>
        /// <param name="color"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public resultInfo SubMenuColorSetting(modelInfoColor color, UIDocument UID, string colorName)
        {

            resultInfo result = new resultInfo();
            VisualElement subMenu = new VisualElement();
            VisualElement subBlank = new VisualElement();

            staticUID = UID;

            subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
            subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubColorWindow);

            VisualElement pnlColorView = new VisualElement();
            UnityEngine.UIElements.Slider sliRslider = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Slider sliGslider = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Slider sliBslider = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Slider tglEmission = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Button btnApply = new UnityEngine.UIElements.Button();
            TextField txtSelectTarget = new TextField();

            pnlColorView = UID.rootVisualElement.Q<VisualElement>(Constants.pnlColorView);
            sliRslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliRslider);
            sliGslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliGslider);
            sliBslider = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliBslider);
            tglEmission = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.tglEmission);
            btnApply = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnApply);
            txtSelectTarget = UID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);

            // スライダーの変更イベントにリスナーを登録
            sliRslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
            sliGslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
            sliBslider.RegisterValueChangedCallback(OnColorSliderValueChanged);
            tglEmission.RegisterValueChangedCallback(OnColorSliderValueChanged);


            int[] color3 = common.ConvertHexToDecimal(color.Color);

            sliRslider.value = color3[0];
            sliGslider.value = color3[1];
            sliBslider.value = color3[2];

            tglEmission.value = 0;
            if (color.Emission)
            {
                tglEmission.value = 1;
            }

            //適用ボタンのイベントリスナー登録
            btnApply.clicked += () =>
            {
                setModelColorTexter();
            };

            //色変え反映用テキストフィールド
            txtSelectTarget.name = "txtSelectTarget";
            txtSelectTarget.value = colorName;
            txtSelectTarget.visible = false;


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
        /// モデルへ色反映
        /// </summary>
        public void setModelColorTexter()
        {
            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);

            //スライダー値をmodelInfoに反映
            int targetId = int.Parse(getTargetId()) - 1;
            bool emission = getEmission();
            string colorCode = getColor();
            modelInfo.modelInfoColor[targetId].Color = colorCode;
            modelInfo.modelInfoColor[targetId].Emission = emission;
            sVrmLoader.setModelInfo(modelInfo);

            //メインメニューへ色反映
            setColorBtn();

            //カラーリスト作成
            List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
            foreach (modelInfoColor colorInfo in modelInfo.modelInfoColor)
            {
                //int[] color3 = common.ConvertHexToDecimal(colorInfo.Color);
                //float[] floats = new float[3];
                //floats[0] = float.Parse($"{color3[0]}");
                //floats[1] = float.Parse($"{color3[1]}");
                //floats[2] = float.Parse($"{color3[2]}");
                UnityEngine.Color backColor = common.ConvertCodeToColor(colorInfo.Color);

                //UnityEngine.Color color = new UnityEngine.Color(floats[0] / 255f, floats[1] / 255f, floats[2] / 255f);
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
                //_faceMaterial.SetColor("_EmissionColor", color4);
            }
#if UNITY_EDITOR
            // この部分はエディター内でのみ実行されます。
            common.TextureOutput(newTexture, "main.png");
            common.TextureOutput(newEmissionMap, "emission.png");
#endif

            //if (newTexture != null)
            //{
            //    newTexture.Resize(0, 0);
            //    newTexture = null; // 参照をクリア
            //}
            //if (newEmissionMap != null)
            //{
            //    newEmissionMap.Resize(0, 0);
            //    newEmissionMap = null; // 参照をクリア
            //}
            //Destroy(newTexture);
            //Destroy(newEmissionMap);
        }

        private string getTargetId()
        {
            TextField txtSelectTarget = new TextField();
            txtSelectTarget = staticUID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);
            return txtSelectTarget.value.Substring(txtSelectTarget.value.Length - 1, 1);
        }

        /// <summary>
        /// メインメニューボタン反映
        /// </summary>
        private void setColorBtn()
        {
            TextField txtSelectTarget = new TextField();
            txtSelectTarget = staticUID.rootVisualElement.Q<TextField>(Constants.txtSelectTarget);
            //valueからIDを取得する
            VisualElement colorPanel = new VisualElement();
            string targetName = $"btnEditColor{getTargetId()}";
            colorPanel = staticUID.rootVisualElement.Q<VisualElement>(targetName);
            modelInfo modelInfo = new modelInfo();
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getModelInfo(ref modelInfo);

            int colorId = int.Parse(getTargetId()) - 1;

            UnityEngine.Color backColor = common.ConvertCodeToColor(modelInfo.modelInfoColor[colorId].Color);

            colorPanel.style.backgroundColor = new StyleColor(backColor);

            common.DebackLog($"colorPanel.name：{colorPanel.name}");
            common.DebackLog($"colorPanel.style.backgroundColor：{colorPanel.style.backgroundColor}");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool getEmission()
        {
            bool returnEmission = false;

            UnityEngine.UIElements.Slider tglEmission = new UnityEngine.UIElements.Slider();
            tglEmission = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.tglEmission);

            if (tglEmission.value == 0)
            {
                returnEmission = false;
            }
            else
            {
                returnEmission = true;
            }

            return returnEmission;
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
                            //Destroy(materialTarget.mainTexture);
                            materialTarget.SetTexture("_MainTex", texture2D);
                            materialTarget.SetTexture("_ShadowTex", texture2D);
                            materialTarget.SetTexture("_EmissionMap", emissionMap);
                            materialTarget.SetTextureScale("_MainTex", new Vector2(1f, -1f));
                            materialTarget.SetTextureScale("_ShadowTex", new Vector2(1f, -1f));
                            materialTarget.SetTextureScale("_EmissionMap", new Vector2(1f, -1f));
                            //materialTarget.SetColor("_EmissionColor", Color.white * 1f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 色をプレビューへ反映
        /// </summary>
        /// <param name="evt"></param>
        private void OnColorSliderValueChanged(ChangeEvent<float> evt)
        {
            //modelInfo modelInfo = new modelInfo();
            //sVrmLoader sVrmLoader = new sVrmLoader();
            //sVrmLoader.getModelInfo(ref modelInfo);

            VisualElement pnlColorView = staticUID.rootVisualElement.Q<VisualElement>(Constants.pnlColorView);

            common.DebackLog($"evt.newValue{evt.newValue}");

            string colorCode = getColor();

            //プレビューに反映
            UnityEngine.Color backColor = common.ConvertCodeToColor(colorCode);
            pnlColorView.style.backgroundColor = backColor;

        }


        private string getColor()
        {
            UnityEngine.UIElements.Slider sliRslider = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Slider sliGslider = new UnityEngine.UIElements.Slider();
            UnityEngine.UIElements.Slider sliBslider = new UnityEngine.UIElements.Slider();
            sliRslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliRslider);
            sliGslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliGslider);
            sliBslider = staticUID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(Constants.sliBslider);

            common.DebackLog($"スライダー値：{sliRslider.value}:{sliGslider.value}:{sliBslider.value}");

            return common.RgbToHex(sliRslider.value, sliGslider.value, sliBslider.value);
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
            int splitCount = (int)Math.Floor(Math.Sqrt(colorCount));
            //分割数
            if ((splitCount * splitCount) != colorCount)
            {
                splitCount++;
            }

            //カラー数が分割数に収まらない場合
            while (colorCount % splitCount != 0)
            {
                colorCount++;
                colors.Add(UnityEngine.Color.white);
            }

            ////応急的
            ////カラー順逆転
            //List < UnityEngine.Color > reverseColors = new List<UnityEngine.Color>();
            //for (int i = colorCount - 1; 0 <= i;i--)
            //{
            //    reverseColors.Add(colors[i]);
            //}
            //colors = reverseColors;

            var texture = new Texture2D(Size * (colorCount / splitCount), Size * (colorCount / splitCount), TextureFormat.RGB24, false);

            //ベースカラーホワイト設定
            for (int x = 0; x < Size * (colorCount / splitCount); x++)
            {
                for (int y = 0; y < Size * (colorCount / splitCount); y++)
                {
                    texture.SetPixel(x, y, UnityEngine.Color.white);
                }
            }
            //Z順に色を設定する。
            int index = 0;
            for (int yBlock = 0; yBlock < colorCount / splitCount; yBlock++)
            {
                for (int xBlock = 0; xBlock < splitCount; xBlock++)
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

        #endregion

        #endregion

    }
}
