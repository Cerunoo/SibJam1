using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private IPlayerController _player;

    private void OnEnable()
    {
        _player = GetComponentInParent<IPlayerController>();
        _player.Sniff += OnSniffChanged;
        _player.Crush += OnCrushed;
        _player.UnCrush += OnUnCrushed;
    }

    private void OnDisable()
    {
        _player.Sniff -= OnSniffChanged;
        _player.Crush -= OnCrushed;
        _player.UnCrush -= OnUnCrushed;
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

    private void OnCrushed()
    {
        _anim.SetTrigger(CrushKey);
    }

    private void OnUnCrushed()
    {
        _anim.SetTrigger("UnCrush");
    }

    private static readonly int MoveKey = Animator.StringToHash("Move");
    private static readonly int RightKey = Animator.StringToHash("Right");
    private static readonly int LeftKey = Animator.StringToHash("Left");
    private static readonly int CrushKey = Animator.StringToHash("Crush");
}