using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UniGLTF;
using UniHumanoid;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using VRM;

namespace vrmBluePrinter
{

    public class sVrmLoader
    {
        // VRMモデルを表示するGameObject
        private static GameObject targetGameObject; 
        private static modelInfo modelInfo;

        public static async void LoadVRMModel(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("ファイルが見つかりません: " + path);
                return;
            }

            common.ModelXmlLoad(path, ref modelInfo);

            byte[] bytes = File.ReadAllBytes(modelInfo.VrmDirectory);
            await LoadBytesAsync(modelInfo.VrmDirectory, bytes);
            await Task.Yield();
            common.vrmLoadStatus(modelInfo, targetGameObject);
        }

        private static async Task LoadBytesAsync(string path, byte[] bytes)
        {
            Debug.Log($"LoadModelAsync: {path}: {bytes.Length} bytes");


            // vrm
            var instance = await VrmUtility.LoadBytesAsync(path, bytes, new ImmediateCaller(),
                GetVrmMaterialGenerator);
            //var instance = await VrmUtility.LoadAsync(path);

            // モデルをターゲットGameObjectに設定
            if (targetGameObject != null)
            {
                foreach (Transform n in targetGameObject.transform)
                {
                    GameObject.Destroy(n.gameObject);
                }
            }

            instance.EnableUpdateWhenOffscreen();
            instance.ShowMeshes();
            instance.transform.SetParent(targetGameObject.transform);
            instance.transform.localPosition = new Vector3(Constants.v3TX, Constants.v3TY, Constants.v3TZ);
            instance.transform.localRotation = new Quaternion(Constants.v3RX, Constants.v3RY, Constants.v3RZ, 0f);
            instance.transform.localScale = new Vector3(Constants.v3SX, Constants.v3SY, Constants.v3SZ);

            //HumanPoseTransferを追加
            vVrmLoader vVrmLoader = new vVrmLoader();
            HumanPoseTransfer pose = instance.gameObject.AddComponent<HumanPoseTransfer>();
            pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
            HumanPoseClip clip = vVrmLoader.getClip();
            pose.PoseClip = clip;

            //AnimatorController
            Animator animatorController = instance.gameObject.GetComponent<Animator>();
            animatorController.runtimeAnimatorController = null;
        }


        private static IMaterialDescriptorGenerator GetVrmMaterialGenerator(glTF_VRM_extensions vrm)
        {
            return new VRM.BuiltInVrmMaterialDescriptorGenerator(vrm);
        }

        /// <summary>
        /// ゲームオブジェクト設定
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public resultInfo setVrmGameObject(GameObject setGameObject)
        {
            resultInfo result = new resultInfo();

            try
            {
                targetGameObject = setGameObject;
                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (System.Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "setGameObject"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// ゲームオブジェクト取得
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public resultInfo getVrmGameObject(ref GameObject getGameObject)
        {
            resultInfo result = new resultInfo();

            try
            {
                getGameObject = targetGameObject;
                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (System.Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "getVrmGameObject"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// 設計図情報設定
        /// </summary>
        /// <param name="setModelInfo"></param>
        /// <returns></returns>
        public resultInfo setModelInfo(modelInfo setModelInfo)
        {
            resultInfo result = new resultInfo();

            try
            {
                modelInfo = setModelInfo;
            }
            catch (System.Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "setModelInfo"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// 設計図情報取得
        /// </summary>
        /// <param name="getModelInfo"></param>
        /// <returns></returns>
        public resultInfo getModelInfo(ref modelInfo getModelInfo)
        {
            resultInfo result = new resultInfo();

            try
            {
                if(modelInfo is null)
                {
                    throw new Exception("設計図が設定されていません。");
                }
                getModelInfo = modelInfo;
                result.resultCode = Constants.SUCCESSCODE000;
                result.resultText = Constants.SUCCESSTEXT000;
            }
            catch (System.Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "getModelInfo"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        public resultInfo setModelInfoWindows(modelInfo getModelInfo)
        {
            resultInfo result = new resultInfo();



            return result;
        }

        /// <summary>
        /// カメラ位置モデルビュー位置リセット
        /// </summary>
        /// <param name="mainCamera"></param>
        /// <param name="targetGameObject"></param>
        /// <returns></returns>
        public resultInfo cameraModelReset(ref Camera mainCamera, ref GameObject targetGameObject)
        {
            resultInfo result = new resultInfo();

            try
            {
                //カメラリセット
                mainCamera.transform.position = new Vector3(-0.34f, 0.61f, -7.43f);
                mainCamera.transform.Rotate(0.0f, 0.0f, 0.0f);

                //ModelViewリセット
                modelViewReset(targetGameObject);
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
    , ex
    , "cameraModelReset"
    , Constants.ERRORCODE000
    , Constants.ERRORTEXT000
);
            }

            return result;
        }

        /// <summary>
        /// モデルビューリセット
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <returns></returns>
        private GameObject modelViewReset(GameObject targetGameObject)
        {
            targetGameObject.transform.position = new Vector3(-0.5f, 2.0f, 0.0f);
            targetGameObject.transform.Rotate(0.0f, 0.0f, 0.0f);

            return targetGameObject;
        }

        public void setCameraMove(ref GameObject targetGameObject,bool flg = false)
        {

            vVrmLoader myComponent = targetGameObject.GetComponent<vVrmLoader>();
            myComponent.setCameraMove(flg);
            
        }

    }

}
