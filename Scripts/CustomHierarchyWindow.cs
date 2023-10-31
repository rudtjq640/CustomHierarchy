using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace GY
{
    public class CustomHierarchyWindow : EditorWindow
    {
        void OnEnable()
        {
            // Hierarchy 변경 이벤트에 핸들러 연결
            EditorApplication.hierarchyChanged += RepaintHierarchyWindow;
        }

        void OnDisable()
        {
            // Hierarchy 변경 이벤트에서 핸들러 제거
            EditorApplication.hierarchyChanged -= RepaintHierarchyWindow;

            iconFiles = null;
        }
        
        private static List<string> iconFiles;

        // 계층 구조 윈도우를 다시 그리는 메서드
        private void RepaintHierarchyWindow()
        {
            EditorApplication.RepaintHierarchyWindow();
        }

        [MenuItem("Window/Custom Hierarchy", false, 10)]
        public static void OpenWindow()
        {
            iconFiles = new List<string>(Directory.GetFiles("Assets/HierarchyEditor/MyIcons", "*.png"));
            EditorWindow.GetWindow(typeof(CustomHierarchyWindow));
        }

        void OnGUI()
        {
            foreach (var iconFile in iconFiles)
            {
                if (GUILayout.Button(Path.GetFileNameWithoutExtension(iconFile)))
                {
                    foreach(var selectedObject in Selection.gameObjects)
                    {
                        // 새 아이콘을 적용
                        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconFile);
                        HierarchyIconManager.ApplyIconByInstanceId(selectedObject.GetInstanceID(), icon);
            
                        // 새 아이콘 정보를 PlayerPrefs에 저장
                        PlayerPrefs.SetString(selectedObject.GetInstanceID().ToString(), iconFile);
                    }
                }
            }
            // 모든 정보를 PlayerPrefs에 즉시 저장
            PlayerPrefs.Save();
        }
    }
}