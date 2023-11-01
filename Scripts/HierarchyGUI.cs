using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace GY
{
    public static class IconPhrases
    {   
        public static Dictionary<string, (string, Color)> Phrases => new Dictionary<string, (string, Color)>
        {
            {"[ Light ]", ("Light Icon", new Color(1f, 0.5f, 0f, 0.5f))},
            {"[ Volume ]", ("LightProbeProxyVolume Gizmo", new Color(0.2f, 1f, 0.18f, 0.5f))},
            {"[ Timeline ]", ("TimelineAsset Icon", new Color(0f, 1f, .5f, 0.3f))},
            {"[ Camera ]", ("Camera Gizmo", new Color(0f, 0.12f, 1f, 0.5f))},
            {"[ Magaica ]", ("Assets/HierarchyEditor/MyIcons/10. Magaica_icon_cloth.png", new Color(1f, 0f, 0f, 0.5f))},
            {"[ Recorder ]", ("Animation.Record", new Color(1f, 0.75f, 0.45f, 0.5f))},
            {"[ Avatar ]", ("Avatar Icon", new Color(0.5f, 0f, 1f, 0.5f))},
            {"[ Scripts ]", ("Script Icon", new Color(0.81f, 0.91f, 0.8f, 0.5f))},
            {"[ Mesh ]", ("Mesh Icon", new Color(0.5f, 0.5f, 1f, 0.5f))},
            {"[ Button ]", ("Button Icon", new Color(0.5f, 0.5f, 1f, 0.5f))},
            {"[ Image ]", ("Sprite Icon", new Color(0.5f, 0.5f, 1f, 0.5f))},


            // 첫번째 문자열은 지정하고 싶은 문구를
            // 두번째 문자열은 지정하고 싶은 아이콘의 파일명 혹은 경로를
            // 세번째 문자열은 컬러를 지정하여 원하는대로 값을 수정합니다.
            // 네번째 숫자의 값은 투명도입니다.

            // 해당 사이트를 통해 0~1로 변환된 RGB 값을 구할 수 있습니다. < https://rgbcolorpicker.com/0-1 >
        };
    }   
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [InitializeOnLoad]
    public class HierarchyGUI
    {
        private const int _iconSize = 18;  // 아이콘의 크기를 지정하는 상수
        private const int _backgroundStartOffset = 17;  // 배경색상의 시작 위치를 조절하는 상수
        private const int _backgroundEndOffset = 50;  // 배경색상의 끝 위치를 조절하는 상수
        
        static HierarchyGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

            if (gameObject == null)
            return;

            string iconFile = PlayerPrefs.GetString(instanceId.ToString(), null);

            if (!string.IsNullOrEmpty(iconFile))
            {
                Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconFile);
                Rect iconRect = new Rect(selectionRect.x + selectionRect.width - _iconSize, selectionRect.y, _iconSize, _iconSize);
                // GUI.DrawTexture(iconRect, icon);
            }

            foreach (var phrase in IconPhrases.Phrases)
            {
                if (gameObject.name.Contains(phrase.Key))
                {
                    // 배경색상의 끝 위치를 조절하는 라인 추가
                    var adjustedRect = new Rect(selectionRect.x + _backgroundStartOffset, selectionRect.y, selectionRect.width - _backgroundStartOffset - _backgroundEndOffset, selectionRect.height);
                    Texture2D tex = MakeTex((int)adjustedRect.width, (int)adjustedRect.height, phrase.Value.Item2);
                    DrawTexture(adjustedRect, tex);

                    Rect iconRect = new Rect(selectionRect.x + selectionRect.width - _iconSize, selectionRect.y, _iconSize, _iconSize);
                    DrawIcon(iconRect, instanceId, phrase.Value.Item1);
                }
            }
        }

        // 주어진 아이콘으로 아이콘을 그리는 메서드
        private static void DrawIcon(Rect rect, int instanceId, string iconName)
        {
            Texture2D icon;
        
            if (iconName.StartsWith("Assets/"))
            {
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconName);
            }
                else
                {
                    GUIContent iconContent = EditorGUIUtility.IconContent(iconName);
                    icon = iconContent.image as Texture2D;
                }
            
            // 아이콘이 null이 아니면 아이콘을 그립니다.
            if(icon != null)
            {
                GUI.DrawTexture(rect, icon);
            }
            // 아이콘이 null인 경우, 적절한 처리를 수행합니다. 
            // else
            // {
            //     Debug.Log("Icon could not be loaded: " + iconName);
            // }
        }

        // 색상으로 텍스처를 생성하는 메서드
        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    float alpha = 1f - (float)x / width;
                    pix[y * width + x] = new Color(col.r, col.g, col.b, col.a * alpha);
                }
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        private static void DrawTexture(Rect position, Texture2D texture)
        {
            GUI.DrawTexture(position, texture);
        }
    }
}