using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Playe : MonoBehaviour
{
    [Header("Player quest info")]
    public Objective objective;

    [Header("Quest Chain")]
    public List<Objective> questList;
    public int currentQuestIndex = 0;

    private string playerID;

    [Header("Other stuff")]
    public DataManager dataManagerRef;

    void Start()
    {
       if (dataManagerRef.IsPlayerLoggedIn())
        {
            if (dataManagerRef.GetLoggedInPlayer() != null)
            {
                playerID = dataManagerRef.GetLoggedInPlayer().Email;
            }
        }
    }

    public void CompleteQuest()
    {
        objective.CompleteObjective();
        currentQuestIndex++; 
        FindAnyObjectByType<QuestGiver>().GiveNextQuest();
    }

}
