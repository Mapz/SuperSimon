using UnityEngine;

namespace XAsset {
    public class Settings {

        public const string AssetBundlesOutputPath = "AssetBundles";
        public static string AssetBunldesDownloadPath = Application.persistentDataPath + "/Bundles";
        public const string AssetBundlesOutputPathForDownload = "AssetBundlsForDownload";

        
        public static int activeBundleMode = -1;
        const string kActiveBundleMode = "ActiveBundleMode";
        public static bool ActiveBundleMode {
            get {
                if (activeBundleMode == -1)
                    activeBundleMode = PlayerPrefs.GetInt (kActiveBundleMode, 0);
                return activeBundleMode != 0;
            }
            set {
                int newValue = value ? 1 : 0;
                if (newValue != activeBundleMode) {
                    activeBundleMode = newValue;
                    PlayerPrefs.SetInt (kActiveBundleMode, newValue);
                }
            }
        }

        public static int activeDownloadMode = -1;
        const string kActiveDownloadMode = "ActiveDownloadMode";
        public static bool ActiveDownloadMode {
            get {
                if (activeDownloadMode == -1)

                    activeDownloadMode = PlayerPrefs.GetInt (kActiveDownloadMode, 0);
                return activeDownloadMode != 0;
            }
            set {
                int newValue = value ? 1 : 0;
                if (newValue != activeDownloadMode) {
                    activeDownloadMode = newValue;
                    PlayerPrefs.SetInt (kActiveDownloadMode, newValue);
                }
            }
        }
    }
}