using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityGameFramework.Editor.ResourceTools;
using System.IO;

namespace DawnEditor
{
    public class DawnBuildEventHandler : IBuildEventHandler
    {
        class VersionListInfo
        {
            public int VersionListLength;
            public int VersionListHashCode;
            public int VersionListCompressedLength;
            public int VersionListCompressedHashCode;
            public VersionListInfo(int versionListLength, int versionListHashCode, int versionListCompressedLength, int versionListCompressedHashCode)
            {
                VersionListLength = versionListLength;
                VersionListHashCode = versionListHashCode;
                VersionListCompressedLength = versionListCompressedLength;
                VersionListCompressedHashCode = versionListCompressedHashCode;
            }
        }
        private Dictionary<string, VersionListInfo> m_VersionListInfo = null;

        public bool ContinueOnFailure
        {
            get;
        }
        public void OnPreprocessAllPlatforms(string productName, string companyName, string gameIdentifier, string gameFrameworkVersion, string unityVersion, string applicableGameVersion, int internalResourceVersion,
            Platform platforms, AssetBundleCompressionType assetBundleCompression, string compressionHelperTypeName, bool additionalCompressionSelected, bool forceRebuildAssetBundleSelected, string buildEventHandlerTypeName, string outputDirectory, BuildAssetBundleOptions buildAssetBundleOptions,
            string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, string buildReportPath)
        {

        }

        public void OnPreprocessPlatform(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath)
        {

        }
        public void OnBuildAssetBundlesComplete(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, AssetBundleManifest assetBundleManifest)
        {

        }
        public void OnOutputUpdatableVersionListData(Platform platform, string versionListPath, int versionListLength, int versionListHashCode, int versionListCompressedLength, int versionListCompressedHashCode)
        {
            if (m_VersionListInfo == null)
            {
                m_VersionListInfo = new Dictionary<string, VersionListInfo>();
            }
            m_VersionListInfo.Add(GetPlatformString(platform), new VersionListInfo(versionListLength, versionListHashCode, versionListCompressedLength, versionListCompressedHashCode));
        }
        public void OnPostprocessPlatform(Platform platform, string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, bool isSuccess)
        {
            // copy resources
            if (outputPackageSelected)
            {
                if (Directory.Exists(Application.streamingAssetsPath))
                {
                    Directory.Delete(Application.streamingAssetsPath, true);
                }
                FileUtil.CopyFileOrDirectory(outputPackagePath, Application.streamingAssetsPath);
                Debug.Log(string.Format("copy resource from {0} to {1}  Success", outputPackagePath, Application.streamingAssetsPath));
            }
        }


        /// 生成版本信息文件
        public void OnPostprocessAllPlatforms(string productName, string companyName, string gameIdentifier, string gameFrameworkVersion, string unityVersion, string applicableGameVersion, int internalResourceVersion,
            Platform platforms, AssetBundleCompressionType assetBundleCompression, string compressionHelperTypeName, bool additionalCompressionSelected, bool forceRebuildAssetBundleSelected, string buildEventHandlerTypeName, string outputDirectory, BuildAssetBundleOptions buildAssetBundleOptions,
            string workingPath, bool outputPackageSelected, string outputPackagePath, bool outputFullSelected, string outputFullPath, bool outputPackedSelected, string outputPackedPath, string buildReportPath)
        {
            if (m_VersionListInfo != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{\n");
                foreach (KeyValuePair<string, VersionListInfo> kv in m_VersionListInfo)
                {
                    string jsonstr = JsonUtility.ToJson(kv.Value, true);
                    sb.AppendFormat("   \"{0}\":{1}", kv.Key, jsonstr);
                    sb.Append(",\n");
                }
                sb.AppendLine(" \"END\":\"\"");
                sb.AppendLine("}");
                var outPath = System.IO.Path.Combine(outputFullPath, "VersionInfo.json");
                if (!System.IO.File.Exists(outPath))
                {
                    System.IO.File.Create(outPath).Dispose();
                }
                System.IO.File.WriteAllText(outPath, sb.ToString());
            }
        }
        public string GetPlatformString(Platform platform)
        {
            switch (platform)
            {
                case Platform.Undefined:
                    return "";
                case Platform.Windows:
                    return "Windows";
                case Platform.Windows64:
                    return "Windows64";
                case Platform.WindowsStore:
                    return "WindowsStore";
                case Platform.MacOS:
                    return "MacOS";
                case Platform.Linux:
                    return "Linux";
                case Platform.IOS:
                    return "IOS";
                case Platform.Android:
                    return "Android";
                case Platform.WebGL:
                    return "WebGL";
                default:
                    return "";
            }
        }
    }
}

