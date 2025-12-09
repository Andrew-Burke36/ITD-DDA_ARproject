// Created by Vonce Chew and Andrew Burke

using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DogInformationUI : MonoBehaviour
{
    // UI Variables and general variables
    public Canvas dogInfoCanvas; // Dog UI Canvas

    public TMP_Text dogNameText; // Dog name text
    
    public TMP_Text dogAgeText; // Dog age text

    public TMP_Text dogBreedText; // Dog breed text

    public TMP_Text dogPersonalityText; // Dog personality text

    public TMP_Text dogShortbioText; // Dog shortbio text

    public string dogName; // Used for checking which dog's data to retireve

    /// <summary>
    /// Opens dog information UI when "about me" button is clicked
    /// Loads dog information data from database and displays in UI
    /// </summary>
    public void OnAboutMeClicked()
    {
        var db = FirebaseDatabase.DefaultInstance.RootReference;
         
        if (dogInfoCanvas != null && dogInfoCanvas.gameObject != null)
        {
            dogInfoCanvas.gameObject.SetActive(true); // Turns on dog canvas when about me button is clicked
        }
        
        // Data Retrieval

        var dogRetrieveData = db.Child("Dogs").Child(dogName).GetValueAsync(); // Retrieves data for whatever dog specified in "dogName" variable

        dogRetrieveData.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Unable to load dog information"); // Displays error message
                return;
            }

            if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue(); // Loads json value

                DogClass dogData = JsonUtility.FromJson<DogClass>(json);

                dogNameText.text = "Name: " + dogData.Name; // Appends dog name from database to on screen dog name text

                dogAgeText.text = "Age: " + dogData.Age; // Appends dog age from database to on screen dog age text

                dogBreedText.text = "Breed: " + dogData.Breed; // Appends dog breed from database to on screen dog breed text

                dogPersonalityText.text = "Personality: " + dogData.Personality; // Appends dog personality from database to on screen dog personality text

                dogShortbioText.text = "Bio: " + dogData.Shortbio; // Appends dog shortbio from database to on screen dog personality text
                
                Debug.Log ("Dog information loaded successfully!");
            }
        });
    }

    /// <summary>
    /// Closes dog information UI when "close" button is clicked
    /// </summary>
    public void OnCloseClicked()
    {
        if (dogInfoCanvas != null && dogInfoCanvas.gameObject != null)
        {
            dogInfoCanvas.gameObject.SetActive(false); // Turns off dog canvas when close button is clicked
        }
    }

    private void Start()
    {
        Debug.Log("Dog information UI is attatched to:" + gameObject.name);
        if (dogInfoCanvas != null && dogInfoCanvas.gameObject != null)
        {
            dogInfoCanvas.gameObject.SetActive(false); // Ensures dog canvas is off at start
        }
    }
}
