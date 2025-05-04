using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private Text bg;
    [SerializeField] private Text vfx;

    private bool open;
    private void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) return;

        SoundManager.Instance.SetTexts(bg, vfx);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!open)
            {
                wrapper.SetActive(true);
                open = true;
                if (PlayerController.Instance) PlayerController.Instance.disableMove = true;
                if (PlayerController2.Instance) PlayerController2.Instance.disableMove = true;
            }
            else
            {
                wrapper.SetActive(false);
                open = false;
                if (PlayerController.Instance) PlayerController.Instance.disableMove = false;
                if (PlayerController2.Instance) PlayerController2.Instance.disableMove = false;
            }
        }
    }
}
