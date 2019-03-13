using System;
using UnityEngine;
using UnityEngine.Playables;
using XAsset;
public partial class Utility
{

    /**AssetCreate Start */
    public static GameObject LoadLevel(string name)
    {
        return Instantiate(LevelPrefabPath + name);
    }

    public static GameObject CreateUI(string name)
    {
        return Instantiate(UIPath + name);
    }
    public static GameObject Instantiate(string Path)
    {
        Asset asset = Assets.Load<GameObject>(Path + ".prefab");
        if (asset != null)
        {
            var prefab = asset.asset;
            if (prefab != null)
            {
                var go = GameObject.Instantiate(prefab) as GameObject;
                ReleaseAssetOnDestroy.Register(go, asset);
                return go;
            }
            else { throw new System.Exception("创建GO失败:" + Path); }
        }
        else { throw new System.Exception("创建GO失败:" + Path); }
    }

    public static GameObject LoadRaw(string path)
    {
        Asset asset = Assets.Load<GameObject>(path + ".prefab");
        if (asset != null)
        {
            var prefab = asset.asset;
            if (prefab != null)
            {
                return (GameObject)prefab;
            }
            else { throw new System.Exception("创建GO失败:" + path); }
        }
        else { throw new System.Exception("创建GO失败:" + path); }
    }

    public static GameObject LoadRawItem(string name)
    {
        return LoadRaw(ItemPrefabPath + name);
    }

    public static GameObject LoadRawWeapon(string name)
    {
        return LoadRaw(WeaponPrefabPath + name);
    }

    public static PlayableDirector LoadTimeline(string name)
    {
        Asset asset = Assets.Load<GameObject>(TimelinePrefabPath + name + ".prefab");
        if (asset != null)
        {
            var prefab = asset.asset;
            if (prefab != null)
            {
                var go = GameObject.Instantiate(prefab) as GameObject;
                ReleaseAssetOnDestroy.Register(go, asset);
                return go.GetComponent<PlayableDirector>();
            }
            else { throw new System.Exception("创建Unit失败:" + name); }
        }
        else { throw new System.Exception("创建Unit失败:" + name); }
    }

    public static Unit CreateUnit(string unitName, string path = null)
    {
        Asset asset = Assets.Load<GameObject>(path != null ? path : UnitPrefabPath + unitName + ".prefab");
        if (asset != null)
        {
            var prefab = asset.asset;
            if (prefab != null)
            {
                var go = GameObject.Instantiate(prefab) as GameObject;
                ReleaseAssetOnDestroy.Register(go, asset);
                return go.GetComponent<Unit>();
            }
            else { throw new System.Exception("创建Unit失败:" + unitName); }
        }
        else { throw new System.Exception("创建Unit失败:" + unitName); }
    }

    public static Font GetFont(string name)
    {
        Asset asset = Assets.Load<Font>(FontPath + name);
        if (asset != null)
        {
            Font font = (Font)asset.asset;
            if (font != null)
            {
                return font;
            }
            else { throw new System.Exception("加载字体失败:" + name); }
        }
        else { throw new System.Exception("加载字体失败:" + name); }
    }

    public static Sprite GetOtherSprite(string name)
    {
        Asset asset = Assets.Load<Sprite>(OtherSpritesPath + name + ".png");
        if (asset != null)
        {
            Sprite wanted = (Sprite)asset.asset;
            if (wanted != null)
            {
                return wanted;
            }
            else { throw new System.Exception("加载图片失败:" + name); }
        }
        else { throw new System.Exception("加载图片失败:" + name); }
    }

    public static Shader GetShader(string name)
    {
        Asset asset = Assets.Load<Shader>(ShadersPath + name + ".shader");
        if (asset != null)
        {
            Shader wanted = (Shader)asset.asset;
            if (wanted != null)
            {
                return wanted;
            }
            else { throw new System.Exception("加载Shader失败:" + name); }
        }
        else { throw new System.Exception("加载Shader失败:" + name); }
    }

    /**AssetCreate End */

}