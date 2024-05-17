using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dawn
{
    public static class UnityExtension
    {
        public static Component GetorAddComponent(this GameObject gameObject, Type type)
        {
            Component component;
            if (gameObject.TryGetComponent(type, out component))
            {
                return component;
            }
            else
            {
                return gameObject.AddComponent(type);
            }
        }

    }
}

