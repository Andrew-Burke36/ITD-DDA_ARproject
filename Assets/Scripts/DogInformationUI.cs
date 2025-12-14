// Created by Vonce Chew and Andrew Burke to handle the Dog information UI in the game and etc.

using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine.UI;

public class DogInformationUI : MonoBehaviour
{
    [Header("Dog Information UI Elements")]
    // UI Variables and general variables
    public Canvas dogInfoCanvas; // Dog UI Canvas

    public TMP_Text dogNameText; // Dog name text

    public TMP_Text dogAgeText; // Dog age text

    public TMP_Text dogBreedText; // Dog breed text

    public TMP_Text dogPersonalityText; // Dog personality text

    public TMP_Text dogShortbioText; // Dog shortbio text

    public Button AdoptMeBTN; // Adopt me button

    public string dogName; // Used for checking which dog's data to retireve
    

    [Header("Edit name information UI elements")]
    public Button editNameButton;
    public Canvas editDogNameCanvas;
    public TMP_Text currentDogNameText;
    public TMP_InputField newDogNameText;
    public Button editName;


    // Initialize references
    private DataManager dataManagerRef;
    private ObjectiveHandling objectiveHandlingRef;
    private DogUIInformation dogData;
    private DogClass dogPlayerData;

    private DogData dogData2;

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
                dogData2 = JsonUtility.FromJson<DogData>(json);
                dogData = JsonUtility.FromJson<DogUIInformation>(json);
                if (editNameButton != null)
                    editNameButton.interactable = true; // Now player can edit name

                dogPlayerData = new DogClass(dogData.Name, dogData.Age, dogData.Breed, dogData.Personality, true);

                dogNameText.text = "Name: " + dogData.Name; // Appends dog name from database to on screen dog name text

                dogAgeText.text = "Age: " + dogData.Age; // Appends dog age from database to on screen dog age text

                dogBreedText.text = "Breed: " + dogData.Breed; // Appends dog breed from database to on screen dog breed text

                dogPersonalityText.text = "Personality: " + dogData.Personality; // Appends dog personality from database to on screen dog personality text

                dogShortbioText.text = "Bio: " + dogData.Shortbio; // Appends dog shortbio from database to on screen dog personality text

                Debug.Log("Dog information loaded successfully!");
            }
        });

        // Disables adopt button if dog is already adopted
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        var adoptedDogData = db.Child("Players").Child(user.UserId).Child("AdoptedDogs").GetValueAsync();

        adoptedDogData.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool isAdopted = false;

                foreach (DataSnapshot dogSnapshot in snapshot.Children)
                {
                    DogClass adoptedDog = JsonUtility.FromJson<DogClass>(dogSnapshot.GetRawJsonValue());
                    if (adoptedDog.Breed == dogData.Breed)
                    {
                        Debug.Log("Dog has already been adopted.");
                        isAdopted = true;
                        break;
                    }
                }

                if (isAdopted)
                {
                    AdoptMeBTN.interactable = false; // Disables adopt me button if dog is already adopted
                    AdoptMeBTN.GetComponentInChildren<TMP_Text>().text = "Adopted"; // Changes button text to "Adopted"
                    Debug.Log("Adopt Me button disabled for adopted dog.");
                }
                else
                {
                    AdoptMeBTN.interactable = true; // Enables adopt me button if dog is not adopted
                    AdoptMeBTN.GetComponentInChildren<TMP_Text>().text = "Adopt Me"; // Changes button text to "Adopt Me"
                    Debug.Log("Adopt Me button enabled for unadopted dog.");
                }
            }
        });
    }

    /// <summary>
    /// This function will handle the creating of the Dog data 
    /// So that when the player adopts the dog, this class will be pushed to the data manager ref to be pushed 
    /// to the player's profile in the database
    /// </summary>
    public void OnAdoptClicked()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            Debug.Log("No user is signed in. Cannot adopt dog.");
            return;
        }

        if (dogData == null)
        {
            Debug.Log("No dog data loaded to adopt.");
            return;
        }

        // Push the data to the class in the data manager
        dataManagerRef.InitializeDogData(user.UserId, dogPlayerData);

        // Disables the button and changes text to adopted
        AdoptMeBTN.interactable = false; // Disables adopt me button after adoption
        AdoptMeBTN.GetComponentInChildren<TMP_Text>().text = "Adopted"; // Changes button text to "Adopted"

        // Update current objective for adopting dog
        objectiveHandlingRef.DogAdopted(dogData.Name);

        // Debugging purposes
        Debug.Log("Dog data has been initialized for adoption.");
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

    /// <summary>
    /// This function updates the current dog's name on the edit information UI
    /// </summary>
    public void OnEditNameClicked()
    {
        if (dogPlayerData == null)
        {
            Debug.LogWarning("No dog loaded. Open dog info first!");
            return;
        }

        if (editDogNameCanvas != null)
            editDogNameCanvas.gameObject.SetActive(true);

        if (currentDogNameText != null)
            currentDogNameText.text = dogPlayerData.Name;

        if (newDogNameText != null)
            newDogNameText.text = "";
    }

    /// <summary>
    /// This functions confirms the new dog's name when they press edit
    /// </summary>
    public void OnConfirmNameEdit()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null || dogPlayerData == null)
        {
            Debug.Log("Cannot rename dog – missing user or dog data");
            return;
        }

        string newName = newDogNameText.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            Debug.Log("Dog name cannot be empty");
            return;
        }

        string dogID = dogPlayerData.DogID;

        //  Update Firebase in REAL TIME
        FirebaseDatabase.DefaultInstance
            .RootReference
            .Child("Players")
            .Child(user.UserId)
            .Child("AdoptedDogs")
            .Child(dogID)
            .Child("Name")
            .SetValueAsync(newName)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    // Update local UI
                    dogPlayerData.Name = newName;
                    dogNameText.text = "Name: " + newName;

                    editDogNameCanvas.gameObject.SetActive(false);

                    // ✅ Trigger objective
                    objectiveHandlingRef.NameDog();

                    Debug.Log("Dog name updated successfully");
                }
            });
    }

    private void Start()
    {
        if (dogInfoCanvas != null && dogInfoCanvas.gameObject != null)
        {
            dogInfoCanvas.gameObject.SetActive(false); // Ensures dog canvas is off at start
        }

        if (editDogNameCanvas != null)
        {
            editDogNameCanvas.gameObject.SetActive(false);
        }

        dataManagerRef = FindFirstObjectByType<DataManager>();
        objectiveHandlingRef = FindFirstObjectByType<ObjectiveHandling>();

        if (editNameButton != null)
        editNameButton.interactable = false; // Disable until dog data loaded
    }
}
