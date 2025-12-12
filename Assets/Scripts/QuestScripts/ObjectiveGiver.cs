// This script was made to give the player the objectives from the quest givers in the game.
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public uiManager uiManagerRef;
    public Playe playerRef;

    void Start() {
        GiveNextQuest();
    }

    /// <summary>
    /// This function gives the player the quest when they spawn into the game world and for the following quests.
    /// </summary>
    public void GiveNextQuest()
    {
        if (playerRef.currentQuestIndex >= playerRef.questList.Count)
        {
            Debug.Log("No more quests available.");
            return;
        }
        Objective nextQuest = playerRef.questList[playerRef.currentQuestIndex];
        
        nextQuest.isActive = true;
        playerRef.objective = nextQuest;

        uiManagerRef.UpdateObjectiveText(nextQuest.title);
        Debug.Log($"New Quest Given: {nextQuest.title}");
    }
}
