using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
namespace Dawn
{
    public class NetResourceComponent : GameFrameworkComponent
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public void SetImage(Image image, string url)
        {
            StartCoroutine(LoadTexture(image, url));
        }
        Texture2D texture2D;
        IEnumerator LoadTexture(Image image, string url)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("图片加载失败" + uwr.error);
                }
                else
                {
                    texture2D = DownloadHandlerTexture.GetContent(uwr);
                    Sprite temp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.width), Vector2.zero);
                    image.sprite = temp;
                }
            }
        }
        IEnumerator UpLoadTexture(string url, byte[] bytes)
        {
            WWWForm form = new WWWForm();
            string id = "Photo_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            form.AddField("id", id);
            form.AddBinaryData("Photo", bytes, "photo.jpg");
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("上传失败:" + www.error);
                }
                else
                {
                    string text = www.downloadHandler.text;
                    Debug.Log("服务器返回值" + text);
                    Debug.Log("上传成功！");
                }
            }
        }
    }
}

