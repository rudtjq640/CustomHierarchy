using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IconData", menuName = "ScriptableObjects/IconData", order = 1)]
public class IconData : ScriptableObject
{
    public Dictionary<int, string> iconInfo = new Dictionary<int, string>();
}