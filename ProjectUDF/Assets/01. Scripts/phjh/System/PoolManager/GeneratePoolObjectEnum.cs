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
        List<PoolingPair> list = GameManager.Instance.poollistSO.PoolingLists;
        sb.Append("public enum PoolObjectListEnum \n{\n");
        foreach(PoolingPair pair in list)
        {
            sb.Append($"\t{pair.name} = {pair.ID},\n");
        }
        sb.Append("}\n\n");

        List<EffectPoolingPair> list2 = GameManager.Instance.poollistSO.EffectLists;
        sb.Append("public enum PoolEffectListEnum \n{\n");
        foreach (EffectPoolingPair pair in list2)
        {
            sb.Append($"\t{pair.name} = {pair.ID},\n");
        }
        sb.Append("}\n\n");

        string enumLocation = Application.dataPath + "\\01. Scripts\\phjh\\System\\PoolManager\\PoolObjectEnum.cs";
        File.WriteAllText(enumLocation, sb.ToString());

    }

}