using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class SpaceshipMotionUI : MonoBehaviour
{
    public GameObject SettingsUI;
    public GameObject SimulationUI;

    public InputField Length;
    public InputField Width;
    public InputField Height;

    public Button RunSimulation;
    public Button PlaceThruster;

    public Bootstrap Bootstrapper;
    public ThrusterPlacement Thrusterplacement;

    private void Awake()
    {
        Length.contentType = InputField.ContentType.DecimalNumber;
        Length.onValueChanged.AddListener(delegate
        {
            float potentialValue = (Length.text == null || Length.text == string.Empty) ? 0f : float.Parse(Length.text);
        
            Bootstrapper.SpaceshipDimensions = new float3
            {
                x = potentialValue,
                y = Bootstrapper.SpaceshipDimensions.y,
                z = Bootstrapper.SpaceshipDimensions.z
            };
            Bootstrapper.Refresh();
        });

        Width.contentType = InputField.ContentType.DecimalNumber;
        Width.onValueChanged.AddListener(delegate
        {
            float potentialValue = (Width.text == null || Width.text == string.Empty) ? 0f : float.Parse(Width.text);
            
            Bootstrapper.SpaceshipDimensions = new float3
            {
                x = Bootstrapper.SpaceshipDimensions.x,
                y = potentialValue,
                z = Bootstrapper.SpaceshipDimensions.z
            };
            Bootstrapper.Refresh();
        });

        Height.contentType = InputField.ContentType.DecimalNumber;
        Height.onValueChanged.AddListener(delegate
        {
            float potentialValue = (Height.text == null || Height.text == string.Empty) ? 0f : float.Parse(Height.text);
            
            Bootstrapper.SpaceshipDimensions = new float3
            {
                x = Bootstrapper.SpaceshipDimensions.x,
                y = Bootstrapper.SpaceshipDimensions.y,
                z = potentialValue
            };
            Bootstrapper.Refresh();
            
        });

        RunSimulation.onClick.AddListener(() => 
        {
            Bootstrapper.RunSimulation();
            SettingsUI.SetActive(false);
            //SimulationUI.SetActive(true);
        });


        PlaceThruster.onClick.AddListener(() => 
        {
            Thrusterplacement.PlaceThruster = true;
        });

    }

    
    


}
