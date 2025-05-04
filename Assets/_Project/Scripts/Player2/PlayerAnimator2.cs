using UnityEngine;

public class PlayerAnimator2 : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private IPlayerController2 _player;

    private void OnEnable()
    {
        _player = GetComponentInParent<IPlayerController2>();
        _player.Sniff += OnSniffChanged;
    }

    private void OnDisable()
    {
        _player.Sniff -= OnSniffChanged;
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        _anim.SetBool(MoveKey, Mathf.Abs(_player.Vertical) > 0);
    }

    private void OnSniffChanged(bool rightSniff)
    {
        if (rightSniff) _anim.SetTrigger(RightKey);
        else _anim.SetTrigger(LeftKey);
    }

    private static readonly int MoveKey = Animator.StringToHash("Move");
    private static readonly int RightKey = Animator.StringToHash("Right");
    private static readonly int LeftKey = Animator.StringToHash("Left");
}