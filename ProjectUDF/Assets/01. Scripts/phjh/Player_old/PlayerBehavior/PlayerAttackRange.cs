using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    [Range(0,360)]
    [SerializeField] float fov;
    [SerializeField] int rayCount = 2;
    [SerializeField] float attackDistance = 5;

    Vector3 origin = Vector3.zero;

    private void OnEnable()
    {
        float angle = -fov/2;
        
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        //vertices[0] = GetVectorFromAngle(angle + fov/rayCount/2) * attackDistance / 3;

        for(int i = 0; i <= rayCount; i++)
        {
            origin = GetVectorFromAngle(angle + fov / rayCount / 2) * (attackDistance / 3);
            Vector3 vertex = origin + GetVectorFromAngle(angle) * attackDistance;
            vertices[i] = vertex;

            if (i > 0)
            {
                triangles[(i - 1) * 3 + 0] = 0;
                triangles[(i - 1) * 3 + 1] = i;
                triangles[(i - 1) * 3 + 2] = i+1;
            }

            angle += fov / rayCount;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }



    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }


}
