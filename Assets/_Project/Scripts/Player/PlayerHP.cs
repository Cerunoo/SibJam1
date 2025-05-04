using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider hp;
    public float val;

    [SerializeField] private float smoothness;

    private bool kill;
    private void Update()
    {
        hp.value = Mathf.MoveTowards(hp.value, val, smoothness * Time.deltaTime);

        if (val == 0 && !kill)
        {
            kill = true;

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
                FindAnyObjectByType<LoadingCircle>().LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}