using System;
using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour, IBotController
{
    private Rigidbody2D _rb;
    private Vector2 _frameVelocity;
    public float MaxSpeed = 14;
    public float Acceleration = 120;
    public float Deceleration = 60;

    [SerializeField] private float vertical;

    [Header("States")]
    public bool facingTop;
    public float crushOut;

    #region Interface

    public float Vertical => vertical;
    public event Action Crush;
    public event Action UnCrush;

    #endregion

    public void InitializeUnit(float direction)
    {
        vertical = direction;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleVertical();

        ApplyMovement();
    }

    #region Vertical

    private void HandleVertical()
    {
        _frameVelocity = _rb.linearVelocity;

        _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, vertical * MaxSpeed, Acceleration * Time.fixedDeltaTime);

        if (vertical > 0 && !facingTop) Flip();
        else if (vertical < 0 && facingTop) Flip();

        if (crush)
        {
            _frameVelocity.y = 0;
        }
    }

    private void Flip()
    {
        facingTop = !facingTop;
        transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
    }

    #endregion

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

    [SerializeField] private bool crush;
    private Coroutine corC;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!crush)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController.Instance.gameObject.GetComponent<PlayerHP>().val--;
            }

            if (corC != null)
                {
                    StopCoroutine(corC);
                    corC = null;
                }
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                collision.gameObject.GetComponent<PlayerController>().SetCrush(true);
                corC = StartCoroutine(Uncrush(collision.gameObject.GetComponent<PlayerController>()));
                StartCoroutine(PlUncrush(collision.gameObject.GetComponent<PlayerController>()));
            }
            else
            {
                StartCoroutine(AltUnCrush());
            }
            Crush?.Invoke();
            crush = true;
        }

        IEnumerator Uncrush(PlayerController pl)
        {
            yield return new WaitForSeconds(crushOut);
            UnCrush?.Invoke();
            corC = null;
            crush = false;
            GetComponent<Collider2D>().enabled = false;
        }

        IEnumerator AltUnCrush()
        {
            yield return new WaitForSeconds(crushOut);
            UnCrush?.Invoke();
            corC = null;
            crush = false;
            GetComponent<Collider2D>().enabled = false;
        }

        IEnumerator PlUncrush(PlayerController pl)
        {
            yield return new WaitForSeconds(crushOut / 2);
            pl.SetCrush(false);
        }
    }
}

public interface IBotController
{
    public float Vertical { get; }
    public event Action Crush;
    public event Action UnCrush;
}