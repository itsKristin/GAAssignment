using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public UICoM ui;
    public List<MeshOption> Prefabs = new List<MeshOption>();
    public GameObject marker;

    private Vector3[] vertices;
    private MeshOption selectedPrefab;
    private Vector3 calculatedCenterOfMass;
    private GameObject instantiatedPrefab;

    public MeshOption SelectedPrefab { get { return selectedPrefab; } set { selectedPrefab = value; } }
    public Vector3 CalculatedCenterOfMass { get { return calculatedCenterOfMass; } }


    public void Start()
    {
        ChangePrefab(0);
    }

    public void ChangePrefab(int _index)
    {
        if(Prefabs.Count < _index)
        {
            return;
        }

        if(instantiatedPrefab != null)
        {
            GameObject temp = instantiatedPrefab;
            Destroy(temp);
        }

        selectedPrefab = Prefabs[_index];
        instantiatedPrefab = Instantiate(selectedPrefab.Prefab);
        CalculateCenterOfMass();
    }

    public void CalculateCenterOfMass()
    {
        Vector3 centerOfMass = Vector3.zero;
        float totalVolume = 0f;

        int[] triangleIndices;
        int trianglePointA, trianglePointB, trianglePointC;

        vertices = selectedPrefab.mesh.vertices;

        for (int submeshIndex = 0; submeshIndex < selectedPrefab.mesh.subMeshCount; submeshIndex++)
        {
            triangleIndices = selectedPrefab.mesh.GetTriangles(submeshIndex);

            for (int i = 0; i < triangleIndices.Length; i+=3)
            {
                trianglePointA = triangleIndices[i];
                trianglePointB = triangleIndices[i + 1];
                trianglePointC = triangleIndices[i + 2];

                float CBA = vertices[trianglePointC].x * vertices[trianglePointB].y * vertices[trianglePointA].z;
                float BCA = vertices[trianglePointB].x * vertices[trianglePointC].y * vertices[trianglePointA].z;
                float CAB = vertices[trianglePointC].x * vertices[trianglePointA].y * vertices[trianglePointB].z;
                float ACB = vertices[trianglePointA].x * vertices[trianglePointC].y * vertices[trianglePointB].z;
                float BAC = vertices[trianglePointB].x * vertices[trianglePointA].y * vertices[trianglePointC].z;
                float ABC = vertices[trianglePointA].x * vertices[trianglePointB].y * vertices[trianglePointC].z;


                float triangleVolume = (-CBA + BCA + CAB - ACB - BAC +  ABC) * 1f/ 6f;
                totalVolume += triangleVolume;

                centerOfMass.x += ((vertices[trianglePointA].x + vertices[trianglePointB].x + vertices[trianglePointC].x) / 4f) * triangleVolume;
                centerOfMass.y += ((vertices[trianglePointA].y + vertices[trianglePointB].y + vertices[trianglePointC].y) / 4f) * triangleVolume;
                centerOfMass.z += ((vertices[trianglePointA].z + vertices[trianglePointB].z + vertices[trianglePointC].z) / 4f) * triangleVolume;
            }
        }

        centerOfMass /= totalVolume;
        calculatedCenterOfMass = selectedPrefab.Prefab.transform.TransformPoint(centerOfMass);
        marker.transform.position = calculatedCenterOfMass;
        ui.Refresh();
    }


}

[System.Serializable]
public struct MeshOption
{
    public string Name;
    public Mesh mesh;
    public GameObject Prefab;

}
