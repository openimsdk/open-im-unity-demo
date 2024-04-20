using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk.listener;
using open_im_sdk;

namespace Dawn.Game
{
    public class User : IUserListener
    {
        public void OnSelfInfoUpdated(LocalUser userInfo)
        {
        }

        public void OnUserStatusChanged(OnlineStatus userOnlineStatus)
        {
        }
    }

}

