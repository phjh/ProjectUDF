using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerAttack))]
public class PlayerAttackRange : Editor
{
    void OnSceneGUI()
    {
        PlayerAttack fow = (PlayerAttack)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector2.up, 360, fow.viewRadius);
        Vector3 viewAngleA = Quaternion.Euler(0, 90, 90) * fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = Quaternion.Euler(0, 90, 90) *  fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.blue;
        foreach (Transform visible in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visible.transform.position);
        }
    }


}
