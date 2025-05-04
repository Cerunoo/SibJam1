using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private ScriptableStats _stats;
    private Rigidbody2D _rb;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;

    [Header("States")]
    public bool facingTop;
    public bool disableMove;
    public bool disableDown;
    public bool disableRows;

    #region Interface

    public float Vertical => _frameInput.Vertical;
    public event Action<bool> Sniff;
    public event Action Crush;
    public event Action UnCrush;

    #endregion

    private float _time;

    private void Awake()
    {
        Instance = this;
        factPosX = transform.position.x;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            Vertical = Input.GetAxisRaw("Vertical"),
            Horizontal = _time - _timeRowWasPressed >= 0.3f ? Input.GetAxisRaw("Horizontal") : 0
        };
        if (_stats.SnapInput)
            _frameInput.Vertical = Mathf.Abs(_frameInput.Vertical) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Vertical);

        if (disableMove)
        {
            _frameInput.Vertical = 0;
            _frameInput.Horizontal = 0;
        }
        if (disableDown && _frameInput.Vertical < 0)
        {
            _frameInput.Vertical = 0;
        }
        if (disableRows)
        {
            _frameInput.Horizontal = 0;
        }

        if (_frameInput.Horizontal == -1)
        {
            _leftToConsume = true;
            _timeRowWasPressed = _time;
        }
        else if (_frameInput.Horizontal == 1)
        {
            _rightToConsume = true;
            _timeRowWasPressed = _time;
        }

        if (Input.GetAxisRaw("Horizontal") == 0) _timeRowWasPressed -= 0.4f;
    }

    private void FixedUpdate()
    {
        HandleRow();
        HandleVertical();

        ApplyMovement();
    }

    #region Row

    private bool _leftToConsume;
    private bool _rightToConsume;

    private float _timeRowWasPressed;

    [SerializeField] private int activeRow = 2;

    private float factPosX;

    private void HandleRow()
    {
        if (_leftToConsume)
        {
            if (activeRow - 1 >= 1)
            {
                ExecuteLeft();
            }
            else Sniff?.Invoke(false);
        }
        else if (_rightToConsume)
        {
            if (activeRow + 1 <= 3)
            {
                ExecuteRight();
            }
            else Sniff?.Invoke(true);
        }

        _leftToConsume = false;
        _rightToConsume = false;

        Vector2 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, factPosX, 12 * Time.deltaTime);
        transform.position = pos;
    }

    private void ExecuteLeft()
    {
        factPosX -= 2.74f;
        activeRow--;

        Sniff?.Invoke(false);
    }

    private void ExecuteRight()
    {
        factPosX += 2.74f;
        activeRow++;

        Sniff?.Invoke(true);
    }

    #endregion

    #region Vertical

    private void HandleVertical()
    {
        _frameVelocity = _rb.linearVelocity;

        if (_frameInput.Vertical == 0)
        {
            float deceleration = _stats.Deceleration;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, _frameInput.Vertical * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }

        if (_frameInput.Vertical > 0 && !facingTop) Flip();
        else if (_frameInput.Vertical < 0 && facingTop) Flip();
    }

    private void Flip()
    {
        facingTop = !facingTop;
        transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
    }

    #endregion

    private void ApplyMovement()
    {
        _rb.linearVelocityX = 0;
        _rb.linearVelocity = _frameVelocity;
    }

    public void SetCrush(bool hit)
    {
        if (hit)
        {
            Crush?.Invoke();
            disableMove = true;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            UnCrush?.Invoke();
            disableMove = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}

public struct FrameInput
{
    public float Vertical;
    public float Horizontal;
}

public interface IPlayerController
{
    public float Vertical { get; }
    public event Action<bool> Sniff;
    public event Action Crush;
    public event Action UnCrush;
}