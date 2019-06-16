using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoM : MonoBehaviour
{
    public Text CenterOfMass;
    public Dropdown MeshOptions;

    public CenterOfMass CenterOfMassSys;

    void Start()
    {
        List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();
        for (int i = 0; i < CenterOfMassSys.Prefabs.Count; i++)
        {
            dropdownOptions.Add(new Dropdown.OptionData(CenterOfMassSys.Prefabs[i].Name));
        }

        MeshOptions.ClearOptions();
        MeshOptions.AddOptions(dropdownOptions);

        MeshOptions.onValueChanged.AddListener(delegate
        {
            CenterOfMassSys.ChangePrefab(MeshOptions.value);
        });
    }

    public void Refresh()
    {
        CenterOfMass.text = CenterOfMassSys.CalculatedCenterOfMass.ToString();
    }
}
