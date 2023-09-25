using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    int resolution;
    Vector3 localUp; //up dir
    Vector3 axisA, axisB; //right and forward directions

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        axisA = new Vector3(localUp.y,localUp.z,localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstuctMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        //formula for triangle count is (r-1)^2 * 6 where r is resolution
        int[] triangles = new int[(resolution-1)*(resolution-1) * 6];

        int triIndex = 0;

        //generate vertices and triangles

        for (int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                int index = x + y * resolution;


                //vertex
                Vector2 percent = new Vector2(x, y) / (resolution - 1); //percent completed for either loop
                Vector3 pointOnSurface = localUp + (percent.x-.5f) * 2 * axisA 
                                                 + (percent.y-.5f) * 2 * axisB;
                
                Vector3 pointOnUnitSphere = pointOnSurface.normalized;
                vertices[index] = pointOnUnitSphere;
                //
                //triangles

                if(x != resolution - 1 && y != resolution - 1) //not on edge of mesh
                {
                    //triangle one
                    triangles[triIndex] = index;
                    triangles[triIndex + 1] = index + resolution + 1;
                    triangles[triIndex + 2] = index + resolution;

                    //triangle two
                    triangles[triIndex + 3] = index;
                    triangles[triIndex + 4] = index + 1;
                    triangles[triIndex + 5] = index + resolution + 1;

                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
