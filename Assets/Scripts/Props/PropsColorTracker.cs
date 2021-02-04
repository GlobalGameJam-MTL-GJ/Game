using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PropsColorTracker
{
    public Dictionary<PropsColor, bool> PropsColorsToAvailability;

    public PropsColorTracker()
    {
        PropsColorsToAvailability = new Dictionary<PropsColor, bool>()
        {
            {PropsColor.Blue, true},
            {PropsColor.Green, true},
            {PropsColor.Orange, true},
            {PropsColor.Purple, true}
        };
    }

    public PropsColor GetRandomFreeColor()
    {
        List<PropsColor> availablePropsColors = new List<PropsColor>();
        foreach (var keyValuePair in PropsColorsToAvailability)
        {
            if (keyValuePair.Value)
            {
                availablePropsColors.Add(keyValuePair.Key);
            }
        }
        int rand = UnityEngine.Random.Range(0, availablePropsColors.Count);
        PropsColorsToAvailability[availablePropsColors[rand]] = false;
        return availablePropsColors[rand];
    }

    public void MakeColorAvailable(PropsColor color)
    {
        PropsColorsToAvailability[color] = true;
    }

    public bool IsThereAnyColorAvailable()
    {
        foreach (var keyValuePair in PropsColorsToAvailability)
        {
            if (keyValuePair.Value)
            {
                return true;
            }
        }

        return false;
    }
}
