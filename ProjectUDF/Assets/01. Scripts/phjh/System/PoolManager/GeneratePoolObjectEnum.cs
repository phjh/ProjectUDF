using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GameManager))]
public class AutoEnumBuilder : Editor
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
        foreach(var lists in GameManager.Instance.poollistSO)
        {
            List<PoolingPair> list = lists.PoolingLists;
            sb.Append("public enum " + lists.name + "Enum\n{\n");
            foreach(PoolingPair pair in list)
            {
                sb.Append($"\t{pair.name} = {pair.ID},\n");
            }
            sb.Append("}\n\n");
            string enumLocation = Application.dataPath + "\\01. Scripts\\phjh\\System\\PoolManager\\PoolObjectEnum.cs";
            File.WriteAllText(enumLocation, sb.ToString());
        }
    }

}