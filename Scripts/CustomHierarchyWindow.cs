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
            // Hierarchy ���� �̺�Ʈ�� �ڵ鷯 ����
            EditorApplication.hierarchyChanged += RepaintHierarchyWindow;
        }

        void OnDisable()
        {
            // Hierarchy ���� �̺�Ʈ���� �ڵ鷯 ����
            EditorApplication.hierarchyChanged -= RepaintHierarchyWindow;

            iconFiles = null;
        }
        
        private static List<string> iconFiles;

        // ���� ���� �����츦 �ٽ� �׸��� �޼���
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
                        // �� �������� ����
                        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconFile);
                        HierarchyIconManager.ApplyIconByInstanceId(selectedObject.GetInstanceID(), icon);
            
                        // �� ������ ������ PlayerPrefs�� ����
                        PlayerPrefs.SetString(selectedObject.GetInstanceID().ToString(), iconFile);
                    }
                }
            }
            // ��� ������ PlayerPrefs�� ��� ����
            PlayerPrefs.Save();
        }
    }
}