// Created by Andrew Burke and Vonce Chew to handle the UI management in the game.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class uiManager : MonoBehaviour
{
    public DataManager dataManager; // Reference point to datamanager script

    [Header("Login / Sign up UI")]
    // Ui elements variables
    public Button signUpButton;
    public Button loginButton;

    public TMP_Text togglePromptText;

    public string signUpPrompt = "Already have an account?";
    public string loginPrompt = "Don't have an account yet?";

    public GameObject loginUI;
    public GameObject signUpUI;

    [Header("Home Page UI")]
    public GameObject homePageUI;

    [Header("Others")]
    public GameObject userAuthUI;

    [Header("In Game UI")]
    public GameObject InGameUI;

    public TMP_Text objectiveText;
    public TMP_Text objectiveProgressText;
    public GameObject ToyUI;
    public GameObject EndGameUI;

    void Start()
    {
        // Initialize UI states
        if (signUpUI != null)
        {
            signUpUI.SetActive(false);
        }
        if (InGameUI != null)
        {
            InGameUI.SetActive(false);
        }

        if (ToyUI != null)
        {
            ToyUI.SetActive(false);
        }

        if(EndGameUI != null)
        {
            EndGameUI.SetActive(false);
        }
    } 

    /// <summary>
    ///  Switches the UI panel for the login and sign up function
    /// </summary>
    public void SwitchUI()
    {
       // Toggles the UI of the panels
       loginUI.SetActive(!loginUI.activeSelf);
       signUpUI.SetActive(!signUpUI.activeSelf);

        if (togglePromptText != null)
        {
            if (signUpButton.gameObject.activeSelf)
            {
                togglePromptText.text = signUpPrompt;
            }
            else
            {
                togglePromptText.text = loginPrompt;
            }
        }
    }

    /// <summary>
    ///  Closes the current UI panel.
    /// </summary>
    public void CloseUI()
    {
        if (loginUI != null)
        {
            loginUI.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    ///  Enables pages
    /// </summary>
    public void EnablePages(string pageName)
    {
        if (pageName == "HomePage")
        {
            if (homePageUI != null)
            {
                homePageUI.SetActive(true);
            }
        }

        else if (pageName == "LoginUI")
        {
            if (loginUI != null)
            {
                loginUI.SetActive(true);
            }
        }

        else if (pageName == "SignUpUI")
        {
            if (signUpUI != null)
            {
                signUpUI.SetActive(true);
            }
        }

        else if (pageName == "UserAuthUI")
        {
            if (userAuthUI != null)
            {
                userAuthUI.SetActive(true);
            }
        }

        else if(pageName == "InGameUI")
        {
            if (InGameUI != null)
            {
                InGameUI.SetActive(true);
            }
        }
        else if(pageName == "ToyUI")
        {
            if (ToyUI != null)
            {
                ToyUI.SetActive(true);
            }
        }
        else if(pageName == "EndGameUI")
        {
            if(EndGameUI != null)
            {
                EndGameUI.SetActive(true);
            }
        }

    }

    /// <summary>
    /// Disables pages
    /// </summary>
    public void DisablePages(string pageName)
    {
        if (pageName == "HomePage")
        {
            if (homePageUI != null)
            {
                homePageUI.SetActive(false);
            }
        }
        else if (pageName == "LoginUI")
        {
            if (loginUI != null)
            {
                Console.WriteLine("Disabling Login UI");
                loginUI.SetActive(false);
            }
        }
        else if (pageName == "SignUpUI")
        {
            if (signUpUI != null)
            {
                signUpUI.SetActive(false);
            }
        }

        else if (pageName == "UserAuthUI")
        {
            if (userAuthUI != null)
            {
                userAuthUI.SetActive(false);
            }
        }
        else if(pageName == "InGameUI")
        {
            if (InGameUI != null)
            {
                InGameUI.SetActive(false);
            }
        }
        else if (pageName == "ToyUI")
        {
            if (ToyUI != null)
            {
                ToyUI.SetActive(false);
            }
        }
        else if (pageName == "EndGameUI")
        {
            if (EndGameUI != null )
            {
                EndGameUI.SetActive(false);
            }
        }
    }

    public void TextUpdate(string type)
    {
        if (type == "Logout")
        {
            // Clear input fields and validation text on logout
            dataManager.EmailInput.text = "";
            dataManager.PasswordInput.text = "";
            dataManager.validationText.text = "";
        }
    }

    public void UpdateObjectiveProgress(int current, int required)
    {
        if (objectiveProgressText != null)
        {
            objectiveProgressText.text = $"Progress: {current} / {required}";
        }
    }

    
    /// <summary>
    /// This function updates the objective text
    /// </summary>
    public void UpdateObjectiveText(string Objective)
    {
        // objectiveText.text = dataManager.currentObjective;
        objectiveText.text = Objective;
    }
}
