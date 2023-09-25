using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField,HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] faces;
    [SerializeField,Range(2,256)] int resolution = 2;
    public bool autoUpdate = true;
    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    ShapeGenerator shapeGenerator;

    void Initialise()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        faces = new TerrainFace[6];

        Vector3[] directions = new Vector3[6]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
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

            faces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        GenerateColours();
    }

    public void OnColourSettingsUpdated()
    {
        if (!autoUpdate) { return; }
        Initialise();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) { return; }
        Initialise();
        GenerateMesh();
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in faces) 
        {
            face.ConstuctMesh();
        }
    }

    void GenerateColours()
    {
        foreach (MeshFilter filter in meshFilters)
        {
            filter.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.colour;
        }
    }
}
