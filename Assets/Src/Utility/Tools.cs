using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dawn
{

    /// 全局工具类
    public static class Tools
    {
        #region Transform
        public static Vector2 cacheVec2;
        public static Vector3 cacheVec3;
        public static void SetPosition(Transform trans, float x, float y, float z, bool world = false)
        {
            Debug.Assert(trans != null);
            cacheVec3.Set(x, y, z);
            if (world)
            {
                trans.position = cacheVec3;
            }
            else
            {
                trans.localPosition = cacheVec3;
            }
        }
        public static void SetScale(Transform trans, float x, float y, float z)
        {
            Debug.Assert(trans != null);
            cacheVec3.Set(x, y, z);
            trans.localScale = cacheVec3;
        }
        public static void SetEuler(Transform trans, float x, float y, float z, bool world = false)
        {
            Debug.Assert(trans != null);
            cacheVec3.Set(x, y, z);
            if (world)
            {
                trans.eulerAngles = cacheVec3;
            }
            else
            {
                trans.localEulerAngles = cacheVec3;
            }
        }
        public static void Rotate(Transform trans, float x, float y, float z)
        {
            Debug.Assert(trans != null);
            cacheVec3.Set(x, y, z);
            trans.Rotate(cacheVec3, Space.Self);
        }

        public static void SetUIPosition(RectTransform trans, float x, float y, float z, bool world = false)
        {
            Debug.Assert(trans != null);
            cacheVec3.Set(x, y, z);
            if (world)
            {
                trans.position = cacheVec3;
            }
            else
            {
                trans.localPosition = cacheVec3;
            }
        }
        public static void SetUIAnchorPosition(RectTransform trans, float x, float y)
        {
            cacheVec2.Set(x, y);
            trans.anchoredPosition = cacheVec2;
        }
        #endregion

        #region Physics
        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layerMask, UnityAction<RaycastHit> callBack)
        {
            RaycastHit hit;
            var suc = Physics.CapsuleCast(point1, point2, radius, direction, out hit, maxDistance, layerMask);
            if (suc && callBack != null)
            {
                callBack(hit);
            }
            return suc;
        }
        public static bool LineCast(Vector3 start, Vector3 end, int layerMask, UnityAction<RaycastHit> callBack)
        {
            RaycastHit hit;
            var suc = Physics.Linecast(start, end, out hit, layerMask);
            if (suc && callBack != null)
            {
                callBack(hit);
            }
            return suc;
        }
        public static bool RayCast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, UnityAction<RaycastHit> callBack)
        {
            RaycastHit hit;
            var suc = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
            if (suc && callBack != null)
            {
                callBack(hit);
            }
            return suc;
        }
        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask, UnityAction<RaycastHit> callBack)
        {
            RaycastHit hit;
            var suc = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask);
            if (suc && callBack != null)
            {
                callBack(hit);
            }
            return suc;
        }
        #endregion

        #region  粒子系统 
        public static float GetParticleSystemMaxDuration(Transform trans)
        {
            var pss = trans.GetComponentsInChildren<ParticleSystem>();
            if (pss.Length <= 0)
            {
                return -1.0f;
            }
            float duration = Mathf.NegativeInfinity;
            for (int i = 0; i < pss.Length; i++)
            {
                var ps = pss[i];
                if (ps.main.loop){
                    return -1.0f;
                }else{
                    if(ps.main.duration > duration){
                        duration = ps.main.duration;
                    }
                }
            }
            return duration;
        }
        #endregion

        #region  动画
        public static float GetAnimationDuration(Animator animator,string animName){
            float duration = -1;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips){
                if (clip.name.Equals(animName)){
                    duration = clip.length;
                }
            }
            return duration;
        }
        #endregion
    }
}

