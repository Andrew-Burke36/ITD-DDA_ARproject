using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class uiManager : MonoBehaviour
{
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
    }
}
