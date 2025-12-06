using UnityEngine;
using TMPro;

public class DogInformationUI : MonoBehaviour
{
    public Canvas dogInfoCanvas; // Dog UI Canvas
    
    /// <summary>
    /// Opens dog information UI when "about me" button is clicked
    /// </summary>
    public void OnAboutMeClicked()
    {
        dogInfoCanvas.gameObject.SetActive(true); // Turns on dog canvas when about me button is clicked
    }

    /// <summary>
    /// Closes dog information UI when "close" button is clicked
    /// </summary>
    public void OnCloseClicked()
    {
        dogInfoCanvas.gameObject.SetActive(false); // Turns off dog canvas when close button is clicked
    }
}
