using System.Collections.Generic;
using Dawn.Game.Event;
using OpenIM.IMSDK.Unity;
using OpenIM.IMSDK.Unity.Listener;
using UnityEngine;
namespace Dawn.Game
{
    public class Conn : IConnListener
    {
        public Conn()
        {
        }

        public void OnConnecting()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.OnConnecting
            });
        }

        public void OnConnectSuccess()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.ConnSuc
            });
        }
        public void OnConnectFailed(int errCode, string errMsg)
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.ConnFailed
            });
        }
        public void OnKickedOffline()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.KickOffline
            });
        }
        public void OnUserTokenExpired()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.TokenExpired
            });
        }


        public void OnUserTokenInvalid(string errMsg)
        {
            // TODO
        }
    }
}

