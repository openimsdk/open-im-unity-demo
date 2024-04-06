//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace Dawn
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        public static int? PlayMusic(this SoundComponent soundComponent, string assetName)
        {
            soundComponent.StopMusic();
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.FadeInSeconds = FadeVolumeDuration;
            playSoundParams.SpatialBlend = 0f;
            s_MusicSerialId = soundComponent.PlaySound(assetName, "Music",playSoundParams);
            return s_MusicSerialId;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }
            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }

        public static int? PlaySound(this SoundComponent soundComponent, string assetName)
        {
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            // 优先级 默认0，128最高，-128最低
            playSoundParams.Priority = 128;
            // 循环
            playSoundParams.Loop = false;
            // 音量 0 ~ 1
            playSoundParams.VolumeInSoundGroup = 1.0f;
            // 声音空间混合量（0为2D，1为3D，中间值混合效果）
            playSoundParams.SpatialBlend = 0.0f;
            playSoundParams.MaxDistance = 100.0f;
            return soundComponent.PlaySound(assetName, "Sound",playSoundParams);
        }

        public static int? PlayUISound(this SoundComponent soundComponent, string assetName)
        {
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 0;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1.0f;
            playSoundParams.SpatialBlend = 0f;
            return soundComponent.PlaySound(assetName, "UISound",playSoundParams);
        }

        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return true;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }

            return soundGroup.Mute;
        }

        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;
            // 保存静音配置
            // GameEntry.Setting.SetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            // GameEntry.Setting.Save();
        }

        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;
            // 保存配置
            // GameEntry.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            // GameEntry.Setting.Save();
        }
    }
}
