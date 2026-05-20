using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;

namespace vrmBluePrinter
{
    /// <summary>
    /// 機能モジュール：モデル情報
    /// </summary>
    public class ModelBlendShapeEdit
    {
        #region プロパティ
        public int moduleNo { get; set; }
        public string moduleTitle { get; set; }

        #endregion

        #region 固定設定関数
        /// <summary>
        /// 初期化
        /// </summary>
        public ModelBlendShapeEdit()
        {
            //機能設定
            //表示する選択肢の設定
            moduleNo = 4;
            moduleTitle = "表情";
        }

        /// <summary>
        /// メインウィンドウ表示設定
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="mainWindowElement"></param>
        /// <returns></returns>
        public resultInfo mainWindowSetting(UIDocument UID, ref VisualElement mainWindowElement)
        {
            //メインウィンドウの表示内容設定
            resultInfo result = new resultInfo();

            try
            {
                sVrmLoader sVrmLoader = new sVrmLoader();
                modelInfo modelInfo = new modelInfo();
                sVrmLoader.getModelInfo(ref modelInfo);

                result = DdlModelBlendShapePanel(modelInfo, ref mainWindowElement, UID);

                //result.resultCode = Constants.SUCCESSCODE000;
                //result.resultText = Constants.SUCCESSTEXT000;
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

        #endregion

        #region 独自機能関数
        /// <summary>
        /// 表情時時、メインメニュー設定処理
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="mainMenu"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public resultInfo DdlModelBlendShapePanel(modelInfo modelInfo, ref VisualElement mainMenu, UIDocument UID)
        {
            resultInfo result = new resultInfo();

            try
            {
                //アタッチ
                mainMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlmenuListBlendShapesWindows);
                VisualElement subBlank = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
                VisualElement subMenu = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubmenuListBlendShapeWindows);

                RadioButtonGroup rdoGBlendShapes = UID.rootVisualElement.Q<RadioButtonGroup>(Constants.rbgBlendShape);
                UnityEngine.UIElements.RadioButton rdoNeutral = UID.rootVisualElement.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoNeutral);
                UnityEngine.UIElements.RadioButton rdoJoy = UID.rootVisualElement.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoJoy);
                UnityEngine.UIElements.RadioButton rdoAngry = UID.rootVisualElement.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoAngry);
                UnityEngine.UIElements.RadioButton rdoSorrow = UID.rootVisualElement.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoSorrow);
                UnityEngine.UIElements.RadioButton rdoFun = UID.rootVisualElement.Q<UnityEngine.UIElements.RadioButton>(Constants.rdoFun);

                List<string> rdoButton = new List<string>();
                rdoButton.Add(Constants.rdoNeutral);
                rdoButton.Add(Constants.rdoJoy);
                rdoButton.Add(Constants.rdoAngry);
                rdoButton.Add(Constants.rdoSorrow);
                rdoButton.Add(Constants.rdoFun);

                rdoGBlendShapes.RegisterValueChangedCallback(x =>
                {
                    SubMenuBlendShapeSetting(UID, rdoButton[x.newValue]);
                });


                if (subMenu.style.display != DisplayStyle.Flex)
                {
                    subMenu.style.display = DisplayStyle.Flex;
                }
                if (subBlank.style.display != DisplayStyle.None)
                {
                    subBlank.style.display = DisplayStyle.None;
                }

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;

            }
            catch (Exception ex)
            {

                common.ErrorResultSetting(ref result
    , ex
    , "DdlModelBlendShapePanel"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }


            return result;
        }

        /// <summary>
        /// サブメニュー、表情設定
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public resultInfo SubMenuBlendShapeSetting(UIDocument UID, string target)
        {
            resultInfo result = new resultInfo();
            GameObject targetObject = null;
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getVrmGameObject(ref targetObject);

            UnityEngine.UIElements.Button btnBlendShapeApply = UID.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnBlendShapeApply);
            if (btnBlendShapeApply.style.display != DisplayStyle.None)
            {
                btnBlendShapeApply.style.display = DisplayStyle.None;
            }

            VRMBlendShapeProxy proxy = targetObject.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();

            int preset = 0;
            string trueTargetName = string.Empty;

            if (target.Equals(Constants.rdoNeutral))
            {
                preset = (int)BlendShapePreset.Neutral;
                trueTargetName = "Neutral";
            }
            else if (target.Equals(Constants.rdoJoy))
            {
                preset = (int)BlendShapePreset.Joy;
                trueTargetName = "Joy";
            }
            else if (target.Equals(Constants.rdoAngry))
            {
                preset = (int)BlendShapePreset.Angry;
                trueTargetName = "Angry";
            }
            else if (target.Equals(Constants.rdoSorrow))
            {
                preset = (int)BlendShapePreset.Sorrow;
                trueTargetName = "Sorrow";
            }
            else if (target.Equals(Constants.rdoFun))
            {
                preset = (int)BlendShapePreset.Fun;
                trueTargetName = "Fun";
            }

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
                {BlendShapeKey.CreateFromPreset((BlendShapePreset)preset), 1f}
            });


            try
            {
                List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
                meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

                VisualElement pnlSubmenuListBlendShapeWindows = UID.rootVisualElement.Q<VisualElement>(Constants.pnlSubmenuListBlendShapeWindows);
                TextField txtTargetType = UID.rootVisualElement.Q<TextField>(Constants.txtTargetType);
                ScrollView scrShapeKey = UID.rootVisualElement.Q<ScrollView>(Constants.scrShapeKey);
                scrShapeKey.Clear();

                //テキストフィールドにBlendShape格納ディレクトリを設定する
                modelInfo modelInfo = new modelInfo();
                sVrmLoader.getModelInfo(ref modelInfo);
                string newPath = modelInfo.Directory.Replace(Constants.extendXML, string.Format(Constants.blendshapeSetting, trueTargetName));
                txtTargetType.value = $"{newPath},{trueTargetName}";

                int count = 0;

                foreach (SkinnedMeshRenderer render in meshRenderers)
                {
                    if (render.sharedMesh != null && render.sharedMesh.blendShapeCount > 0)
                    {
                        if (btnBlendShapeApply.style.display != DisplayStyle.Flex)
                        {
                            btnBlendShapeApply.style.display = DisplayStyle.Flex;
                            btnBlendShapeApply.clicked += () =>
                            {
                                AddOrUpdateBlendShapeClip(UID, targetObject);
                            };
                        }
                        for (int i = 0; i < render.sharedMesh.blendShapeCount; i++)
                        {
                            UnityEngine.UIElements.Label lblBlendShape = new UnityEngine.UIElements.Label();
                            UnityEngine.UIElements.Slider fSlideBlendShape = new UnityEngine.UIElements.Slider();
                            TextField txtBlendShape = new TextField();
                            VisualElement time = new VisualElement();
                            time.style.flexDirection = FlexDirection.Column;

                            lblBlendShape.name = string.Format(Constants.lblBlendShape, count.ToString());
                            lblBlendShape.text = render.sharedMesh.GetBlendShapeName(i);
                            lblBlendShape.style.fontSize = 20;
                            fSlideBlendShape.name = string.Format(Constants.fSlideBlendShape, count.ToString());
                            fSlideBlendShape.lowValue = 0;
                            fSlideBlendShape.highValue = 100f;
                            fSlideBlendShape.value = render.GetBlendShapeWeight(i);
                            fSlideBlendShape.showInputField = true;
                            fSlideBlendShape.style.fontSize = 20;
                            fSlideBlendShape.style.paddingLeft = 20;
                            fSlideBlendShape.style.paddingRight = 20;
                            fSlideBlendShape.RegisterValueChangedCallback(x =>
                            {
                                OnBlendShapeSliderValueChanged(target, UID);
                            });

                            time.Add(lblBlendShape);
                            time.Add(fSlideBlendShape);

                            scrShapeKey.Add(time);

                            count++;
                        }
                    }
                }

                pnlSubmenuListBlendShapeWindows.Add(scrShapeKey);

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "SubMenuBlendShapeSetting"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }


            return result;
        }

        /// <summary>
        /// ブレンドシェイプキー操作時処理
        /// </summary>
        /// <param name="target"></param>
        /// <param name="UI"></param>
        private void OnBlendShapeSliderValueChanged(string target, UIDocument UI)
        {
            GameObject targetObject = null;
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.getVrmGameObject(ref targetObject);
            modelInfo modelInfo = new modelInfo();
            sVrmLoader.getModelInfo(ref modelInfo);

            VRMBlendShapeProxy proxy = targetObject.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();

            int preset = 0;

            if (target.Equals(Constants.rdoNeutral))
            {
                preset = (int)BlendShapePreset.Neutral;
            }
            else if (target.Equals(Constants.rdoJoy))
            {
                preset = (int)BlendShapePreset.Joy;
            }
            else if (target.Equals(Constants.rdoAngry))
            {
                preset = (int)BlendShapePreset.Angry;
            }
            else if (target.Equals(Constants.rdoSorrow))
            {
                preset = (int)BlendShapePreset.Sorrow;
            }
            else if (target.Equals(Constants.rdoFun))
            {
                preset = (int)BlendShapePreset.Fun;
            }

            List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
            meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

            int count = 0;

            List<Tuple<string, float>> list = new List<Tuple<string, float>>();

            foreach (SkinnedMeshRenderer render in meshRenderers)
            {
                if (render.sharedMesh != null && render.sharedMesh.blendShapeCount > 0)
                {
                    for (int i = 0; i < render.sharedMesh.blendShapeCount; i++)
                    {
                        UnityEngine.UIElements.Slider fSlideBlendShape = new UnityEngine.UIElements.Slider();
                        fSlideBlendShape = UI.rootVisualElement.Q<UnityEngine.UIElements.Slider>(string.Format(Constants.fSlideBlendShape, count.ToString()));

                        string blendshapeName = render.sharedMesh.GetBlendShapeName(i);
                        float blendshapeValue = fSlideBlendShape.value;

                        //モデルへの適応
                        render.SetBlendShapeWeight(i, blendshapeValue);


                        list.Add(Tuple.Create(blendshapeName, blendshapeValue));

                        count++;
                    }
                }
            }

        }

        /// <summary>
        /// ブレンドシェイプ作成・更新
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="targetObject"></param>
        /// <param name="clipName"></param>
        /// <param name="clipDirectory"></param>
        private void AddOrUpdateBlendShapeClip(UIDocument UID, GameObject targetObject)
        {
            // get or create blendshape proxy
            var proxy = targetObject.transform.Find("VRM").GetComponent<VRMBlendShapeProxy>();
            //if (proxy == null)
            //{
            //    proxy = targetObject.transform.Find("VRM").AddComponent<VRMBlendShapeProxy>();
            //}

            // get or create blendshape avatar
            var avatar = proxy.BlendShapeAvatar;
            if (avatar == null)
            {
                avatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
                proxy.BlendShapeAvatar = avatar;
            }

            TextField txtTargetType = UID.rootVisualElement.Q<TextField>(Constants.txtTargetType);
            string clipDirectory = txtTargetType.value.Split(',')[0];
            string clipName = txtTargetType.value.Split(',')[1];

            List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
            meshRenderers = common.getModelSkinnedMeshRenderer(targetObject);

            int count = 0;

            List<Tuple<string, float,int>> list = new List<Tuple<string, float,int>>();

            foreach (SkinnedMeshRenderer render in meshRenderers)
            {
                if (render.sharedMesh != null && render.sharedMesh.blendShapeCount > 0)
                {
                    for (int i = 0; i < render.sharedMesh.blendShapeCount; i++)
                    {
                        UnityEngine.UIElements.Slider fSlideBlendShape = new UnityEngine.UIElements.Slider();
                        fSlideBlendShape = UID.rootVisualElement.Q<UnityEngine.UIElements.Slider>(string.Format(Constants.fSlideBlendShape, count.ToString()));

                        //string blendshapeName = render.sharedMesh.GetBlendShapeName(i);
                        string renderName = render.name;
                        float blendshapeValue = fSlideBlendShape.value;
                        int blendshapeIndex = i;

                        if (blendshapeValue > 0f)
                        {
                            list.Add(Tuple.Create(renderName, blendshapeValue, blendshapeIndex));
                            
                        }
                        count++;
                    }
                }
            }

            // check if the clip already exists
            BlendShapeClip existingClip = avatar.Clips.Find(clip => clip.Key.Name == clipName);

            if (existingClip != null)
            {
                // update existing clip
                common.DebackLog($"Updating {clipName}");

                int countIndex = 0;

                BlendShapeBinding[] listBlendShapeBinding = new BlendShapeBinding[list.Count];

                foreach (Tuple<string, float,int> item in list)
                {
                    BlendShapeBinding bsb = new BlendShapeBinding();
                    bsb.RelativePath = item.Item1;
                    bsb.Index = item.Item3;
                    bsb.Weight = item.Item2;

                    listBlendShapeBinding[countIndex] = bsb;
       
                    countIndex++;
                }

                existingClip.Values = listBlendShapeBinding;

            }
            //else
            //{
            //    // create new clip
            //    var clip = ScriptableObject.CreateInstance<BlendShapeClip>();
            //    common.DebackLog($"Adding {clipName}");

            //    // unity asset name and vrm export name
            //    clip.name = clipName;
            //    clip.BlendShapeName = clipName;
            //    clip.Preset = BlendShapePreset.Unknown;

            //    clip.IsBinary = false;

            //    BlendShapeBinding[] listBlendShapeBinding = new BlendShapeBinding[list.Count];
            //    int countIndex = 0;
            //    foreach (Tuple<string, float,int> item in list)
            //    {
            //        listBlendShapeBinding[countIndex] = (
            //new BlendShapeBinding
            //{
            //    RelativePath = item.Item1,
            //    Index = item.Item3,
            //    Weight = item.Item2
            //});
            //        countIndex++;
            //    }

            //    clip.Values = listBlendShapeBinding;

            //    existingClip = clip;

            //}

            //avatar.Clips[10] = existingClip;
            proxy.Reinitialize();
            //proxy.Apply();
        }


        #endregion

    }
}
