using UnityEngine;

public class ToyInteraction : MonoBehaviour
{
    ToyManager toyManager; // Reference point to ToyManager script

    /// <summary>
    /// This function handles the interaction of the dog prefab with the toy prefabs, by using OnTriggerEnter
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {

            Debug.Log("Dog detected.");
            
            if (toyManager.ToyIndex == 0) // Plays bone biting audio
            {
                toyManager.audioSource.PlayOneShot(toyManager.boneBitingAudio);
            }

            if (toyManager.ToyIndex == 1) // Plays squeaky toy biting audio
            {
                toyManager.audioSource.PlayOneShot(toyManager.squeakyToyBitingAudio);
            }

            if (toyManager.ToyIndex == 2) // Plays tennis ball toy biting audio
            {
                toyManager.audioSource.PlayOneShot(toyManager.tennisBallBitingAudio);
            }

            Destroy(toyManager.currentToy); // Destroys toy
        }
    }

    void Start()
    {
        toyManager = FindFirstObjectByType<ToyManager>();
    }
}
