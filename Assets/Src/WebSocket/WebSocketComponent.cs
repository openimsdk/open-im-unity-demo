using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Resource;
using UnityWebSocket;

namespace Dawn
{
    public class WebSocketComponent : GameFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
        }
        public WebSocketHelper AddWebSocket(string url){
            return new WebSocketHelper(url);
        }

        public bool HasWebSocketInConnecting(string url){
            return false;
        }


    }
}


