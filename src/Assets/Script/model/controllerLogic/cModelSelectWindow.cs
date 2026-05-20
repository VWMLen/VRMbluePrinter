using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace vrmBluePrinter
{
    public class cModelSelectWindow : MonoBehaviour
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
            sModelSelectWindow sModelSelectWindow = new sModelSelectWindow();
            result = sModelSelectWindow.settingModelListView(modelInfos, ref btns);

            return result;
        }
    }
}
