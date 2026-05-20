using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using VRM;
using vrmBluePrinter;

namespace vrmBluePrinter
{

    public class IniFileExInport
    {
        #region エクスポート
        /// <summary>
        /// iniファイルエクスポート
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="BlendShapeBinding"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public resultInfo IniFileExport(modelInfo modelInfo, List<BlendShapeClip> clips, string directory)
        {
            resultInfo result = new resultInfo();

            try
            {
                StringBuilder iniContent = new StringBuilder();
                WriteIni(modelInfo, "Model", iniContent);
                foreach (BlendShapeClip clip in clips)
                {
                    iniContent.AppendLine($"[BlendShape.{clip.name}]");
                    if (clip.Values.Length >= 1)
                    {
                        WriteBlendShapeBinding(clip.Values, iniContent, $"{clip.name}");
                    }
                    iniContent.AppendLine();
                }


                // INIファイルに書き込み
                File.WriteAllText(directory, iniContent.ToString());

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "IniFileExport"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }
            return result;
        }

        private void WriteIni<T>(T items, string sectionName, StringBuilder iniContent)
        {
            iniContent.AppendLine($"[{sectionName}]");
            WriteProperties(items, iniContent, $"{sectionName}");
            iniContent.AppendLine();
        }

        private void WriteProperties<T>(T item, StringBuilder iniContent, string prefix)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(item);

                if (value is Array array)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        WriteProperties(array.GetValue(j), iniContent, $"{prefix}.{prop.Name}{j}");
                    }
                }
                else if (value is IList<modelInfoColor> colorList)
                {
                    for (int j = 0; j < colorList.Count; j++)
                    {
                        WriteProperties(colorList[j], iniContent, $"{prefix}.{prop.Name}{j}");
                    }
                }
                else if (value is modelInfoIconScroll iconScroll)
                {
                    WriteProperties(iconScroll, iniContent, $"{prefix}.{prop.Name}");
                }
                else if (value is modelInfoVRMInfo vrmInfo)
                {
                    WriteProperties(vrmInfo, iniContent, $"{prefix}.{prop.Name}");
                }
                else if (value is IList<modelInfoSettingMesh> meshList)
                {
                    for (int j = 0; j < meshList.Count; j++)
                    {
                        WriteProperties(meshList[j], iniContent, $"{prefix}.{prop.Name}{j}");
                    }
                }
                else
                {
                    iniContent.AppendLine($"{prefix}.{prop.Name}={value}");
                }
            }
        }

        private void WriteBlendShapeBinding(BlendShapeBinding[] binding, StringBuilder iniContent, string prefix)
        {
            for (int i = 0; i < binding.Length; i++)
            {
                iniContent.AppendLine($"{prefix}.RelativePath={binding[i].RelativePath}");
                iniContent.AppendLine($"{prefix}.Index={binding[i].Index}");
                iniContent.AppendLine($"{prefix}.Weight={binding[i].Weight}");
            }
        }

        #endregion

        #region インポート
        public resultInfo ImportModelInfo(string filePath, ref modelInfo model, ref List<BlendShapeClip> clips)
        {
            resultInfo result = new resultInfo();

            try
            {
                var lines = File.ReadAllLines(filePath);
                string currentSection = string.Empty;

                foreach (var line in lines)
                {
                    if (line.StartsWith("["))
                    {
                        currentSection = line.Trim('[', ']');
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            if (currentSection == "Model")
                            {
                                SetModelInfoProperty(model, key, value);
                            }
                            else if (currentSection.StartsWith("BlendShape."))
                            {
                                string blendShapeName = currentSection.Substring("BlendShape.".Length);
                                string newName = blendShapeName.Split('.')[1];

                                var clip = clips.FirstOrDefault(c => c.name == newName);

                                // BlendShapeClipが存在しない場合は新規作成
                                if (clip == null)
                                {
                                    clip = new BlendShapeClip { name = newName, BlendShapeName= newName };
                                    clips.Add(clip);
                                }

                                SetBlendShapeBindingProperty(clip, key, value);

                                //string blendShapeName = currentSection.Substring("BlendShape.".Length);
                                //var clip = clips.FirstOrDefault(c => c.name == blendShapeName);
                                //if (clip != null)
                                //{
                                //    SetBlendShapeBindingProperty(clip, key, value);
                                //}
                            }
                        }
                    }
                }

                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "ImportModelInfo"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);

            }

            return result;
        }

        private void SetModelInfoProperty(modelInfo model, string key, string value)
        {
            switch (key)
            {
                case "Model.Name":
                    model.Name = value;
                    break;
                case "Model.Icon":
                    model.Icon = value;
                    break;
                case "Model.Directory":
                    model.Directory = value;
                    break;
                case "Model.VrmDirectory":
                    model.VrmDirectory = value;
                    break;
                case var k when k.StartsWith("Model.modelInfoColor"):
                    // 数字部分を抽出
                    var match = Regex.Match(k, @"\d+");
                    if (match.Success)
                    {
                        int colorIndex = int.Parse(match.Value);
                        if (model.modelInfoColor == null)
                            model.modelInfoColor = new List<modelInfoColor>();
                        while (model.modelInfoColor.Count <= colorIndex)
                            model.modelInfoColor.Add(new modelInfoColor());

                        if (key.EndsWith("Color"))
                            model.modelInfoColor[colorIndex].Color = value;
                        else if (key.EndsWith("Emission"))
                            model.modelInfoColor[colorIndex].Emission = bool.Parse(value);
                    }
                    break;
                case var k when k.StartsWith("Model.modelInfoIconScroll"):
                    // modelInfoIconScrollがnullの場合は初期化
                    if (model.modelInfoIconScroll == null)
                        model.modelInfoIconScroll = new modelInfoIconScroll();
                    if (key.EndsWith("IconScrollX"))
                        model.modelInfoIconScroll.IconScrollX = int.Parse(value);
                    else if (key.EndsWith("IconScrollY"))
                        model.modelInfoIconScroll.IconScrollY = int.Parse(value);
                    break;
                case var k when k.StartsWith("Model.modelInfoVRMInfo"):
                    // modelInfoVRMInfoがnullの場合は初期化
                    if (model.modelInfoVRMInfo == null)
                        model.modelInfoVRMInfo = new modelInfoVRMInfo();
                    SetVRMInfoProperty(model.modelInfoVRMInfo, key, value);
                    break;
                case var k when k.StartsWith("Model.modelInfoSettingMesh"):
                    // メッシュ設定の処理を追加
                    if (model.modelInfoSettingMesh == null)
                        model.modelInfoSettingMesh = new List<modelInfoSettingMesh>();

                    // 数字部分を抽出
                    var meshMatch = Regex.Match(k, @"\d+");
                    if (meshMatch.Success)
                    {
                        int meshIndex = int.Parse(meshMatch.Value);

                        // インデックスが範囲外の場合は追加
                        while (model.modelInfoSettingMesh.Count <= meshIndex)
                            model.modelInfoSettingMesh.Add(new modelInfoSettingMesh());

                        // メッシュ名とマテリアル設定を処理
                        if (key.EndsWith("MeshName"))
                        {
                            model.modelInfoSettingMesh[meshIndex].MeshName = value;
                        }
                        //else if (key.EndsWith("Materials"))
                        //{
                        //    // マテリアル設定の処理（ここでは仮に空のリストを設定）
                        //    model.modelInfoSettingMesh[meshIndex].Materials = new List<modelInfoSettingMaterial>();
                        //}
                    }

                    break;
            }
        }

        private void SetVRMInfoProperty(modelInfoVRMInfo vrmInfo, string key, string value)
        {
            switch (key)
            {
                case "Model.modelInfoVRMInfo.Thumbnail":
                    vrmInfo.Thumbnail = value;
                    break;
                case "Model.modelInfoVRMInfo.Title":
                    vrmInfo.Title = value;
                    break;
                case "Model.modelInfoVRMInfo.Version":
                    vrmInfo.Version = value;
                    break;
                case "Model.modelInfoVRMInfo.Author":
                    vrmInfo.Author = value;
                    break;
                case "Model.modelInfoVRMInfo.ContactInfo":
                    vrmInfo.ContactInfo = value;
                    break;
                case "Model.modelInfoVRMInfo.Reference":
                    vrmInfo.Reference = value;
                    break;
                case "Model.modelInfoVRMInfo.VRMppr":
                    vrmInfo.VRMppr = int.Parse(value);
                    break;
                case "Model.modelInfoVRMInfo.VRMvp":
                    vrmInfo.VRMvp = int.Parse(value);
                    break;
                case "Model.modelInfoVRMInfo.VRMsp":
                    vrmInfo.VRMsp = int.Parse(value);
                    break;
                case "Model.modelInfoVRMInfo.VRMcup":
                    vrmInfo.VRMcup = int.Parse(value);
                    break;
                case "Model.modelInfoVRMInfo.VRMopu":
                    vrmInfo.VRMopu = value;
                    break;
                case "Model.modelInfoVRMInfo.VRMlt":
                    vrmInfo.VRMlt = int.Parse(value);
                    break;
            }
        }

        private void SetBlendShapeBindingProperty(BlendShapeClip clip, string key, string value)
        {
            // BlendShapeのプロパティを処理
            if (key.EndsWith("RelativePath"))
            {
                // 新しいBindingを追加
                Array.Resize(ref clip.Values, (clip.Values?.Length ?? 0) + 1);
                var newBinding = new BlendShapeBinding
                {
                    RelativePath = value
                };
                clip.Values[^1] = newBinding; // 最後に追加
            }
            else if (key.EndsWith("Index"))
            {
                // 最後のBindingにインデックスを設定
                if (clip.Values != null && clip.Values.Length > 0)
                {
                    clip.Values[^1].Index = int.Parse(value); // 最後のBindingに設定
                }
            }
            else if (key.EndsWith("Weight"))
            {
                // 最後のBindingにウェイトを設定
                if (clip.Values != null && clip.Values.Length > 0)
                {
                    clip.Values[^1].Weight = float.Parse(value); // 最後のBindingに設定
                }
            }
        }

        #endregion
    }

}
