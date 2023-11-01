using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class IconChangerWindow : EditorWindow
{
    private Texture2D[] _icons;
    private GameObject _currentSelectedGameObject;

    [MenuItem("Window/Icon Changer")]
    public static void ShowWindow()
    {
        GetWindow<IconChangerWindow>("Icon Changer");
    }

    private void OnEnable()
    {
        LoadIcons();
    }

    private void OnGUI()
{
    _currentSelectedGameObject = Selection.activeGameObject;

    if (_currentSelectedGameObject == null)
    {
        EditorGUILayout.LabelField("No GameObject selected in Hierarchy");
        return;
    }
    
    EditorGUILayout.LabelField("Selected GameObject: " + _currentSelectedGameObject.name);
    
    for (int i = 0; i < _icons.Length; i++)
    {
        if (_icons[i] == null)
        {
            Debug.LogError("Icon at index " + i + " is null");
            continue;
        }

        if (GUILayout.Button(_icons[i]))
        {
            SetIcon(_currentSelectedGameObject, _icons[i]);
        }
    }
}

    private void LoadIcons()
    {
        string path = "Assets/HierarchyEditor/MyIcons";
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        _icons = new Texture2D[fileInfo.Length];

        for (int i = 0; i < fileInfo.Length; i++)
        {
            string fullPath = fileInfo[i].FullName;
            string assetPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            _icons[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        }
    }

    private static void SetIcon(GameObject gObj, Texture2D iconTex)
{
    if (gObj == null)
    {
        Debug.LogError("GameObject is null");
        return;
    }
    if (iconTex == null)
    {
        Debug.LogError("Icon Texture is null");
        return;
    }

    var egu = typeof(EditorGUIUtility);
    var mi = egu.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
    mi.Invoke(null, new object[] { gObj, iconTex });
}
}
