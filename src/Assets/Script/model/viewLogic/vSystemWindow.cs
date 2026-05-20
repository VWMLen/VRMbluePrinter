using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using vrmBluePrinter;

namespace vrmBluePrinter
{
    public class vSystemWindow : MonoBehaviour
    {
        [SerializeField]
        private UIDocument systemWindow;
        [SerializeField]
        private AudioClip OKSound;
        [SerializeField]
        private AudioClip CancelSound;

        private VisualElement allPanel;
        private VisualElement mainPanel;
        private Label titleLabel;
        private Label massegeLabel;
        private UnityEngine.UIElements.Button OKButton;
        private UnityEngine.UIElements.Button CancelButton;

        const string panelAll = "systemWindow";
        //メッセージ
        const string panelMain = "mainWindow";
        //メッセージタイトル
        const string lblTitle = "title";
        //メッセージ本文
        const string lblMessage = "message";
        //メッセージボタンOK
        const string btnOK = "btnOK";
        //メッセージボタンCancel
        const string btnCancel = "btnCancel";

        const string dummy = "dummy";

        private string strTitle = string.Empty;
        private string strMessage = string.Empty;
        private string strBtnOK = Constants.btnOKText;
        private string strBtnCancel = Constants.btnCancelText;

        public int resultCode = 0;


        void Start()
        {
            allPanel = systemWindow.rootVisualElement.Q<VisualElement>(panelAll);
            mainPanel = systemWindow.rootVisualElement.Q<VisualElement>(panelMain);
            titleLabel = systemWindow.rootVisualElement.Q<Label>(lblTitle);
            massegeLabel = systemWindow.rootVisualElement.Q<Label>(lblMessage);
            OKButton = systemWindow.rootVisualElement.Q<UnityEngine.UIElements.Button>(btnOK);
            CancelButton = systemWindow.rootVisualElement.Q<UnityEngine.UIElements.Button>(btnCancel);
            OKButton.clicked += () =>
            {
                BtnOKClicked();
            };
            CancelButton.clicked += () =>
            {
                BtnCancelClicked();
            };
            mainPanel.visible = false;
            allPanel.visible = false;
        }

        /// <summary>
        /// メッセージウィンドウ設定
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttonOK"></param>
        /// <param name="buttonCancel"></param>
        /// <returns></returns>
        public resultInfo SettingMessage(systemWindowInfo systemWindowInfo)
        {
            //common.DebackLog("SettingMessage");
            //common.DebackLog(systemWindowInfo.title);
            //common.DebackLog(systemWindowInfo.message);
            //common.DebackLog(systemWindowInfo.btnOK);
            //common.DebackLog(systemWindowInfo.btnCancel);
            resultInfo resultInfo = new resultInfo();

            cSystemWindow cSystemWindow = new cSystemWindow();
            resultInfo = cSystemWindow.SettingMessage(ref systemWindowInfo,ref titleLabel,ref massegeLabel,ref OKButton,ref CancelButton);

            return resultInfo;
        }

        /// <summary>
        /// メッセージウィンドウ表示
        /// </summary>
        public void showMessageWindow()
        {
            //common.DebackLog("showMessageWindow");
            //common.DebackLog(mainPanel.ToString());
            mainPanel.visible = true;
            allPanel.visible = true;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = OKSound;
            audioSource.Play();
        }

        /// <summary>
        /// 警告メッセージウィンドウ表示
        /// </summary>
        public void showMessageWindowAlert()
        {
            //common.DebackLog("showMessageWindow");
            //common.DebackLog(mainPanel.ToString());
            mainPanel.visible = true;
            allPanel.visible = true;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = CancelSound;
            audioSource.Play();
        }

        private void BtnOKClicked()
        {
            resultCode = Constants.btnOK;
            mainPanel.visible = false;
            allPanel.visible = false;
        }

        private void BtnCancelClicked()
        {
            resultCode = Constants.btnCancel;
            mainPanel.visible = false;
            allPanel.visible = false;
        }

    }
}
