using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sourceBG;
    [SerializeField] private AudioSource sourceVFX;

    private float bg;
    private float vfx;

    [SerializeField] private Text bgText;
    [SerializeField] private Text vfxText;

    [SerializeField] private Color selColor;
    [SerializeField] private Color defColor;

    public AudioClip setVol;
    public AudioClip posVol;

    public void PullVFX(AudioClip clip)
    {
        sourceVFX.PlayOneShot(clip);
    }

    public void SetTexts(Text bg, Text vfx)
    {
        bgText = bg;
        vfxText = vfx;
    }

    private void OnEnable()
    {
        BG = PlayerPrefs.GetFloat("BG");
        VFX = PlayerPrefs.GetFloat("VFX");

        slot = 1;
        if (bgText) bgText.color = selColor;
        if (vfxText) vfxText.color = defColor;
    }

    int slot = 1;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Instance != null)
            {
                if (slot == 1)
                {
                    Instance.BG -= 15 * Time.deltaTime;
                }
                else
                {
                    Instance.VFX -= 15 * Time.deltaTime;
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Instance != null)
            {
                if (slot == 1)
                {
                    Instance.BG += 15 * Time.deltaTime;
                }
                else
                {
                    Instance.VFX += 15 * Time.deltaTime;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PullVFX(setVol);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PullVFX(setVol);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            slot = 1;
            if (bgText) bgText.color = selColor;
            if (vfxText) vfxText.color = defColor;
            PullVFX(posVol);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            slot = 2;
            if (bgText) bgText.color = defColor;
            if (vfxText) vfxText.color = selColor;
            PullVFX(posVol);
        }
    }

    public float BG
    {
        get => bg;
        set
        {
            if (value > 100) value = 100;
            else if (value < 0) value = 0;
            bg = value;
            PlayerPrefs.SetFloat("BG", value);

            if (bgText) bgText.text = $"BG: {value:F0}";

            sourceBG.volume = value / 100;
        }
    }
    public float VFX
    {
        get => vfx;
        set
        {
            if (value > 100) value = 100;
            else if (value < 0) value = 0;
            vfx = value;
            PlayerPrefs.SetFloat("VFX", value);

            if (vfxText) vfxText.text = $"VFX: {value:F0}";

            sourceVFX.volume = value / 100;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        if (PlayerPrefs.HasKey("BG") && PlayerPrefs.HasKey("VFX"))
        {
            bg = PlayerPrefs.GetFloat("BG");
            vfx = PlayerPrefs.GetFloat("VFX");
        }
        else
        {
            bg = 75;
            vfx = 50;
            PlayerPrefs.SetFloat("BG", bg);
            PlayerPrefs.SetFloat("VFX", vfx);
        }
    }
}