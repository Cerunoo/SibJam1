using UnityEngine;

public class CarAnimator : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private IBotController _player;

    private void OnEnable()
    {
        _player = GetComponentInParent<IBotController>();
        _player.Crush += OnCrushed;
        _player.UnCrush += OnUnCrushed;
    }

    private void OnDisable()
    {
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

    private void OnCrushed()
    {
        _anim.SetTrigger(CrushKey);
    }

    private void OnUnCrushed()
    {
        _anim.SetTrigger("UnCrush");
    }

    private static readonly int MoveKey = Animator.StringToHash("Move");
    private static readonly int CrushKey = Animator.StringToHash("Crush");
}