using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private float timeToReloadScene = 2;
    private WaitForSeconds waitForSecconds;
    private void Start()
    {
        waitForSecconds = new WaitForSeconds(timeToReloadScene);
        ScaleUpAnChangeColor.instance.OnD�edEvent += �nstance_OnD�edEvent;
    }

    private void �nstance_OnD�edEvent(object sender, System.EventArgs e)
    {
        StartCoroutine(ReloadScene());
    }
    private void OnDisable()
    {
        ScaleUpAnChangeColor.instance.OnD�edEvent -= �nstance_OnD�edEvent;
    }
    private IEnumerator ReloadScene()
    {
        yield return waitForSecconds;
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
