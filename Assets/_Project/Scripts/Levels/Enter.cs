using UnityEngine;
using System.Collections;

public class Enter : MonoBehaviour
{
    public int index;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.disableMove = true;
            if (PlayerController2.Instance != null) PlayerController2.Instance.disableMove = true;

            StartCoroutine(CallNull());
            IEnumerator CallNull()
            {
                yield return new WaitForSeconds(1);
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);

                while (FindAnyObjectByType<LoadingCircle>() == null)
                {
                    yield return null;
                }
                FindAnyObjectByType<LoadingCircle>().LoadScene(index);
            }
        }
    }
}
