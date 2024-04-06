using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using GameFramework.Resource;
using System.IO;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
namespace Dawn
{
    public class SpriteAltasComponent: GameFrameworkComponent 
    {
        public List<string> SpriteAltasList;
        int leftLoadSpriteAltasCount = 0;
        Dictionary<string,SpriteAtlas> spriteAltasDic = new Dictionary<string, SpriteAtlas>();
        void Start()
        {

        }

        public void LoadSpriteAtlas(){
            leftLoadSpriteAltasCount = SpriteAltasList.Count;
            foreach(string spritealtasName in SpriteAltasList){
                GameEntry.Resource.LoadAsset(spritealtasName,typeof(SpriteAtlas),new LoadAssetCallbacks(LoadAssetSuccessCallback,LoadAssetFailureCallback));
            }
        }

        protected void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData){
            var name = Path.GetFileNameWithoutExtension(assetName);
            var sp = asset as SpriteAtlas;
            spriteAltasDic.Add(name,sp);
            Debug.Log(string.Format("<color=red>{0}</color>","load atlas " + name + " success" + " Count = "  + sp.spriteCount));
            leftLoadSpriteAltasCount--;
        }

        public bool IsLoadSpriteAltasDone(){
            return leftLoadSpriteAltasCount <= 0;
        }

        protected void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData){
            Debug.Log(errorMessage);
        }

        protected Sprite GetSprite(string atlas,string spritePath){
            SpriteAtlas sa = null;
            var suc = spriteAltasDic.TryGetValue(atlas,out sa);
            if (suc){
                var sprite = sa.GetSprite(spritePath);
                if (sprite == null){
                    Debug.LogError(string.Format("{0}.spritealtas no Sprite -> {1}",atlas,spritePath));
                }
                return sprite;
            }else{
                Debug.LogError("Cant load SpriteAtlas " + atlas);
            }
            return null;
        }

        public void SetSprite(Image image,string url){
            if(string.IsNullOrEmpty(url)){
                return;
            }
            string[] sprite_name = url.Split('/');
            if (sprite_name.Length != 2){
                return;
            }
            var spite = GetSprite(sprite_name[0],sprite_name[1]);
            image.sprite = spite;
        }
    }
}

