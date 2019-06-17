using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public UICoM UI;
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

        //Looping through submeshes to make sure the center of mass is correct even if there are submeshes.
        for (int submeshIndex = 0; submeshIndex < selectedPrefab.mesh.subMeshCount; submeshIndex++)
        {
            //Getting triangles for the current submesh
            triangleIndices = selectedPrefab.mesh.GetTriangles(submeshIndex);

            //Looping through the triangle vertex indices in steps of three (per triangle)
            for (int i = 0; i < triangleIndices.Length; i+=3)
            {
                trianglePointA = triangleIndices[i];
                trianglePointB = triangleIndices[i + 1];
                trianglePointC = triangleIndices[i + 2];

                //Calculating the faces of the tetrahedron
                float CBA = vertices[trianglePointC].x * vertices[trianglePointB].y * vertices[trianglePointA].z;
                float BCA = vertices[trianglePointB].x * vertices[trianglePointC].y * vertices[trianglePointA].z;
                float CAB = vertices[trianglePointC].x * vertices[trianglePointA].y * vertices[trianglePointB].z;
                float ACB = vertices[trianglePointA].x * vertices[trianglePointC].y * vertices[trianglePointB].z;
                float BAC = vertices[trianglePointB].x * vertices[trianglePointA].y * vertices[trianglePointC].z;
                float ABC = vertices[trianglePointA].x * vertices[trianglePointB].y * vertices[trianglePointC].z;


                //Combining all of our faces by following the right hand rule and multiplying the result by 1 over 6
                //https://en.wikipedia.org/wiki/Tetrahedron
                float tetrahedronVolume = (-CBA + BCA + CAB - ACB - BAC +  ABC) * 1f/ 6f;

                //Adding the tetrahedron volume to our total volume
                totalVolume += tetrahedronVolume;

                //Adding the average per coordinate times the tetrahedrons volume to our center of mass to ultimately give us the sum of all centers of mass. 
                //Dividing by four because I am assuming the fourth point of tetrahedron is at (0,0,0).
                centerOfMass.x += ((vertices[trianglePointA].x + vertices[trianglePointB].x + vertices[trianglePointC].x) / 4f) * tetrahedronVolume;
                centerOfMass.y += ((vertices[trianglePointA].y + vertices[trianglePointB].y + vertices[trianglePointC].y) / 4f) * tetrahedronVolume;
                centerOfMass.z += ((vertices[trianglePointA].z + vertices[trianglePointB].z + vertices[trianglePointC].z) / 4f) * tetrahedronVolume;
            }
        }

        //Dividing the center of mass by the total volume to give us the average center of mass of all the tetrahedrons, which is the meshes center of mass.
        centerOfMass /= totalVolume;

        //using transform.Transformpoint to make sure that we are in world space and not local space. 
        calculatedCenterOfMass = selectedPrefab.Prefab.transform.TransformPoint(centerOfMass);
        marker.transform.position = calculatedCenterOfMass;
        UI.Refresh();
    }
}

[System.Serializable]
public struct MeshOption
{
    public string Name;
    public Mesh mesh;
    public GameObject Prefab;

}
