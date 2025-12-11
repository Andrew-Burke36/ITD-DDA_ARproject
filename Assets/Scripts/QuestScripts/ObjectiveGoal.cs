using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestGoal
{
    public ObjectiveType objectiveType;

    public int requiredAmount;
    public int currentAmount;

    public bool IsCompleted()
    {
        return currentAmount >= requiredAmount;
    }
}

