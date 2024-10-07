using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenIM.IMSDK.Unity.Listener;
using OpenIM.IMSDK.Unity;

namespace Dawn.Game
{
    public class CustomBusiness : ICustomBusinessListener
    {
        public void OnRecvCustomBusinessMessage(string businessMessage)
        {
        }
    }

}

