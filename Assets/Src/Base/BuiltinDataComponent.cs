using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Dawn
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        public string GameVersion = "0.0.0";
        public int InternalGameVersion = 1;
        public string CheckVersionUrl = "http://127.0.0.1:11006/version";
        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;
        [SerializeField]
        public LoadingForm LoadingForm = null;
        LoadingForm loadingFormInstance = null;

        [SerializeField]
        private UpdateResourceForm m_UpdateResourceFormTemplate = null;

        private VersionInfo mVersionInfo = null;
        public VersionInfo VersionInfo
        {
            get
            {
                return mVersionInfo;
            }
            set
            {
                mVersionInfo = value;
            }
        }

        public UpdateResourceForm UpdateResourceFormTemplate
        {
            get
            {
                return m_UpdateResourceFormTemplate;
            }
        }

        void Start()
        {
            loadingFormInstance = Object.Instantiate(LoadingForm);
        }

        public void SetLoadingProgress(float ratio, string text)
        {
            if (loadingFormInstance != null)
            {
                loadingFormInstance.SetProgress(ratio, text);
            }
        }

        public void ClearLoadingForm()
        {
            if (loadingFormInstance != null)
            {
                Object.Destroy(loadingFormInstance.gameObject);
                loadingFormInstance = null;
            }
        }

        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!GameEntry.Localization.ParseData(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }

        public void PrintVersionInfo()
        {
            if (mVersionInfo == null)
            {
                return;
            }
            Debug.Log(mVersionInfo.ForceUpdateGame);
            Debug.Log(mVersionInfo.LatestGameVersion);
            Debug.Log(mVersionInfo.InternalGameVersion);
            Debug.Log(mVersionInfo.UpdatePrefixUri);
            Debug.Log(mVersionInfo.VersionListLength);
            Debug.Log(mVersionInfo.VersionListHashCode);
            Debug.Log(mVersionInfo.VersionListCompressedLength);
            Debug.Log(mVersionInfo.VersionListCompressedHashCode);
        }
    }
}
