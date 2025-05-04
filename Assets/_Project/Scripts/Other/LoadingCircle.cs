using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingCircle : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private AsyncOperation operation;

    public void LoadScene(int index)
    {
        anim.SetTrigger("Start");
        StartCoroutine(LoadSceneCoroutine(index));
    }
    public void pressActiveScene() => operation.allowSceneActivation = true;

    private IEnumerator LoadSceneCoroutine(int i)
    {
        yield return null;
        operation = SceneManager.LoadSceneAsync(i);
        operation.allowSceneActivation = false;
    }
}