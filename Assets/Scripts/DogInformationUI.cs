// Created by Andrew Burke and Vonce Chew

using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class DogInformationUI : MonoBehaviour
{
    public Canvas dogInfoCanvas; // Dog UI Canvas

    /// <summary>
    /// Opens dog information UI when "about me" button is clicked
    /// </summary>
    public void OnAboutMeClicked()
    {
        if (dogInfoCanvas != null && dogInfoCanvas.gameObject != null)
        {
            dogInfoCanvas.gameObject.SetActive(true); // Turns on dog canvas when about me button is clicked
        }
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
