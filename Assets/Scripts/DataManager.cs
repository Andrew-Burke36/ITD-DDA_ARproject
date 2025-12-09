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
    

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
    }

    public void SignUp()
    {
        var signUpTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(SignInEmailInput.text, SignInPasswordInput.text);

        void CreatePlayerDetails(string email, string username)
        {
            Player newPlayer = new Player(email, username);
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
                CreatePlayerDetails(SignInEmailInput.text, SignInUsernameInput.text);
            }
        });
    }

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
                uiManagerRef.DisablePages("LoginUI");
                uiManagerRef.EnablePages("HomePage");                
            }
        });
    }

    // dog information



    // public void UpdateCRUD()
    // {
    //     // Update using UpdateChildrenAsync method
    //     Dictionary<string, object> data = new Dictionary<string, object>();
    //     data["damage"] = 25;
    //     data["capacity"] = 20;
    //     data["weapon"] = "Ghost";

    //     // Push the update
    //     mDatabaseRef.Child("Guns").Child("1").UpdateChildrenAsync(data);
    // }

    // public void DeleteCRUD()
    // {
    //     // Delete a value
    //     mDatabaseRef.Child("Guns").Child("2").RemoveValueAsync();

    // }
    
    // public void RetrieveCRUD()
    // {
    //     // Retrieve the data
    //     var retrieveData = mDatabaseRef.Child("Guns").Child("1").GetValueAsync();

    //     retrieveData.ContinueWithOnMainThread(task =>
    //     {
    //         if (task.IsFaulted || task.IsCanceled)
    //         {
    //             Debug.Log("Error retrieving data");
    //         }

    //         if (task.IsCompleted)
    //         {
    //             string jsonValue = task.Result.GetRawJsonValue();
    //             Guns gun = JsonUtility.FromJson<Guns>(jsonValue);
    //             Debug.Log($"Data retrieved successfully: Type: {gun.type}, Weapon: {gun.weapon}, Damage: {gun.damage}, Range: {gun.range}, Capacity: {gun.capacity}");
    //         }
    //     });
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        uiManagerRef = FindFirstObjectByType<uiManager>();
    }
}
