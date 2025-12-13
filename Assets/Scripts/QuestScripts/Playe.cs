// This code was made by andrew to handle the player's quest information and current objective.
using System.Collections.Generic;
using Firebase.Database;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Playe : MonoBehaviour
{
    [Header("Player quest info")]
    public Objective objective;

    public ObjectiveGiver objectiveGiverRef;
    public int currentQuestIndex;

    private string playerID;

    [Header("Other stuff")]
    public DataManager dataManagerRef;

    void Start()
    {
        if (objectiveGiverRef == null)
        {
            objectiveGiverRef = FindAnyObjectByType<ObjectiveGiver>();
        }

       if (dataManagerRef.IsPlayerLoggedIn())
        {
            if (dataManagerRef.GetLoggedInPlayer() != null)
            {
                playerID = dataManagerRef.GetLoggedInPlayer().Email;
            }
        }
    }
    /// <summary>
    /// This function is called to complete the current quest and give the next quest to the player.
    /// </summary>
    public void CompleteQuest()
    {
        // Complete the current objective and increment the quest index
        objective.CompleteObjective();
        currentQuestIndex++; 

        // Checks if the player has anymore quest to give
        if (currentQuestIndex >= objectiveGiverRef.questList.Count)
        {
            // Reduce the quest index down by 1 to avoid out of range errors + ensure quest index retrieval is correct
            if (currentQuestIndex > 0)
            {
                currentQuestIndex--;
            }
            return;
        }

        // Else if there are more quests, give the next quest
        objectiveGiverRef.LoadQuest();
    }

}
