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
}

public enum ObjectiveTypes
{
    scanDog,
    adoptDog
}