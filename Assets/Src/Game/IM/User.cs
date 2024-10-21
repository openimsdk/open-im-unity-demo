using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenIM.IMSDK.Unity.Listener;
using OpenIM.IMSDK.Unity;

namespace Dawn.Game
{
    public class User : IUserListener
    {
        public void OnSelfInfoUpdated(UserInfo userInfo)
        {
        }

        public void OnUserCommandAdd(string userCommand)
        {
        }

        public void OnUserCommandDelete(string userCommand)
        {
        }

        public void OnUserCommandUpdate(string userCommand)
        {
        }

        public void OnUserStatusChanged(OnlineStatus userOnlineStatus)
        {
        }
    }

}

