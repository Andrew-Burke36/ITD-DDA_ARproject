// This script was made by andrew to give the player the objectives from the quest givers in the game.
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class ObjectiveGiver : MonoBehaviour
{
    public uiManager uiManagerRef;
    public Playe playerRef;

    [Header("Quest Chain")]
    public List<Objective> questList;
    void Start()
    {
        
    }

    /// <summary>
    /// This function gives the player the quest when they spawn into the game world and for the following quests.
    /// </summary>

    public void LoadQuest()
    {
        Debug.Log("load quest method was called");
        if (playerRef.currentQuestIndex >= questList.Count)
        {
            Debug.Log("No more quests available.");
            return;
        }

        Objective currentQuest = questList[playerRef.currentQuestIndex];

        currentQuest.isActive = true;
        playerRef.objective = currentQuest;

        uiManagerRef.UpdateObjectiveText(currentQuest.title);
        Debug.Log($"Quest Loaded: {currentQuest.title}");
    }
}
