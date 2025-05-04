using Unity.Cinemachine;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private IPlayerController _player;

    [SerializeField] private CinemachineCamera vCam;
    private float startLens;

    [SerializeField, Space(5)] private float speed;

    private void Start()
    {
        _player = GetComponentInParent<IPlayerController>();
        startLens = vCam.Lens.OrthographicSize;
    }

    private void Update()
    {
        vCam.Lens.OrthographicSize = Mathf.MoveTowards(vCam.Lens.OrthographicSize, startLens + Mathf.Abs(_player.Speed / 15), Time.deltaTime * speed);
    }
}