using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace GY
{
    [InitializeOnLoad]
    public static class HierarchyIconManager
    {
        // 필드와 메서드의 정보를 저장하는 필드
        private static readonly FieldInfo _sceneHierarchyField;
        private static readonly FieldInfo _treeViewField;
        private static readonly MethodInfo _treeViewItemsMethod;
        private static readonly PropertyInfo _treeViewDataProperty;

        // 윈도우 업데이트에 대한 타이밍을 관리하는 필드
        private static double _nextWindowsUpdate;

        // 캐시된 윈도우 배열
        private static EditorWindow[] _windowCache;

        static HierarchyIconManager()
        {
            // 필드와 메서드의 정보를 Reflection을 통해 가져옴
            var assembly = Assembly.GetAssembly(typeof(EditorWindow));
            _sceneHierarchyField = assembly.GetType("UnityEditor.SceneHierarchyWindow").GetField("m_SceneHierarchy", BindingFlags.Instance | BindingFlags.NonPublic);
            _treeViewField = assembly.GetType("UnityEditor.SceneHierarchy").GetField("m_TreeView", BindingFlags.Instance | BindingFlags.NonPublic);
            _treeViewDataProperty = assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController").GetProperty("data", BindingFlags.Instance | BindingFlags.Public);
            _treeViewItemsMethod = assembly.GetType("UnityEditor.GameObjectTreeViewDataSource").GetMethod("GetRows", BindingFlags.Instance | BindingFlags.Public);
        }

        // 모든 하이어라키 윈도우를 반환하는 메서드
        private static IEnumerable<EditorWindow> GetAllHierarchyWindows(bool forceUpdate = false)
        {
            // 윈도우 업데이트가 필요한지 확인하고, 필요하면 업데이트
            if (forceUpdate || _nextWindowsUpdate < EditorApplication.timeSinceStartup)
            {
                _nextWindowsUpdate = EditorApplication.timeSinceStartup + 2.0;
                _windowCache = GetWindowsByType("UnityEditor.SceneHierarchyWindow").ToArray();
            }
            // 캐시된 윈도우 배열을 반환
            return _windowCache;
        }

        // 주어진 타입의 모든 윈도우를 반환하는 메서드
        private static IEnumerable<EditorWindow> GetWindowsByType(string typeName) 
        {
            return Resources.FindObjectsOfTypeAll<EditorWindow>().Where(window => window.GetType().ToString() == typeName);
        }

        // 주어진 윈도우의 모든 트리뷰 아이템을 반환하는 메서드
        private static IEnumerable<TreeViewItem> GetTreeViewItems(EditorWindow window)
        {
            // Reflection을 통해 필요한 객체를 가져옴
            var sceneHierarchy = _sceneHierarchyField.GetValue(window);
            var treeView = _treeViewField.GetValue(sceneHierarchy);
            var treeViewData = _treeViewDataProperty.GetValue(treeView, null);

            // 트리뷰 아이템을 반환
            return (IEnumerable<TreeViewItem>)_treeViewItemsMethod.Invoke(treeViewData, null);
        }

        // instanceId에 해당하는 아이템에 아이콘을 적용하는 메서드
        public static void ApplyIconByInstanceId(int instanceId, Texture2D icon)
        {
            // 모든 하이어라키 윈도우에 대해
            foreach (var hierarchyWindow in GetAllHierarchyWindows())
            {
                // instanceId에 해당하는 트리뷰 아이템을 찾아
                var treeViewItem = GetTreeViewItems(hierarchyWindow).FirstOrDefault(item => item.id == instanceId);
                
                // 아이템이 null이 아니면 아이콘을 적용
                if (treeViewItem != null)
                {
                    treeViewItem.icon = icon;
                }
            }
        }
    }
}
