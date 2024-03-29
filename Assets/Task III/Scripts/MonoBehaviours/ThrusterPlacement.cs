﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterPlacement : MonoBehaviour
{
    private bool placeThruster;

    public GameObject ThrusterMarker;
    public Bootstrap Bootstrapper;
    public bool PlaceThruster { get { return placeThruster; } set { placeThruster = value; } }
    private List<GameObject> placedThrusters = new List<GameObject>();

    RaycastHit hitPoint;

    private void Update()
    {
        if(placeThruster)
        {
            if(placedThrusters.Count != 0)
            {
                foreach(var thrust in placedThrusters)
                {
                    DestroyImmediate(thrust);
                }
                placedThrusters.Clear();
            }

            if(!ThrusterMarker.activeSelf)
            {
                ThrusterMarker.SetActive(true);
            }
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint,float.MaxValue))
            {
                if(hitPoint.collider.gameObject.tag == "Cube")
                {
                    ThrusterMarker.transform.position = hitPoint.point;
                }
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject thrusterGO = Instantiate(ThrusterMarker, ThrusterMarker.transform.position, Quaternion.identity);
                placedThrusters.Add(thrusterGO);
                thrusterGO.transform.SetParent(Bootstrapper.SpaceshipPrefab.transform);
                Bootstrapper.ThrusterPosition = thrusterGO.transform.position;
                Bootstrapper.Trusterplaced = true;
                placeThruster = false;
            }
        }
        else
        {
            if(ThrusterMarker.activeSelf)
            {
                ThrusterMarker.SetActive(false);
            }
        }
    }
}
