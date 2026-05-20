using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UniGLTF;
using UniHumanoid;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;
using static UnityEngine.GraphicsBuffer;

namespace vrmBluePrinter
{

    public class vVrmLoader : MonoBehaviour
    {
        // VRMモデルを表示するGameObject
        [SerializeField]
        private GameObject targetGameObject;
        [SerializeField]
        private GameObject UIObject;
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private HumanPoseClip clip;
        [SerializeField]
        private RuntimeAnimatorController[] controllers;
        [SerializeField]
        public bool CameraMove = true;

        private UIDocument UIDocument;
        private Slider SliderX;
        private Slider SliderY;

        public modelInfo modelInfo;

        // Start is called before the first frame update
        void Start()
        {
            cVrmLoader cVrmLoader = new cVrmLoader();
            resultInfo resultInfo = new resultInfo();
            //ゲームオブジェクト設定
            resultInfo = cVrmLoader.setVrmGameObject(targetGameObject);

            //スライダー設定
            UIDocument = UIObject.GetComponent<UIDocument>();
            SliderX = UIDocument.rootVisualElement.Q<UnityEngine.UIElements.Slider>("SdModelXscroll");
            SliderY = UIDocument.rootVisualElement.Q<UnityEngine.UIElements.Slider>("SdModelYscroll");
            common.DebackLog($"SliderX.value:{SliderX.value}");
            common.DebackLog($"SliderY.value:{SliderY.value}");

            //カメラ設定

        }

        // Update is called once per frame
        void Update()
        {
            if (CameraMove == true)
            {
                if (targetGameObject.transform.Find("VRM"))
                {
                    //スライダー
                    targetGameObject.transform.localRotation = Quaternion.Euler(SliderY.value, SliderX.value, Constants.v3RZ);

                    //真ん中クリック
                    if (Input.GetMouseButton(2))
                    {

                        rotateCamera();
                    }

                    //マウスホイール
                    sizeCamera();

                    //右クリック
                    if (Input.GetMouseButton(1))
                    {
                        postion();
                    }

                    //キーR
                    if (Input.GetKeyUp(KeyCode.R))
                    {
                        //オブジェクトリセット
                        cameraModelReset();
                    }

                    //if (mainCamera.orthographicSize <= 0)
                    //{
                    //    mainCamera.orthographicSize = 1;
                    //}
                    //else if (mainCamera.orthographicSize >= 11)
                    //{
                    //    mainCamera.orthographicSize = 10;
                    //}
                }

            }

        }

        #region ビュー操作系

        /// <summary>
        /// カメラオブジェクト中心に回転
        /// </summary>
        private void rotateCamera()
        {
            Vector3 angle = new Vector3(
                    Input.GetAxis("Mouse X") * 2.0f,
                    0,
                    0
                );
            mainCamera.transform.RotateAround(targetGameObject.transform.position, Vector3.up, angle.x);
        }

        /// <summary>
        /// カメラズーム設定
        /// </summary>
        private void sizeCamera()
        {
            var scroll = Input.mouseScrollDelta.y * Time.deltaTime * 100;
            if (mainCamera.orthographicSize >= 2)
            {
                mainCamera.orthographicSize += scroll;
        }
            else if (mainCamera.orthographicSize <= 30)
            {
                mainCamera.orthographicSize -= scroll;
            }
}

        /// <summary>
        /// カメラ移動設定
        /// </summary>
        private void postion()
        {
            float rotateX = Input.GetAxis("Mouse X") * 1.0f;
            float rotateY = Input.GetAxis("Mouse Y") * 1.0f;
            mainCamera.transform.Rotate(rotateY, rotateX, 0.0f);
        }

        /// <summary>
        /// カメラ・モデル位置のリセット
        /// </summary>
        private void cameraModelReset()
        {
            resultInfo resultInfo = new resultInfo();
            cVrmLoader cVrmLoader = new cVrmLoader();
            resultInfo = cVrmLoader.cameraModelReset(ref mainCamera, ref targetGameObject);
        }

        public void setCameraMove(bool flg = false)
        {
            CameraMove = flg;
        }

        #endregion

        public HumanPoseClip getClip()
        {
            return clip;
        }

        public RuntimeAnimatorController[] getAnimatorController()
        {
            return controllers;
        }

        public resultInfo getVrmGameObject(ref GameObject targetGameObject)
        {
            resultInfo result = new resultInfo();

            cVrmLoader cVrmLoader = new cVrmLoader();
            result = cVrmLoader.getVrmGameObject(ref targetGameObject);

            return result;
        }

        public resultInfo getModelInfo(ref modelInfo modelInfo)
        {
            resultInfo result = new resultInfo();

            cVrmLoader cVrmLoader = new cVrmLoader();
            result = cVrmLoader.getModelInfo(ref modelInfo);

            return result;
        }

    }

}
