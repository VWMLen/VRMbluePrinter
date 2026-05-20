using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace vrmBluePrinter
{
    /// <summary>
    /// 設計図編集情報
    /// </summary>
    public class modelEditingInformation
    {
        /// <summary>
        /// 設計図編集情報-名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 設計図編集情報-アイコン
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 設計図編集情報-アイコン（バイナリ)
        /// </summary>
        public byte[] IconData { get; set; }

        /// <summary>
        /// 設計図編集情報-カラー設定
        /// </summary>
        public List<modelInfoEColor> modelInfoEColor { get; set; }
        /// <summary>
        /// 設計図編集情報-アイコンスクロール設定
        /// </summary>
        public modelInfoEIconScroll modelInfoEIconScroll { get; set; }
        /// <summary>
        /// 設計図編集情報-表情設定
        /// </summary>
        public List<modelInfoEShapeSetting> modelInfoEShapeSetting { get; set; }
        /// <summary>
        /// 設計図編集情報-VRM設定
        /// </summary>
        public modelInfoEVRMInfo modelInfoEVRMInfo { get; set; }
        /// <summary>
        /// 設計図編集情報-メッシュ設定
        /// </summary>
        public List<modelInfoESettingMesh> modelInfoESettingMesh { get; set; }

        public modelEditingInformation()
        {
            Name = string.Empty;
            Icon = string.Empty;
            IconData = null;

            modelInfoEColor = null;
            modelInfoEIconScroll = null;
            modelInfoEShapeSetting = null;
            modelInfoEVRMInfo = null;
            modelInfoESettingMesh = null;
        }

    }

    /// <summary>
    /// 設計図編集情報-カラー設定
    /// </summary>
    public class modelInfoEColor
    {
        /// <summary>
        /// カラー設定-カラーコード
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// カラー設定-エミッションの有無
        /// (有=true)
        /// </summary>
        public bool Emission { get; set; }


        public modelInfoEColor()
        {
            Color = string.Empty;
            Emission = false;
        }
    }

    /// <summary>
    /// 設計図編集情報-アイコンスクロール設定
    /// </summary>
    public class modelInfoEIconScroll
    {
        /// <summary>
        /// アイコンスクロール設定-スクロールX値
        /// </summary>
        public int IconScrollX { get; set; }
        /// <summary>
        /// アイコンスクロール設定-スクロールY値
        /// </summary>
        public int IconScrollY { get; set; }

        public modelInfoEIconScroll()
        {
            IconScrollX = 0;
            IconScrollY = 0;
        }
    }

    /// <summary>
    /// 設計図編集情報-表情設定
    /// </summary>
    public class modelInfoEShapeSetting
    {
        /// <summary>
        /// 表情設定-表情名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表情設定-ブレンドシェイプ設定
        /// </summary>
        public List<modelInfoEBlendShapeSetting> BlendShapeSetting { get; set; }

        public modelInfoEShapeSetting()
        {
            Name = string.Empty;
            BlendShapeSetting = null;
        }
    }

    /// <summary>
    /// 設計図編集情報-ブレンシェイプ設定
    /// </summary>
    public class modelInfoEBlendShapeSetting
    {
        /// <summary>
        /// ブレンシェイプ設定-ブレンドシェイプ名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ブレンシェイプ設定-ブレンドシェイプ値
        /// </summary>
        public int BlendShape { get; set; }

        public modelInfoEBlendShapeSetting()
        {
            Name = string.Empty;
            BlendShape = 0;
        }
    }

    /// <summary>
    /// 設計図編集情報-VRM設定
    /// </summary>
    public class modelInfoEVRMInfo
    {
        /// <summary>
        /// VRM設定-サムネイル
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// VRM設定-サムネイル（バイナリ）
        /// </summary>
        public byte[] ThumbnailData { get; set; }
        /// <summary>
        /// VRM設定-タイトル
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// VRM設定-バージョン
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// VRM設定-作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// VRM設定-連絡情報
        /// </summary>
        public string ContactInfo { get; set; }
        /// <summary>
        /// VRM設定-参照
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// VRM設定-人格許諾範囲
        /// </summary>
        public int VRMppr { get; set; }
        /// <summary>
        /// VRM設定-暴力表現の許可
        /// </summary>
        public int VRMvp { get; set; }
        /// <summary>
        /// VRM設定-性的表現の許可
        /// </summary>
        public int VRMsp { get; set; }
        /// <summary>
        /// VRM設定-商用利用の許可
        /// </summary>
        public int VRMcup { get; set; }
        /// <summary>
        /// VRM設定-Other Permission Url
        /// </summary>
        public string VRMopu { get; set; }
        /// <summary>
        /// VRM設定-License Type
        /// </summary>
        public int VRMlt { get; set; }

        public modelInfoEVRMInfo()
        {
            Thumbnail = string.Empty;
            ThumbnailData = null;
            Title = string.Empty;
            Version = string.Empty;
            Author = string.Empty;
            ContactInfo = string.Empty;
            Reference = string.Empty;

            VRMppr = 0;
            VRMvp = 0;
            VRMsp = 0;
            VRMcup = 0;
            VRMopu = string.Empty;
            VRMlt = 0;
        }
    }

    /// <summary>
    /// 設計図編集情報-メッシュ設定
    /// </summary>
    public class modelInfoESettingMesh
    {
        /// <summary>
        /// 設計図編集情報-メッシュ名
        /// </summary>
        public string MeshName { get; set; }
        public modelInfoESettingMaterial Materials { get; set; }

        public modelInfoESettingMesh()
        {
            MeshName = string.Empty;
            Materials = null;
        }

    }

    /// <summary>
    /// 設計図編集情報-メッシュ設定-マテリアル設定
    /// </summary>
    public class modelInfoESettingMaterial
    {
        /// <summary>
        /// 設計図編集情報-マテリアル名
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 設計図編集情報-マテリアルがアイコンか
        /// 1=Yes
        /// </summary>
        public int MaterialIcon { get; set; }


        public modelInfoESettingMaterial()
        {
            MaterialName = string.Empty;
            MaterialIcon = 0;
        }

    }

}
