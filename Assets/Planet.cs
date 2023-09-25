using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField,HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] faces;
    [SerializeField,Range(2,256)] int resolution = 2;

    private void OnValidate()
    {
        Initialise();
        GenerateMesh();
    }

    void Initialise()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        faces = new TerrainFace[6];

        Vector3[] directions = new Vector3[6]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.right
        };


        for (int i = 0; i < 6; i++) 
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            faces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in faces) 
        {
            face.ConstuctMesh();
        }
    }
}
