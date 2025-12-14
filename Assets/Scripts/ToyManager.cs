// Created by Vonce Chew
using System.Collections.Generic;
using UnityEngine;

public class ToyManager : MonoBehaviour
{
    // Variables
    public List<GameObject> toyPrefabs; // Stores all the toy prefabs

    public int ToyIndex; // Stores current index of toy

    public Transform spawnPoint; // Spawn point for toy prefabs

    public GameObject currentToy; // Stores current toy
    
    public AudioSource audioSource; // Audio source
     
    public AudioClip boneBitingAudio; // Audio for dog biting bone

    public AudioClip squeakyToyBitingAudio; // Audio for dog biting squeaky toy

    public AudioClip tennisBallBitingAudio; // Audio for dog biting tennis ball

    /// <summary>
    /// This function handles the instantiation of the toy prefabs
    /// </summary>
    /// <param name="index"></param>
    public void SpawnToy(int index)
    { 
        ToyIndex = index; // Save current index of the toy 

        if (index < 0 || index >= toyPrefabs.Count) // Safety check
        {
            Debug.LogWarning("Invalid prefab index.");
            return;
        }

        currentToy = Instantiate(toyPrefabs[index], spawnPoint.position, spawnPoint.rotation); // Instantiates prefab in accordance to index
    }
}
