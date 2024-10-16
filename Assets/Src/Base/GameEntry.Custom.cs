
using Dawn.Game;
using OpenIM.IMSDK.Unity;
using UnityEngine;

namespace Dawn
{

    public partial class GameEntry : MonoBehaviour
    {
        public static TimerComponent Timer
        {
            get;
            private set;
        }
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static NetResourceComponent NetResource
        {
            get;
            private set;
        }

        public static WebSocketComponent WebSocket
        {
            get;
            private set;
        }

        public static SpriteAltasComponent SpriteAltas
        {
            get;
            private set;
        }

        public static IMComponent IM
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            NetResource = UnityGameFramework.Runtime.GameEntry.GetComponent<NetResourceComponent>();
            WebSocket = UnityGameFramework.Runtime.GameEntry.GetComponent<WebSocketComponent>();
            SpriteAltas = UnityGameFramework.Runtime.GameEntry.GetComponent<SpriteAltasComponent>();
            IM = UnityGameFramework.Runtime.GameEntry.GetComponent<IMComponent>();

        }
    }

}