using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject pauser;
    public AudioClip accClip;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(pauser);
    }

    bool start;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !start)
        {
            StartCoroutine(CallNull());
            start = true;
            SoundManager.Instance.PullVFX(accClip);
        }
    }

    private IEnumerator CallNull()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (FindAnyObjectByType<LoadingCircle>() == null)
        {
            yield return null;
        }
        FindAnyObjectByType<LoadingCircle>().LoadScene(2);
    }
}