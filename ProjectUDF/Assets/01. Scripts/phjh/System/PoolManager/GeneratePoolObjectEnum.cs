using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(GameManager))]
public class GeneratePoolObjectEnum : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate PoolObject Enum"))
        {
            GenerateEnum();
        }

    }

    void GenerateEnum()
    {
        StringBuilder sb = new StringBuilder();
        List<PoolingPair> list1 = GameManager.Instance.poollistSO.PoolObjectLists;
        sb.Append("public enum PoolObjectListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list1)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        List<EffectPoolingPair> list2 = GameManager.Instance.poollistSO.PoolEffectLists;
        sb.Append("public enum PoolEffectListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list2)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        List<UIPoolingPair> list3 = GameManager.Instance.poollistSO.PoolUILists;
        sb.Append("public enum PoolUIListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list3)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        string enumLocation = Application.dataPath + "\\01. Scripts\\phjh\\System\\PoolManager\\PoolObjectEnum.cs";
        Equals(enumLocation, sb);
        File.WriteAllText(enumLocation, sb.ToString());

    }

}

#endif