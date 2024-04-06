using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Presets;
using System.IO;
using System.Text;
using System.Reflection;
using UnityGameFramework.Runtime;
using System;
namespace Dawn
{

    [InitializeOnLoad]
    public class EditorTools
    {

        #region  Editor事件监听
        static EditorTools()
        {
            EditorSceneManager.sceneOpened += EditorSceneManager_Opened;
            EditorSceneManager.sceneSaved += EditorSceneManager_sceneSaved; //场景保存事件

            //添加UI Label 
            EditorApplication.hierarchyWindowItemOnGUI += AddLabelOnHierarchyItem;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }
        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    // ClearConsoleLog();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }
        private static void EditorSceneManager_Opened(Scene scene, OpenSceneMode mode)
        {
        }
        private static void EditorSceneManager_sceneSaved(Scene scene)
        {
            GameObject[] rootGameobjects = scene.GetRootGameObjects();
            foreach (GameObject gameObject in rootGameobjects)
            {
                if (gameObject.GetComponent<UIFormLogic>() != null)
                {
                    try
                    {
                        var savePath = "Assets/BundleResources/UI/" + gameObject.name + ".prefab";
                        PrefabUtility.SaveAsPrefabAsset(gameObject, savePath);
                        Debug.Log("Save UI Prefab = " + savePath);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
        }
        private static void AddLabelOnHierarchyItem(int instanceid, Rect selectionrect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                if (obj.GetComponent<UIFormLogic>() != null)
                {
                    Rect r = new Rect(selectionrect);
                    r.x = r.width;
                    var style = new GUIStyle();
                    style.normal.textColor = Color.yellow;
                    style.hover.textColor = Color.cyan;
                    GUI.Label(r, "[UI]", style);
                }
                if (true)
                {
                    if (Selection.objects.Length == 1)
                    {
                        if (obj == Selection.objects[0])
                        {
                            Rect btnPos = new Rect(selectionrect);
                            btnPos.x = btnPos.x + (btnPos.width - 40);
                            btnPos.width = 40;
                            if (GUI.Button(btnPos, "path"))
                            {
                                string path = obj.name;
                                var temp = obj.transform.parent;
                                while (temp != null)
                                {
                                    if (temp.gameObject.GetComponent<UIFormLogic>() != null)
                                    {
                                        break;
                                    }
                                    path = temp.gameObject.name + "/" + path;
                                    temp = temp.transform.parent;
                                }
                                UnityEngine.GUIUtility.systemCopyBuffer = path;
                                Debug.Log("Path:" + path);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 日志清理函数
        /// </summary>
        private static MethodInfo ConsoleLogClearMethod;
        private static void ClearConsoleLog()
        {
            if (ConsoleLogClearMethod == null)
            {
                var assembly = Assembly.GetAssembly(typeof(ActiveEditorTracker));
                var type = assembly.GetType("UnityEditorInternal.LogEntries");
                if (type == null)
                {
                    type = assembly.GetType("UnityEditor.LogEntries");
                }
                ConsoleLogClearMethod = type.GetMethod("Clear");
            }
            ConsoleLogClearMethod.Invoke(new object(), null);
        }
        #endregion

        #region  UI Tools
        [MenuItem("Tools/UI/Auto Gen UI LuaFile", false, 3001)]
        public static void GenUILuaFile()
        {
            var scene = SceneManager.GetActiveScene();
            GameObject uiGo = null;
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<UIFormLogic>() != null)
                {
                    uiGo = go;
                    break;
                }
            }
            if (uiGo != null)
            {
                var dir = Path.Combine(System.Environment.CurrentDirectory, string.Format("Product/Lua/game/ui/{0}", uiGo.name.ToLower()));
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var path = Path.Combine(dir, string.Format("{0}.lua", uiGo.name.ToLower()));
                if (File.Exists(path))
                {
                    Debug.LogError(path + "has exists: Gen LuaFile Failed");
                    return;
                }
                try
                {
                    var UIName = uiGo.name;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("local {0}  = Class(\"{1}\",UIBase)\n", UIName, UIName);
                    sb.Append("\n");
                    sb.AppendFormat("function {0}:OnInit()\n", UIName);
                    sb.Append("\n");
                    sb.Append("end\n");
                    sb.AppendFormat("function {0}:OnOpen()\n", UIName);
                    sb.Append("\n");
                    sb.Append("end\n");
                    sb.AppendFormat("function {0}:OnClose()\n", UIName);
                    sb.Append("\n");
                    sb.Append("end\n");
                    sb.AppendFormat("function {0}:OnDestroy()\n", UIName);
                    sb.Append("\n");
                    sb.Append("end\n");
                    sb.Append("\n");
                    sb.AppendFormat("return {0}\n", UIName);
                    File.WriteAllText(path, sb.ToString());
                    sb.Clear();
                    Debug.Log("Save Lua Path = " + path);
                }
                catch (IOException e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
        #endregion

        #region shortcut 快捷键
        //ctrl shift i    打开运行入口 % = Ctrl  # = shift   & = alt
        [MenuItem("Tools/Short Cut/OpenLaunchScene %&i", false, 4001)]
        public static void OpenLaunchScene()
        {
            string launchScenePath = "Assets/launch.unity";
            Debug.Log("Open Scene  name =  " + launchScenePath);
            EditorSceneManager.OpenScene(launchScenePath);
        }
        #endregion
    }

    public class PresetImportPreFolder : AssetPostprocessor
    {
        void OnPreprocessAset()
        {
            if (assetImporter.importSettingsMissing)
            {
                var path = Path.GetDirectoryName(assetPath);
                while (!string.IsNullOrEmpty(path))
                {
                    var presetGuids = AssetDatabase.FindAssets("t:Preset", new[] { path });
                    foreach (var presetGUid in presetGuids)
                    {
                        string presetPath = AssetDatabase.GUIDToAssetPath(presetGUid);
                        if (Path.GetDirectoryName(presetPath) == path)
                        {
                            var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                            if (preset.ApplyTo(assetImporter))
                                return;
                        }
                    }
                    path = Path.GetDirectoryName(path);
                }
            }
        }
    }

}