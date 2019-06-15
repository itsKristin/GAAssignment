using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class SpaceshipMotionUI : MonoBehaviour
{
    public GameObject DeactivationPanel;
    public GameObject SimulationPanel;

    public InputField Length;
    public InputField Width;
    public InputField Height;

    public InputField ThrustVectorX;
    public InputField ThrustVectorY;
    public InputField ThrustVectorZ;

    public InputField Mass;

    public Button RunSimulation;
    public Button PlaceThruster;
    public Button Reset;

    public Toggle Gravity;

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

        ThrustVectorX.contentType = InputField.ContentType.DecimalNumber;
        ThrustVectorX.onValueChanged.AddListener(delegate
        {
            float potentialValue = (ThrustVectorX.text == null || ThrustVectorX.text == string.Empty) ? 0f : float.Parse(ThrustVectorX.text);
        
            Bootstrapper.ThrustVector = new float3
            {
                x = potentialValue,
                y = Bootstrapper.ThrustVector.y,
                z = Bootstrapper.ThrustVector.z
            };
        });

        ThrustVectorY.contentType = InputField.ContentType.DecimalNumber;
        ThrustVectorY.onValueChanged.AddListener(delegate
        {
            float potentialValue = (ThrustVectorY.text == null || ThrustVectorY.text == string.Empty) ? 0f : float.Parse(ThrustVectorY.text);
        
            Bootstrapper.ThrustVector = new float3
            {
                x = Bootstrapper.ThrustVector.x,
                y = potentialValue,
                z = Bootstrapper.ThrustVector.z
            };
        });

        ThrustVectorZ.contentType = InputField.ContentType.DecimalNumber;
        ThrustVectorZ.onValueChanged.AddListener(delegate
        {
            float potentialValue = (ThrustVectorZ.text == null || ThrustVectorZ.text == string.Empty) ? 0f : float.Parse(ThrustVectorZ.text);
        
            Bootstrapper.ThrustVector = new float3
            {
                x = Bootstrapper.ThrustVector.x,
                y = Bootstrapper.ThrustVector.y,
                z = potentialValue
            };
        });

        Mass.contentType = InputField.ContentType.DecimalNumber;
        Mass.onValueChanged.AddListener(delegate
        {
            Bootstrapper.SpaceshipMass = (Mass.text == null || Mass.text == string.Empty) ? 0f : float.Parse(Mass.text);
        });

        RunSimulation.onClick.AddListener(() => 
        {
            Bootstrapper.RunSimulation();
            DeactivationPanel.SetActive(false);
            SimulationPanel.SetActive(true);
        });

        Reset.onClick.AddListener(() =>
        {
            Bootstrapper.Reset();
            DeactivationPanel.SetActive(true);
            SimulationPanel.SetActive(false);
        });


        PlaceThruster.onClick.AddListener(() => 
        {
            Thrusterplacement.PlaceThruster = true;
        });

        Gravity.onValueChanged.AddListener(delegate
        {
            Bootstrapper.Gravity = Gravity.isOn;
        });

    }

    
    


}
