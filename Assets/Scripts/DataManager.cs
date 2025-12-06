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
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public TMP_Text validationText;

    DatabaseReference mDatabaseRef;
    LoginUI uiManagerRef;
    

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
    }

    public void SignUp()
    {
        var signUpTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);

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
                uiManagerRef.CloseUI();
            }
        });
    }

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
        uiManagerRef = FindFirstObjectByType<LoginUI>();

        // WriteNewGun("1", "Pistol", "classic", 10, "Medium", 15);
        // WriteNewGun("2", "Rifle", "Vandal", 35, "Long", 30);
    }

    // private void WriteNewGun(string gunID, string type, string weapon, int damage, string range, int capacity)
    // {
    //     Guns gun = new Guns(type, weapon, damage, range, capacity);
    //     string json = JsonUtility.ToJson(gun);

    //     mDatabaseRef.Child("Guns").Child(gunID).SetRawJsonValueAsync(json);
    // }
}
