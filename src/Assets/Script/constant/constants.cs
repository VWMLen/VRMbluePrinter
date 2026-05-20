using System.Collections;
using System.Collections.Generic;

namespace vrmBluePrinter
{
    public static class Constants
    {
        #region 定数

        #region リザルトコード
        /// <summary>
        /// ER異常終了
        /// </summary>
        public const string ERRORCODE000 = "9000";

        /// <summary>
        /// SU正常終了
        /// </summary>
        public const string SUCCESSCODE000 = "0000";

        #region XML 9100-9199
        /// <summary>
        /// ER設計図データ
        /// </summary>
        public const string ERRPRCODE100 = "9100";
        /// <summary>
        /// ER設計図モデル
        /// </summary>
        public const string ERRORCODE101 = "9101";

        #endregion

        #region ini 9200-9299
        /// <summary>
        /// ERセーブ
        /// </summary>
        public const string ERRPRCODE200 = "9200";
        /// <summary>
        /// ERロード
        /// </summary>
        public const string ERRORCODE201 = "9201";
        /// <summary>
        /// ERini項目
        /// </summary>
        public const string ERRPRCPDE202 = "9202";

        #endregion

        #region etc 9300-
        /// <summary>
        /// ERエクスポート
        /// </summary>
        public const string ERRPRCODE300 = "9300";

        /// <summary>
        /// ERモデル未読込
        /// </summary>
        public const string ERRORCODE301 = "9301";

        #endregion

        #endregion

        #region リザルトテキスト
        /// <summary>
        /// ER異常終了
        /// </summary>
        public const string ERRORTEXT000 = "異常終了：{0}";
        /// <summary>
        /// SU正常終了
        /// </summary>
        public const string SUCCESSTEXT000 = "正常完了";

        #region XML 9100-9199
        /// <summary>
        /// ER設計図データ
        /// </summary>
        public const string ERRORTEXT100 = "設計図データが読み込めませんでした。：{0}";
        /// <summary>
        /// ER設計図モデル
        /// </summary>
        public const string ERRORTEXT101 = "設計図モデルが読み込めませんでした。：{0}";

        #endregion

        #region ini 9200-9299
        /// <summary>
        /// ERセーブ
        /// </summary>
        public const string ERRORTEXT200 = "編集情報をセーブできませんでした。：{0}";
        /// <summary>
        /// ERロード
        /// </summary>
        public const string ERRORTEXT201 = "編集情報をロードできませんでした。：{0}";
        /// <summary>
        /// ERini項目
        /// </summary>
        public const string ERRORTEXT202 = "編集情報項目が存在しませんでした。：{0}";

        #endregion

        #region etc 9300-
        /// <summary>
        /// ERエクスポート
        /// </summary>
        public const string ERRORTEXT300 = "エクスポートに失敗しました。：{0}";

        /// <summary>
        /// ERモデル未読込
        /// </summary>
        public const string ERRORTEXT301 = "モデルを先に読み込んでください。";


        #endregion

        #endregion

        #region XMLタグ
        public const string XML_settings = "settings";

        public const string XML_VRM = "VRM";
        public const string XML_VRM_name = "name";
        public const string XML_VRM_icon = "icon";

        public const string XML_VRM_Colors = "Colors";
        public const string XML_VRM_Colors_Color = "Color";
        public const string XML_VRM_Colors_Color_color = "color";
        public const string XML_VRM_Colors_Color_emission = "emission";

        public const string XML_IconScroll = "IconScroll";
        public const string XML_IconScroll_iconScrollX = "iconScrollX";
        public const string XML_IconScroll_iconScrollY = "iconScrollY";

        public const string XML_VRMSettings = "VRMSettings";
        public const string XML_VRMSettings_VRMthumbnail = "VRMthumbnail";
        public const string XML_VRMSettings_VRMtitle = "VRMtitle";
        public const string XML_VRMSettings_VRMversion = "VRMversion";
        public const string XML_VRMSettings_VRMauthor = "VRMauthor";
        public const string XML_VRMSettings_VRMcontactInfo = "VRMcontactInfo";
        public const string XML_VRMSettings_VRMreference = "VRMreference";
        public const string XML_VRMSettings_VRMpp = "VRMpp";
        public const string XML_VRMSettings_VRMpd = "VRMpd";
        public const string XML_VRMSettings_VRMsp = "VRMsp";
        public const string XML_VRMSettings_VRMcup = "VRMcup";
        public const string XML_VRMSettings_VRMopu = "VRMopu";
        public const string XML_VRMSettings_VRMit = "VRMit";

        public const string XML_Meshes = "Meshes";
        public const string XML_Meshes_Mesh = "Mesh";
        public const string XML_Meshes_Mesh_MeshName = "MeshName";
        public const string XML_Meshes_Mesh_Materials = "Materials";
        public const string XML_Meshes_Mesh_Materials_Material = "Material";
        public const string XML_Meshes_Mesh_Materials_Material_MaterialName = "MaterialName";
        public const string XML_Meshes_Mesh_Materials_Material_MaterialIcon = "MaterialIcon";

        #endregion

        #endregion

        #region view定数
        public const string btnOKText = "OK";
        public const string btnCancelText = "Cancel";
        public const int btnOK = 1;
        public const int btnCancel = 2;

        /// <summary>
        /// 選択肢 モデル情報
        /// </summary>
        public const string ddlGpMenu01 = "モデル情報";
        /// <summary>
        /// 選択肢 カラー
        /// </summary>
        public const string ddlGpMenu02 = "カラー";
        /// <summary>
        /// 選択肢 表情
        /// </summary>
        public const string ddlGpMenu03 = "表情";
        /// <summary>
        /// 選択肢 アイコン
        /// </summary>
        public const string ddlGpMenu04 = "アイコン";

        #region メインメニューサイド
        public const string pnlMainBlankWindow = "menuListWindows";
        #region メインメニューモデル情報
        public const string pnlMainModelInfoWindow = "menuListWindowsModelInfo";
        public const string btnThumbnail = "btnThumbnail";
        public const string btnInfomation = "btnInfomation";
        public const string btnParsonal = "btnParsonal";
        public const string btnRedistMod = "btnRedistMod";
        #endregion
        public const string pnlMainColorWindow = "menuListWindowsColor";
        #region メインメニューアイコン
        public const string pnlMainIconWindow = "menuListWindowsIcon";
        public const string btnIconSet = "btnIconSet";
        #endregion
        #region メインメニュー表情
        public const string pnlmenuListBlendShapesWindows = "menuListWindowsBlendShapes";
        public const string rbgBlendShape = "rbgBlendShape";
        public const string rdoNeutral = "rdoNeutral";
        public const string rdoJoy = "rdoJoy";
        public const string rdoAngry = "rdoAngry";
        public const string rdoSorrow = "rdoSorrow";
        public const string rdoFun = "rdoFun";
        #endregion

        public const string pnlMainOptionWindow = "menuOptionWindow";
        #endregion

        #region サブメニューサイド
        public const string pnlSubBlankWindow = "submenuListWindows";

        #region サブメニューモデル情報
        #region サブメニューモデル情報-サムネ
        public const string pnlSubThumWindow = "submenuListModelThumWindows";
        public const string btnThumFileLoad = "btnThumFileLoad";
        #endregion
        #region サブメニューモデル情報-モデル情報
        public const string pnlSubModelInfoWindow = "submenuListModelInfoWindows";
        public const string txtModelInfoTitle = "txtModelInfoTitle";
        public const string txtModelInfoVersion = "txtModelInfoVersion";
        public const string txtModelInfoAuthor = "txtModelInfoAuthor";
        public const string txtModelInfoContactInfo = "txtModelInfoContactInfo";
        public const string txtModelInfoReference = "txtModelInfoReference";
        #endregion
        #region サブメニューモデル情報-人格許容範囲情報
        public const string pnlSubPersonalWindow = "submenuListModelPersonalWindows";
        public const string ddlPpr = "ddlPpr";
        public const string ddlVp = "ddlVp";
        public const string ddlSp = "ddlSp";
        public const string ddlCup = "ddlCup";
        public const string txtOpu = "txtOpu";
        #endregion
        #region サブメニューモデル情報-ライセンス種類情報
        public const string pnlSubModelLicenseTypeWindow = "submenuListModelLicenseTypeWindows";
        public const string groupLicenseRdo = "groupLicenseRdo";
        public const string rdoRedistributionProhibited = "rdoRedistributionProhibited";
        public const string rdoCC0 = "rdoCC0";
        public const string rdoCCBY = "rdoCCBY";
        public const string rdoCCBYNC = "rdoCCBYNC";
        public const string rdoCCBYSA = "rdoCCBYSA";
        public const string rdoCCBYNCSA = "rdoCCBYNCSA";
        public const string rdoCCBYND = "rdoCCBYND";
        public const string rdoCCBYNCND = "rdoCCBYNCND";
        public const string rdoOther = "rdoOther";
        #endregion
        #endregion

        #region サブメニューカラー
        public const string pnlSubColorWindow = "submenuListModelColorWindows";
        public const string pnlColorView = "pnlColorView";
        public const string sliRslider = "sliRslider";
        public const string sliGslider = "sliGslider";
        public const string sliBslider = "sliBslider";
        public const string tglEmission = "tglEmission";
        public const string btnApply = "btnApply";
        public const string txtSelectTarget = "txtSelectTarget";
        #endregion

        #region サブメニューアイコン
        public const string pnlSubIconWindows = "submenuListModelIconWindows";
        public const string btnIconLoad = "btnIconLoad";
        public const string sliIconX = "sliIconX";
        public const string sliIconY = "sliIconY";
        public const string btnIconApply = "btnIconApply";
        #endregion

        #region サブメニュー表情
        public const string pnlSubmenuListBlendShapeWindows = "submenuListBlendShapeWindows";
        public const string txtTargetType = "txtTargetType";
        public const string scrShapeKey = "scrShapeKey";
        public const string btnBlendShapeApply = "btnBlendShapeApply";

        public const string lblBlendShape = "lblBlendShape{0}";
        public const string fSlideBlendShape = "fSlideBlendShape{0}";
        public const string txtBlendShape = "txtBlendShape{0}";
        #endregion

        #endregion

        #region セーブ・ロード
        public const string BtnSave = "BtnSave";
        public const string BtnLoad = "BtnLoad";

        #endregion

        #region モーションプレビュー
        public const string ddlMotion = "DdlMotion";
        public const string btnPlay = "BtnPlay";
        public const string btnStop = "BtnStop";
        #endregion

        #endregion

        #region 文字設定
        public const string extendVRM = ".vrm";
        public const string extendXML = ".xml";
        public const string blendshapeSetting = ".BlendShapes\\BlendShape.{0}.asset";
        public const string VrmBlendShapeClip = "BlendShape.{0}";

        #endregion

        #region VrmView定数
        public const float v3TX = 0f;
        public const float v3TY = -3f;
        public const float v3TZ = 0f;

        public const float v3RX = 0f;
        public const float v3RY = 180f;
        public const float v3RZ = 0f;

        public const float v3SX = 3f;
        public const float v3SY = 3f;
        public const float v3SZ = 3f;


        #endregion
    }

}
