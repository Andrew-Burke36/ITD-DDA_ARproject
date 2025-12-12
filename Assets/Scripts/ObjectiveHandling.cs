// This script was made by Andrew Burke to handle objectives in the game.
using UnityEngine;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Auth;

public class ObjectiveHandling : MonoBehaviour
{
    [Header("References")]
    public DataManager dataManagerRef;
    public Playe playerRef;
    private string objectiveType;

    
    [Header("Gameplay")]
    public int ScorePerScan = 1;

    // References to the player script that contains the objective
    private HashSet<string> scannedDogs = new HashSet<string>();

    /// <summary>
    /// call this function when an ar dog is scanned, this function will complete and update the player's objective
    /// </summary>
    public void DogScanned(string dogID)
    {
        var player = dataManagerRef.GetLoggedInPlayer();
        if (player == null)
        {
            return;
        }

        // dont scan the same dog twice
        if (scannedDogs.Contains(dogID))
        {
            return;
        }

        // if dog has not been scanned yet, jst add it to the list
        scannedDogs.Add(dogID);

        if (!player.ScannedPictures.Contains(dogID))
        {
            player.ScannedPictures.Add(dogID);

            if (player.ScannedPictures.Count <= dataManagerRef.currentScannedPictures)
            { 
                // Increment the objective goal
                if (playerRef == null)
                {
                    Debug.Log("player reference is null");
                    return;
                }
                else
                {
                    // Call the scanning dog function in the objective goal taht updates the local object player 
                    // playerRef.objective.goal.ScanningDog();
                    playerRef.objective.goal.ScanningDog();

                    if (playerRef.objective.goal.IsReached())
                    {
                        playerRef.CompleteQuest();
                    }

                    // Retrieves the local players data and pushes the updated objective progress to firebase for the logged player.
                    dataManagerRef.UpdateCurrentObjective(player, ObjectiveTypes.scanDog);
                }
            }
        }
    }

    public void DogAdopted(string dogID)
    {
        Debug.Log("Dog adopted called");
        var player = dataManagerRef.GetLoggedInPlayer();

        if (player == null)
        {
            return;
        }

        if (!player.AdoptedDogs.Contains(dogID))
        {
            player.AdoptedDogs.Add(dogID);
        }

        playerRef.objective.goal.AdoptingDog();
        if (playerRef.objective.goal.IsReached())
        {
            playerRef.CompleteQuest();
        }
        dataManagerRef.UpdateCurrentObjective(player, ObjectiveTypes.adoptDog);

    }
}
