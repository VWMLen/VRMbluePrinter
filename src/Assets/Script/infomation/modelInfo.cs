using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace vrmBluePrinter
{
    /// <summary>
    /// 設計図設定
    /// </summary>
    public class modelInfo
    {
        /// <summary>
        /// モデル情報-名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// モデル情報-アイコン
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// モデル情報-アイコン（バイナリ)
        /// </summary>
        public byte[] IconData { get; set; }

        /// <summary>
        /// モデル情報-ディレクトリ
        /// </summary>
        public string Directory {  get; set; }

        /// <summary>
        /// モデル情報-VRMディレクトリ
        /// </summary>
        public string VrmDirectory {  get; set; }

        /// <summary>
        /// モデル情報-カラー設定
        /// </summary>
        public List<modelInfoColor> modelInfoColor { get; set; }
        /// <summary>
        /// モデル情報-アイコンスクロール設定
        /// </summary>
        public modelInfoIconScroll modelInfoIconScroll { get; set; }
        /// <summary>
        /// モデル情報-VRM設定
        /// </summary>
        public modelInfoVRMInfo modelInfoVRMInfo { get; set; }
        /// <summary>
        /// モデル情報-メッシュ設定
        /// </summary>
        public List<modelInfoSettingMesh> modelInfoSettingMesh { get; set; }

        public modelInfo()
        {
            Name = string.Empty;
            Icon = string.Empty;
            IconData = null;
            Directory = string.Empty;

            modelInfoColor = null;
            modelInfoIconScroll = null;
            modelInfoVRMInfo = null;
            modelInfoSettingMesh = null;
        }

    }

    /// <summary>
    /// モデル情報-カラー設定
    /// </summary>
    public class modelInfoColor
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


        public modelInfoColor()
        {
            Color = string.Empty;
            Emission = false;
        }
    }

    /// <summary>
    /// モデル情報-アイコンスクロール設定
    /// </summary>
    public class modelInfoIconScroll
    {
        /// <summary>
        /// アイコンスクロール設定-スクロールX値
        /// </summary>
        public int IconScrollX { get; set; }
        /// <summary>
        /// アイコンスクロール設定-スクロールY値
        /// </summary>
        public int IconScrollY { get; set; }

        public modelInfoIconScroll()
        {
            IconScrollX = 0;
            IconScrollY = 0;
        }
    }

    /// <summary>
    /// モデル情報-VRM設定
    /// </summary>
    public class modelInfoVRMInfo
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

        public modelInfoVRMInfo()
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
    /// モデル情報-メッシュ設定
    /// </summary>
    public class modelInfoSettingMesh
    {
        /// <summary>
        /// モデル情報-メッシュ名
        /// </summary>
        public string MeshName { get; set; }
        public List<modelInfoSettingMaterial> Materials { get; set; }

        public modelInfoSettingMesh()
        {
            MeshName = string.Empty;
            Materials = null;
        }

    }

    /// <summary>
    /// モデル情報-メッシュ設定-マテリアル設定
    /// </summary>
    public class modelInfoSettingMaterial
    {
        /// <summary>
        /// モデル情報-マテリアル名
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// モデル情報-マテリアルがアイコンか
        /// 1=Yes
        /// </summary>
        public int MaterialIcon { get; set; }


        public modelInfoSettingMaterial()
        {
            MaterialName = string.Empty;
            MaterialIcon = 0;
        }

    }

}
