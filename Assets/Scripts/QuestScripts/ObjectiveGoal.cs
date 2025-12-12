using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveGoal
{
    
    public DataManager dataManagerRef;
    public ObjectiveTypes objectiveType;
    public int requiredAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return currentAmount >= requiredAmount;
    }

    public void IncrementProgress()
    {
        currentAmount++;
    }

    public void ScanningDog()
    {
        if (objectiveType == ObjectiveTypes.scanDog)
        {
            IncrementProgress();
        }
    }

    public void AdoptingDog()
    {
        if (objectiveType == ObjectiveTypes.adoptDog)
        {
            IncrementProgress();
        }
    }
}

public enum ObjectiveTypes
{
    scanDog,
    adoptDog
}