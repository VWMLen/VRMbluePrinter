using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UniGLTF;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;

namespace vrmBluePrinter
{

    public class cVrmLoader
    {
        
        public resultInfo setVrmGameObject(GameObject targetGameObject)
        {
            resultInfo result = new resultInfo();

            sVrmLoader sVrmLoader = new sVrmLoader();
            result = sVrmLoader.setVrmGameObject(targetGameObject);

            return result;
        }

        public resultInfo getVrmGameObject(ref GameObject targetGameObject)
        {
            resultInfo result = new resultInfo();

            sVrmLoader sVrmLoader = new sVrmLoader();
            result = sVrmLoader.getVrmGameObject(ref targetGameObject);

            return result;
        }

        public resultInfo cameraModelReset(ref Camera mainCamera,ref GameObject targetGameObject)
        {
            resultInfo result = new resultInfo();

            sVrmLoader sVrmLoader = new sVrmLoader();
            result = sVrmLoader.cameraModelReset(ref mainCamera, ref targetGameObject);

            return result;
        }

        public resultInfo getModelInfo(ref modelInfo modelInfo)
        {
            resultInfo result = new resultInfo();

            sVrmLoader sVrmLoader = new sVrmLoader();
            result = sVrmLoader.getModelInfo(ref modelInfo);

            return result;
        }

        public void setCameraMove(ref GameObject targetGameObject, bool flg = false)
        {
            sVrmLoader sVrmLoader = new sVrmLoader();
            sVrmLoader.setCameraMove(ref targetGameObject,flg);
        }
    }

}
