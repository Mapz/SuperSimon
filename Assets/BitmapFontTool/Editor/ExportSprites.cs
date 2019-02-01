using UnityEditor;
using UnityEngine;
static class BitmapFontTool {
    [MenuItem ("BitmapFontTool/导出切好的Sprite")]
    static void SaveSprite () {
        //每一张贴图类型Advanced下 Read/Write Enabled打上勾才能进行文件读取
        //导入类型不能是压缩过的
        string bitMapfontPath = @"Assets/BitmapFontTool/";
        foreach (Object obj in Selection.objects) {

            string selectionPath = AssetDatabase.GetAssetPath (obj);

            // 必须最上级是"Assets/Resources/"
            // if (selectionPath.StartsWith (resourcesPath)) {
            //获取文件后罪名.png
            string selectionExt = System.IO.Path.GetExtension (selectionPath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension (selectionPath);
            string loadPath = selectionPath.Substring (0, selectionPath.Length - selectionExt.Length);
            if (selectionExt.Length == 0) continue;

            //加载此文件下的所有资源
            Object[] spriteList = AssetDatabase.LoadAllAssetsAtPath (selectionPath); //AssetDatabase.LoadAllAssetsAtPath (loadPath);

            if (spriteList.Length > 0) {
                //创建导出文件夹
                string outPath = bitMapfontPath + "/SpriteExportOutput/" + fileName;
                System.IO.Directory.CreateDirectory (outPath);

                foreach (var sprite in spriteList) {
                    try {
                        var sprite_ = (Sprite) sprite;
                        Texture2D tex = new Texture2D ((int) sprite_.rect.width, (int) sprite_.rect.height, sprite_.texture.format, false);
                        tex.SetPixels (sprite_.texture.GetPixels ((int) sprite_.rect.xMin, (int) sprite_.rect.yMin, (int) sprite_.rect.width, (int) sprite_.rect.height));
                        tex.Apply ();
                        //写出成png文件
                        System.IO.File.WriteAllBytes (outPath + "/" + sprite_.name + ".png", tex.EncodeToPNG ());
                        Debug.Log ("SaveSprite to" + outPath);
                    } catch (System.Exception e) {
                        Debug.Log (e);
                    }

                }
                Debug.Log ("保存图片完毕!" + outPath);
            }
        }
        // }
    }
}