// Created by Andrew Burke and Vonce Chew

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;

public class DataManager : MonoBehaviour
{
    // TM textfields;
    [Header("Login Input fields")]
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public TMP_Text validationText;

    [Header("SignIn input fields")]
    public TMP_InputField SignInUsernameInput;
    public TMP_InputField SignInEmailInput;
    public TMP_InputField SignInPasswordInput;

    
    DatabaseReference mDatabaseRef;
    uiManager uiManagerRef;
 
    [HideInInspector]
    public string currentObjective; // String that stores player's current objective
    

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
    }

    /// <summary>
    /// This handles the sign up function for the player
    /// Pushes the player's data into the database that can be retrieved later.
    /// </summary>
    public void SignUp()
    {
        var signUpTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(SignInEmailInput.text, SignInPasswordInput.text);

        void CreatePlayerDetails(string email, string username, int currentObjective)
        {
            Player newPlayer = new Player(email, username, currentObjective);
            string json = JsonUtility.ToJson(newPlayer);

            mDatabaseRef.Child("Players").Child(signUpTask.Result.User.UserId).SetRawJsonValueAsync(json);
        }

        signUpTask.ContinueWithOnMainThread(signUpTask =>
        {
            if ( signUpTask.IsFaulted )
            {
                var baseException = signUpTask.Exception?.GetBaseException();

                if (baseException is FirebaseException)
                {
                    var firebaseException = baseException as FirebaseException;
                    var errorCode = (AuthError)firebaseException.ErrorCode;

                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            validationText.text = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            validationText.text = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            validationText.text = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            validationText.text = "Email Already In Use";
                            break;
                        default:
                            Debug.Log("Other error occurred: " + errorCode);
                            break;
                    }
                }
            }

            if (signUpTask.IsCompleted)
            {
                var uid = signUpTask.Result.User.UserId;
                validationText.text = $"Sign-up completed successfully!";

                StartCoroutine(Delay());
                uiManagerRef.SwitchUI();

                // Sends the player's details and player profile to the databaes
                CreatePlayerDetails(SignInEmailInput.text, SignInUsernameInput.text, 01);
            }
        });
    }

    /// <summary>
    /// This handles the sign in function for the player
    /// Retrieves the player's data from the database.
    /// </summary>
    public void SignIn()
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);

        signInTask.ContinueWithOnMainThread(task =>
        {
            var baseException = signInTask.Exception?.GetBaseException();

                if (baseException is FirebaseException)
                {
                    var firebaseException = baseException as FirebaseException;
                    var errorCode = (AuthError)firebaseException.ErrorCode;

                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            validationText.text = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            validationText.text = "Missing Password";
                            break;
                        case AuthError.InvalidEmail:
                            validationText.text = "Invalid Email";
                            break;
                        case AuthError.WrongPassword:
                            validationText.text = "Wrong Password";
                            break;
                        
                        default:
                            Debug.Log("Other error occurred: " + errorCode);
                            break;
                    }
                }

            if (task.IsCompleted)
            {
                var uid = signInTask.Result.User.UserId;
                validationText.text = $"Sign-in completed successfully!";

                StartCoroutine(Delay());
                uiManagerRef.DisablePages("UserAuthUI");
                uiManagerRef.EnablePages("HomePage");           
            }
        });
    }

    /// <summary>
    /// This function will handle the initialization of the player's dog data
    /// that will be pushed to their database profile
    /// </summary>
    public void InitializeDogData(string uid, DogClass adoptedDog)
    {
        if (uid == null || adoptedDog == null)
        {
            Debug.Log("Invalid user ID or dog data. Cannot initialize dog data.");
            return;
        }

        // Create a custom dog data object into the user
        string DogID = mDatabaseRef.Child("Players").Child(uid).Child("AdoptedDogs").Push().Key;

        // Convert dog class into a json
        string dogJson = JsonUtility.ToJson(adoptedDog);

        // Push the dog data into the player's profile under AdoptedDogs
        mDatabaseRef.Child("Players").Child(uid).Child("AdoptedDogs").Child(DogID)
            .SetRawJsonValueAsync(dogJson)
            .ContinueWithOnMainThread(task =>
             {
                if (task.IsCompleted)
                 {
                    Debug.Log("Dog data initialized successfully for user: " + uid);
                 }
                 else
                 {
                    Debug.Log("Failed to initialize dog data for user: " + uid);
                 }
             });
    }

    /// <summary>
    /// This function will handle the retrieving of the player's current objective
    /// This is to update the objective text in the in game UI
    /// </summary>
    public void RetrieveCurrentObjective()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        var playerCurrentObjective = mDatabaseRef.Child("Players").Child(user.UserId).GetValueAsync(); // Reads the values of the player's data

        playerCurrentObjective.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Unable to load player's data");
                return;
            }

            if (task.IsCompleted)
            {
                string playerData = task.Result.GetRawJsonValue(); // Gets json value 

                Player objective = JsonUtility.FromJson<Player>(playerData); // Deserializing

                string currentObjectiveString = objective.CurrentObjective.ToString(); // Storing current player's objective in currentObjectiveNumber variable
                
                var objectiveData = mDatabaseRef.Child("Objectives").GetValueAsync(); // Reads the values of the objectives' data.

                objectiveData.ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        
                        // Checks if key exists
                        if (snapshot.HasChild(currentObjectiveString))
                        {
                            DataSnapshot keySnapshot = snapshot.Child(currentObjectiveString); // Gets child snapshot 

                            string objectiveJsonValue = keySnapshot.Value.ToString(); // Gets string json value

                            currentObjective = objectiveJsonValue;
                        }
                        else
                        {
                            Debug.Log("Key not found.");
                        }
                    }
                    else
                    {
                        Debug.Log("Failure to retrieve from database.");
                    }
                });
            }
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        uiManagerRef = FindFirstObjectByType<uiManager>();
    }
}
