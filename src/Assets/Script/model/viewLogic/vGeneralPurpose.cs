using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


namespace vrmBluePrinter
{
    /// <summary>
    /// 汎用機能View
    /// </summary>
    public class viewGeneralPurpose : MonoBehaviour
    {
        [SerializeField]
        private UIDocument mainUI;
        [SerializeField]
        public GameObject systemWindow;
        [SerializeField]
        public GameObject modelSelectWindow;
        [SerializeField]
        public GameObject modelView;
        [SerializeField]
        private AudioClip selectSound;

        private DropdownField DdlMenuView;
        private UnityEngine.UIElements.Button BtnModelSelectView;
        private UnityEngine.UIElements.Button BtnExportView;
        private UnityEngine.UIElements.Button BtnLoadView;
        private UnityEngine.UIElements.Button BtnSaveView;

        /// <summary>
        /// メインメニュービュー
        /// </summary>
        private VisualElement mainMenuView;
        /// <summary>
        /// サブメニュービュー
        /// </summary>
        private VisualElement subMenuView;


        private controllerGeneralPurpose controller;

        private AudioSource audioSource;

        //機能選択
        const string DdlMenu = "DdlMenu";
        //モデル選択
        const string BtnModelSelect = "BtnModelSelect";
        //エクスポート
        const string BtnExport = "BtnExport";

        #region メインメニュー UI
        const string mainMenu = "menuListWindows";
        #region モデル情報
        const string mainMenuModelInfo = "menuListWindowsModelInfo";
        const string BtnThumbnail = "btnThumbnail";
        const string BtnInfomation = "btnInfomation";
        const string BtnParsonal = "btnParsonal";
        const string BtnRedistMod = "btnRedistMod";
        #endregion
        #region カラー
        const string mainMenuColor = "menuListWindowsColor";
        private UnityEngine.UIElements.Slider sliRslider;
        private UnityEngine.UIElements.Slider sliGslider;
        private UnityEngine.UIElements.Slider sliBslider;
        private UnityEngine.UIElements.Slider tglEmission;
        #endregion

        const string mainMenuBlendShapes = "menuListWindowsBlendShapes";
        const string mainMenuIcon = "menuListWindowsIcon";
        #endregion


        #region モーションプレビュー
        private DropdownField DdlMotion;
        private UnityEngine.UIElements.Button BtnPlay;
        private UnityEngine.UIElements.Button BtnStop;

        #endregion

        private List<moduleList> moduleLists = new List<moduleList>();

        UIDocument element;

        public viewGeneralPurpose()
        {
            controller = new controllerGeneralPurpose();
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Start()
        {
            common.DebackLog("Start");
            //メインメニューバインド
            mainMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlMainBlankWindow);
            //サブメニューバインド
            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(mainMenu);

            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = selectSound;

            #region コールバック登録

            #region 機能選択ドロップダウンリストのコールバック登録
            DdlMenuView = mainUI.rootVisualElement.Q<DropdownField>(DdlMenu);
            //モジュール読込
            resultInfo result = new resultInfo();
            result = common.setDropDownField(ref DdlMenuView, ref moduleLists);
            if (object.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
            {
                DdlMenuView.RegisterValueChangedCallback(
                evt => MainMenuSetting(evt, moduleLists)
                );
            }
            //controller.DllMenu(Constants.ddlGpMenu01, ref mainMenuView);
            #endregion

            #region セーブ・ロードボタンのコールバック登録
            BtnLoadView = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.BtnLoad);
            BtnLoadView.clicked += () =>
            {

                IniLoad();
            };
            BtnSaveView = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.BtnSave);
            BtnSaveView.clicked += () =>
            {

                IniSave();
            };
            #endregion

            #region モデルプレビューボタンのコールバック登録
            DdlMotion = mainUI.rootVisualElement.Q<DropdownField>(Constants.ddlMotion);
            BtnPlay = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnPlay);
            BtnStop = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(Constants.btnStop);
            DdlMotion.RegisterValueChangedCallback(
evt => MotionMenuSetting(evt)
);
            BtnPlay.clicked += () =>
            {

                MotionPlay();
            };
            BtnStop.clicked += () =>
            {

                MotionStop();
            };
            #endregion

            #region モデル選択ボタンのコールバック登録
            BtnModelSelectView = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(BtnModelSelect);
            BtnModelSelectView.clicked += () =>
            {
                ModelSelect();
            };
            #endregion

            #region エクポートボタンのコールバック登録
            BtnExportView = mainUI.rootVisualElement.Q<UnityEngine.UIElements.Button>(BtnExport);
            BtnExportView.clicked += () =>
            {
                GameObject gameObject = null;
                vVrmLoader vVrmLoader = new vVrmLoader();
                vVrmLoader.getVrmGameObject(ref gameObject);
                modelInfo modelInfo = new modelInfo();
                vVrmLoader.getModelInfo(ref modelInfo);
                resultInfo resultInfo = new resultInfo();
                resultInfo = common.vrmLoadStatus(modelInfo, gameObject);
                if (common.ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
                {
                    MotionStop();
                    ModelExport();
                }

            };
            #endregion

            #endregion
        }

        /// <summary>
        /// メイン・サブメニューの初期化
        /// </summary>
        private void MainSubMenuReset()
        {

            if (mainMenuView.style.display != DisplayStyle.None)
            {
                mainMenuView.style.display = DisplayStyle.None;
            }
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            mainMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlMainModelInfoWindow);
            if (mainMenuView.style.display != DisplayStyle.None)
            {
                mainMenuView.style.display = DisplayStyle.None;
            }

            mainMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlMainColorWindow);
            mainMenuView.Clear();
            if (mainMenuView.style.display != DisplayStyle.None)
            {
                mainMenuView.style.display = DisplayStyle.None;
            }

            mainMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlmenuListBlendShapesWindows);
            if (mainMenuView.style.display != DisplayStyle.None)
            {
                mainMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubColorWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubIconWindows);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubThumWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelInfoWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubPersonalWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubModelLicenseTypeWindow);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubmenuListBlendShapeWindows);
            if (subMenuView.style.display != DisplayStyle.None)
            {
                subMenuView.style.display = DisplayStyle.None;
            }

            mainMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlMainBlankWindow);
            if (mainMenuView.style.display != DisplayStyle.Flex)
            {
                mainMenuView.style.display = DisplayStyle.Flex;
            }
            subMenuView = mainUI.rootVisualElement.Q<VisualElement>(Constants.pnlSubBlankWindow);
            if (subMenuView.style.display != DisplayStyle.Flex)
            {
                subMenuView.style.display = DisplayStyle.Flex;
            }

        }

        /// <summary>
        /// ドロップダウンリスト機能選択変更時
        /// </summary>
        /// <param name="evt"></param>
        private void MainMenuSetting(ChangeEvent<string> evt, List<moduleList> moduleLists)
        {
            common.DebackLog("MainMenuSetting");
            resultInfo result = new resultInfo();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            try
            {
                MainSubMenuReset();
                //mainMenuView.Clear();
                result = controller.DllMenu(evt.newValue, ref mainMenuView, ref mainUI, moduleLists);
                if (!ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                {
                    vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();

                    if (ReferenceEquals(result.resultCode, Constants.ERRORCODE301))
                    {
                        DdlMenuView.index = 0;
                        systemWindowInfo.title = "モデル未読込";
                        systemWindowInfo.message = result.resultText;
                    }
                    else
                    {
                        systemWindowInfo.title = "エラー";
                        systemWindowInfo.message = result.resultText;
                    }

                    vSystemWindow.SettingMessage(systemWindowInfo);
                    vSystemWindow.showMessageWindowAlert();
                }
                audioSource.Play();

            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
, ex
, "MainMenuSetting"
, Constants.ERRORCODE000
, Constants.ERRORTEXT000
);
                vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
                systemWindowInfo.title = "エラー";
                systemWindowInfo.message = ex.Message;
                vSystemWindow.SettingMessage(systemWindowInfo);
                vSystemWindow.showMessageWindowAlert();
            }

            //SettingMessage():
        }

        /// <summary>
        /// モデル選択押下時処理
        /// </summary>
        private void ModelSelect()
        {
            common.DebackLog("ModelSelect");
            resultInfo result = new resultInfo();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            List<modelInfo> models = new List<modelInfo>();
            try
            {
                result = controller.ModelSelect(ref models);
                if (!ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                {
                    vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
                    systemWindowInfo.title = "エラー";
                    systemWindowInfo.message = result.resultText;

                    vSystemWindow.SettingMessage(systemWindowInfo);
                    vSystemWindow.showMessageWindowAlert();
                }
                //設計図モデル一覧ウィンドウを表示する。
                common.setCameraControl(false);
                vModelSelectWindow vModelSelectWindow = modelSelectWindow.GetComponent<vModelSelectWindow>();
                vModelSelectWindow.settingModelListView(models);
                audioSource.Play();
            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
, ex
, "ModelSelect"
, Constants.ERRORCODE000
, Constants.ERRORTEXT000
);
                vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
                systemWindowInfo.title = "エラー";
                systemWindowInfo.message = ex.Message;
                vSystemWindow.SettingMessage(systemWindowInfo);
                vSystemWindow.showMessageWindowAlert();
            }
        }

        /// <summary>
        /// モデルエクスポート
        /// </summary>
        private void ModelExport()
        {
            common.DebackLog("ModelExport");

            resultInfo resultInfo = new resultInfo();

            resultInfo = controller.ModelExport();

            vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            if (resultInfo.resultCode.Equals(Constants.SUCCESSCODE000))
            {
                systemWindowInfo.title = "エクスポート完了";
                systemWindowInfo.message = resultInfo.resultText;
            }
            else
            {
                systemWindowInfo.title = "エクスポート失敗";
                systemWindowInfo.message = resultInfo.resultText;
            }
            vSystemWindow.SettingMessage(systemWindowInfo);
            vSystemWindow.showMessageWindowAlert();
        }

        /// <summary>
        /// INIファイルエクスポート
        /// </summary>
        private void IniSave()
        {
            common.DebackLog("IniSave");
            resultInfo resultInfo = new resultInfo();

            resultInfo = controller.IniSave();

            vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            if (ReferenceEquals(resultInfo.resultCode,Constants.SUCCESSCODE000))
            {
                systemWindowInfo.title = "セーブ完了";
                systemWindowInfo.message = resultInfo.resultText;
            }
            else
            {
                systemWindowInfo.title = "セーブ失敗";
                systemWindowInfo.message = resultInfo.resultText;
            }
            vSystemWindow.SettingMessage(systemWindowInfo);
            vSystemWindow.showMessageWindowAlert();

        }

        /// <summary>
        /// INIファイルインポート
        /// </summary>
        private void IniLoad()
        {
            common.DebackLog("IniLoad");
            resultInfo resultInfo = new resultInfo();

            resultInfo = controller.IniLoad();

            vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            if (ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
            {
                systemWindowInfo.title = "ロード完了";
                systemWindowInfo.message = resultInfo.resultText;
            }
            else
            {
                systemWindowInfo.title = "ロード失敗";
                systemWindowInfo.message = resultInfo.resultText;
            }
            vSystemWindow.SettingMessage(systemWindowInfo);
            vSystemWindow.showMessageWindowAlert();

        }

        public void Update()
        {
            //vVrmLoader vVrmLoader = new vVrmLoader();
            ////カラー制御
            //if (GameObject.Find("VRM") && vVrmLoader.modelInfo.modelInfoColor != null)
            //{
            //    //テクスチャへの反映
            //}
        }

        #region プレビュー系関数

        /// <summary>
        /// モデルプレビューモーション選択
        /// </summary>
        /// <param name="evt"></param>
        private void MotionMenuSetting(ChangeEvent<string> evt)
        {
            common.DebackLog("MotionMenuSetting");
            resultInfo result = new resultInfo();
            systemWindowInfo systemWindowInfo = new systemWindowInfo();
            RuntimeAnimatorController playAnime = null;
            Transform transformVrmObject = modelView.transform.Find("VRM");
            vVrmLoader vVrmLoader = modelView.GetComponent<vVrmLoader>();

            try
            {
                //vVrmLoader.getVrmGameObject(ref vrmObject);
                result = controller.MotionMenuSetting(evt.newValue, vVrmLoader, ref playAnime);
                if (!common.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                {
                    vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
                    systemWindowInfo.title = "エラー";
                    systemWindowInfo.message = result.resultText;

                    vSystemWindow.SettingMessage(systemWindowInfo);
                    vSystemWindow.showMessageWindowAlert();
                }
                if (common.ReferenceEquals(result.resultCode, Constants.SUCCESSCODE000))
                {
                    //vrmObject = modelView.Finde
                    //AnimatorController
                    Animator animatorController = transformVrmObject.GetComponent<Animator>();
                    animatorController.runtimeAnimatorController = playAnime;
                }

            }
            catch (Exception ex)
            {
                common.ErrorResultSetting(ref result
, ex
, "MotionMenuSetting"
, Constants.ERRORCODE000
, Constants.ERRORTEXT000
);
                vSystemWindow vSystemWindow = systemWindow.GetComponent<vSystemWindow>();
                systemWindowInfo.title = "エラー";
                systemWindowInfo.message = ex.Message;
                vSystemWindow.SettingMessage(systemWindowInfo);
                vSystemWindow.showMessageWindowAlert();
            }

            //SettingMessage():
        }

        /// <summary>
        /// モーション再生ボタン押下時
        /// </summary>
        private void MotionPlay()
        {
            vVrmLoader vVrmLoader = modelView.GetComponent<vVrmLoader>();
            Transform transformVrmObject = modelView.transform.Find("VRM");
            resultInfo resultInfo = new resultInfo();
            RuntimeAnimatorController playAnime = null;

            DdlMotion = mainUI.rootVisualElement.Q<DropdownField>(Constants.ddlMotion);
            resultInfo = controller.MotionMenuSetting(DdlMotion.value, vVrmLoader, ref playAnime);

            if (common.ReferenceEquals(resultInfo.resultCode, Constants.SUCCESSCODE000))
            {
                //AnimatorController
                Animator animatorController = transformVrmObject.gameObject.GetComponent<Animator>();
                animatorController.runtimeAnimatorController = playAnime;
            }
            //勝手にobjectが作られていた場合の対応
            if (transform.Find("New Game Object"))
            {
                Destroy(transform.Find("New Game Object"));
            }
        }

        private void MotionStop()
        {
            Transform transformVrmObject = modelView.transform.Find("VRM");

            //AnimatorController
            if (transformVrmObject.childCount >= 1)
            {
                Animator animatorController = transformVrmObject.gameObject.GetComponent<Animator>();
                animatorController.runtimeAnimatorController = null;
            }
        }
        #endregion
    }
}
