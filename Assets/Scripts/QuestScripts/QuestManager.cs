// using System.Collections.Generic;
// using UnityEngine;

// public class QuestManager : MonoBehaviour
// {
//     public List<Objective> questPool = new List<Objective>(); // All possible quests
    
//     [HideInInspector]
//     public Objective currentQuest; // Currently active quest
//     public Playe playerRef;
//     private int currentQuestIndex = 0;

//     void Start()
//     {
//         if (questPool.Count > 0)
//         {
//             currentQuestIndex = 0;
//             currentQuest = questPool[currentQuestIndex];
//             currentQuest.isActive = true;
//         }
//     }
//     // Initialize with a pool of quests
//     public void InitializeQuests(List<Objective> quests)
//     {
//         questPool = quests;
//         currentQuestIndex = 0;

//         if (questPool.Count > 0)
//         {
//             ActivateQuest(0);
//         }
//     }

//     // Activate a quest by index
//     private void ActivateQuest(int index)
//     {
//         if (index >= 0 && index < questPool.Count)
//         {
//             currentQuest = questPool[index];
//             currentQuest.isActive = true;
//             Debug.Log($"Quest Activated: {currentQuest.title}");
//         }
//     }

//    /// <summary>
//     /// Call this whenever player makes progress (scans or adopts a dog)
//     /// </summary>
//     public void UpdateCurrentQuestProgress()
//     {
//         if (currentQuest == null) return;

//         // Increment the progress for the current quest
//         currentQuest.goal.currentAmount++;

//         // Check if quest is complete
//         if (currentQuest.goal.IsReached())
//         {
//             CompleteCurrentQuest();
//         }
//     }

//     private void CompleteCurrentQuest()
//     {
//         currentQuest.CompleteObjective();

//         // Move to next quest if any
//         currentQuestIndex++;
//         if (currentQuestIndex < questPool.Count)
//         {
//             currentQuest = questPool[currentQuestIndex];
//             currentQuest.isActive = true;
//             Debug.Log("New quest started: " + currentQuest.title);
//         }
//         else
//         {
//             currentQuest = null;
//             Debug.Log("All quests completed!");
//         }
//     }
// }