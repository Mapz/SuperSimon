using UnityEditor;
using UnityEngine;
namespace XAsset.Editor {
    public class EditorUtility : Settings {

        [InitializeOnLoadMethod]
        static void Init () {
            Debug.Log ("Init->activeBundleMode: " + ActiveBundleMode);
            Debug.Log ("Init->activeDownloadMode: " + ActiveDownloadMode);
        }

    }
}