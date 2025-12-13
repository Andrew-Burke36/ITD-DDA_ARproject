// Created by Andrew Burke and Vonce Chew to handle firebase data management such as authentication and database updating.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using System;
using NUnit.Framework;
using UnityEngine.SocialPlatforms;


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
    Playe localPlayerObjRef;
    ObjectiveGiver objectiveGiverRef;

    private PlayerClass loggedInPlayer;

    // Player objective data
    public int currentScannedPictures = 2;

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

        void CreatePlayerDetails(string email, string username, ObjectiveTypes currentObjective)
        {
            PlayerClass newPlayer = new PlayerClass(email, username, currentObjective, 0, 0);
            string json = JsonUtility.ToJson(newPlayer);

            mDatabaseRef.Child("Players").Child(signUpTask.Result.User.UserId).SetRawJsonValueAsync(json);
        }

        signUpTask.ContinueWithOnMainThread(signUpTask =>
        {
            if (signUpTask.IsFaulted)
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
                CreatePlayerDetails(SignInEmailInput.text, SignInUsernameInput.text, ObjectiveTypes.scanDog);
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
            if (task.IsFaulted)
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
                        case AuthError.UserNotFound:
                            validationText.text = "User not found";
                            break;
                        default:
                            validationText.text = "Error logging in, invalid email or password.";
                            break;
                    }
                }
                return; 
            }

            if (task.IsCompleted)
            {
                string uid = task.Result.User.UserId;
                validationText.text = $"Sign-in completed successfully!";
                
                StartCoroutine(Delay());
                uiManagerRef.DisablePages("UserAuthUI");
                uiManagerRef.EnablePages("HomePage");

                mDatabaseRef.Child("Players").Child(uid).GetValueAsync().ContinueWithOnMainThread(playerTask =>
                {
                    if (playerTask.IsFaulted)
                    {
                        Debug.Log("Failed to retrieve player data.");
                        return;
                    }
                    if (playerTask.IsCompleted)
                    {
                        Debug.Log("objective loaded");
                        string json = playerTask.Result.GetRawJsonValue();
                        loggedInPlayer = JsonUtility.FromJson<PlayerClass>(json);
                        
                        // load in data ( test )
                        RetrieveCurrentObjective();
                    }
                });
            }
            
        });
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        loggedInPlayer = null;

        // Switches UI back to login screen
        uiManagerRef.EnablePages("UserAuthUI");
        uiManagerRef.DisablePages("HomePage");
        uiManagerRef.DisablePages("InGameUI");

        // Clear input fields and validation text on logout
        uiManagerRef.TextUpdate("Logout");
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
        string DogID = adoptedDog.DogID;

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
        
        if (user == null)
        {
            Debug.Log("No user is signed in. Cannot retrieve current objective.");
            return;
        }

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

                PlayerClass objective = JsonUtility.FromJson<PlayerClass>(playerData); // Deserializing

                int currentObjective= (int)objective.CurrentObjective; // Storing current player's objective in currentObjectiveNumber variable
                localPlayerObjRef.objective.goal.objectiveType = objective.CurrentObjective;

                // Retrieves the necessary data from the objective to update the in game UI
                int currentObjectiveProgress = objective.CurrentObjectiveProgress;
                localPlayerObjRef.objective.goal.currentAmount = currentObjectiveProgress;

                // Set the data manager's current objective index
                int currentObjIndex = objective.CurrentObjectiveIndex;
                localPlayerObjRef.currentQuestIndex = currentObjIndex;

                // Call to update the objective text in the in game UI
                localPlayerObjRef.objective.title = GetObjectiveTitle(objective.CurrentObjective);
                uiManagerRef.UpdateObjectiveText(localPlayerObjRef.objective.title);

                localPlayerObjRef.objectiveGiverRef.LoadQuest();
            }
        });
    }

    private string GetObjectiveTitle(ObjectiveTypes objectiveType)
    {
        switch (objectiveType)
        {
            case ObjectiveTypes.scanDog:
                return "Scan Dogs";
            case ObjectiveTypes.adoptDog:
                return "Adopt Dogs";
            default:
                return "Unknown Objective";
        }
    }

    // Objective updating
    /// <summary>
    /// This function will handle the updating of the player's data in the database
    /// </summary>
    public async void UpdatePlayer(PlayerClass player)
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            Debug.Log("No user is signed in. Cannot update player data.");
            return;
        }

        string json = JsonUtility.ToJson(player);
        await mDatabaseRef.Child("Players").Child(user.UserId).SetRawJsonValueAsync(json);
    }

    // / <summary>
    // / This function will handle the updating of the player's current objective in the database
    // / </summary>
    public async void UpdateCurrentObjective(PlayerClass player)
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (!IsPlayerLoggedIn() || player != loggedInPlayer)
        {
            Debug.Log("No user was found, cannot update the current objective.");
            return;
        }
            
        player.CurrentObjectiveProgress = localPlayerObjRef.objective.goal.currentAmount;
        player.CurrentObjective = localPlayerObjRef.objective.goal.objectiveType;
        player.CurrentObjectiveIndex = localPlayerObjRef.currentQuestIndex;

        // Update necessary fields
        var updateObjectives = new Dictionary<string, object>
        {
            {"CurrentObjectiveProgress", player.CurrentObjectiveProgress},
            {"CurrentObjective", (int)player.CurrentObjective},
            {"ScannedPictures", player.ScannedPictures.ToArray()},
            {"CurrentObjectiveIndex", player.CurrentObjectiveIndex}

        };
        
        await mDatabaseRef.Child("Players").Child(user.UserId).UpdateChildrenAsync(updateObjectives);

        Debug.Log("Objective updated, new objective: " + player.CurrentObjective);
    }


    public PlayerClass GetLoggedInPlayer()
    {
        return loggedInPlayer;
    }
    public bool IsPlayerLoggedIn()
    {
        return loggedInPlayer != null && FirebaseAuth.DefaultInstance.CurrentUser != null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        uiManagerRef = FindFirstObjectByType<uiManager>();
        localPlayerObjRef = FindFirstObjectByType<Playe>();
    }
}
