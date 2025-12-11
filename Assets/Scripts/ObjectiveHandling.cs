// This script was made by Andrew Burke to handle objectives in the game.
using UnityEngine;
using System.Collections.Generic;
using Firebase.Auth;

public class ObjectiveHandling : MonoBehaviour
{
    [Header("References")]
    public DataManager dataManagerRef;
    
    [Header("Gameplay")]
    public int ScorePerScan = 1;

    private HashSet<string> scannedDogs = new HashSet<string>();

    /// <summary>
    /// call this function when an ar dog is scanned
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
                dataManagerRef.UpdateCurrentObjective(player, ScorePerScan, ObjectiveType.ScanDog);
            }
        }
    }
}
