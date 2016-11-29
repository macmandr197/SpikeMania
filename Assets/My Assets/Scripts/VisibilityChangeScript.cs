using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VisibilityChangeScript : MonoBehaviour
{
    public bool someBool = false;
    // Use this for initialization
    void Start()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SpikeMania");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in this.transform)
        {
            if (!someBool)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}