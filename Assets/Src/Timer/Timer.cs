using System;
using UnityEngine;

namespace Dawn
{
    public enum TimeUnit
    {
        MILLSECOND, //毫秒
        SECOND,     //秒
        MINUTE,     //分钟
        HOUR,       //小时
    }
    /// <summary>
    /// 定时器
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// 计时结束回调事件
        /// </summary>
        public event Action OnEnd;
        /// <summary>
        /// 计时更新事件
        /// </summary>
        public event Action<float> OnUpdate;
        //定时时长
        private float duration;
        //开始计时时间
        private float beginTime;
        //已计时间
        private float elapsedTime;
        //缓存时间（用于暂停）
        private float cacheTime;
        //是否循环
        private bool loop;
        //是否暂停
        private bool isPaused;
        //是否计时完成
        bool isFinished;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="duration">定时时长</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="loop">是否循环</param>
        public Timer(float duration, TimeUnit timeUnit = TimeUnit.SECOND, bool loop = false)
        {
            switch (timeUnit)
            {
                case TimeUnit.MILLSECOND: this.duration = duration / 1000; break;
                case TimeUnit.SECOND: this.duration = duration; break;
                case TimeUnit.MINUTE: this.duration = duration * 60; break;
                case TimeUnit.HOUR: this.duration = duration * 3600; break;
            }
            this.loop = loop;
            beginTime = Time.realtimeSinceStartup;
        }
        /// <summary>
        /// 更新定时器
        /// </summary>
        public void Update()
        {
            if (!isFinished && !isPaused)
            {
                elapsedTime = Time.realtimeSinceStartup - beginTime;
                OnUpdate?.Invoke(Mathf.Clamp(elapsedTime, 0, duration));
                if (elapsedTime >= duration)
                {
                    OnEnd?.Invoke();
                    if (loop)
                    {
                        beginTime = Time.realtimeSinceStartup;
                    }
                    else
                    {
                        isFinished = true;
                    }
                }
            }
        }
        /// <summary>
        /// 暂停定时器
        /// </summary>
        public void Pause()
        {
            if (!isFinished)
            {
                isPaused = true;
                cacheTime = Time.realtimeSinceStartup;
            }
        }
        /// <summary>
        /// 恢复定时器
        /// </summary>
        public void Unpause()
        {
            if (!isFinished && isPaused)
            {
                beginTime += Time.realtimeSinceStartup - cacheTime;
                isPaused = false;
            }
        }
        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            isFinished = true;
        }
        public bool IsFinished()
        {
            return isFinished;
        }
    }


}

