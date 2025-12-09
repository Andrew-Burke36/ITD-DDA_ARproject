// Created by Andrew Burke

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{

    public Button signUpButton;
    public Button loginButton;

    public TMP_Text togglePromptText;

    public string signUpPrompt = "Already have an account?";
    public string loginPrompt = "Don't have an account yet?";

    public GameObject uiRoot;
    
    /// <summary>
    ///  Closes the current UI panel.
    /// </summary>

    public void SwitchUI()
    {
        signUpButton.gameObject.SetActive(!signUpButton.gameObject.activeSelf);
        loginButton.gameObject.SetActive(!loginButton.gameObject.activeSelf);

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
        if (uiRoot != null)
        {
            uiRoot.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
