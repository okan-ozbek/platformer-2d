using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    
    public void OnClickedStart()
    {
        StartCoroutine(LoadLevel());
    }

    public void OnClickedQuit()
    {
        Application.Quit();
    }

    private IEnumerator LoadLevel()
    {
        transition.SetBool("Crossfade", true);

        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene("Main scene");
    }
}
