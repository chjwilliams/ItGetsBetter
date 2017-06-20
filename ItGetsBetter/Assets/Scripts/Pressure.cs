using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils;
using ChrsUtils.ChrsExtensionMethods;
using ChrsUtils.ChrsEventSystem.GameEvents;
using ItGetsBetterGameEvents;

public class Pressure : MonoBehaviour
{


    public bool scaledDown;
    public bool applyingPressure;
    public bool isBurning;
    public float moveSpeed;
    public Transform player;
    public TrailRenderer trail;
    public EasingProperties easing;

    private Vector3 _position;                   //    Reference to position
    private Rigidbody2D _rigidbody;
    private ApplyPressureEvent.Handler pressureApplied;

    // Use this for initialization
    void Start ()
    {
        scaledDown = false;
        applyingPressure = false;
        isBurning = false;
        moveSpeed = 0.7f;
        _position = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        easing = Resources.Load<EasingProperties>("Easing");
        pressureApplied = new ApplyPressureEvent.Handler(PressureApplied);
        Services.Events.Register<ApplyPressureEvent>(pressureApplied);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void PressureApplied(GameEvent e)
    {
        applyingPressure = true;
    }

    IEnumerator ScaleDown()
    {
        yield return StartCoroutine(Coroutines.DoOverEasedTime(1.0f, easing.ScaleDown, t =>
        {
            float scale = Mathf.Lerp(1.0f, 0.1f, t);
            transform.localScale = new Vector3(scale, scale, scale);
            trail.startWidth = scale * trail.startWidth;
        }));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "FirePosition")
        {
            isBurning = true;
            StartCoroutine(PauseMovement());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            scaledDown = true;
            StartCoroutine(ScaleDown());
            
        }
    }

    private IEnumerator PauseMovement()
    {
        moveSpeed = 0.0f;
        yield return new WaitForSeconds(3.0f);
        isBurning = false;
        moveSpeed = 0.7f;
    }

    public void FollowTarget(Transform target)
    {
        _position = transform.localPosition;
        _position = Vector3.Lerp(_position, target.localPosition, Time.deltaTime * moveSpeed);
        transform.localPosition = _position;

        if (scaledDown)
        {
            moveSpeed = 0;
        }
        _rigidbody.MovePosition(Vector3.MoveTowards (transform.position, player.transform.position, moveSpeed * Time.deltaTime));

    }

    protected void SinWaveMovement()
    {
        bool turn = false;            //
        float changeX = 1.0f;         //
        float changeY = 1.0f;         //

        //    Raycasting
        Vector2 lineCastPosition = transform.position.ToVector2();
        int collisionMask = 11;
        collisionMask = ~collisionMask;
        bool leftIsBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.up);
        bool rightIsBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down);
        bool frontIsBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition + transform.right.ToVector2());
        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.up, Color.green);
        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down, Color.red);
        Debug.DrawLine(lineCastPosition, lineCastPosition + transform.right.ToVector2(), Color.blue);

        float frequency = 7.0f;      //    Speed of the Sine movement
        float magnitude = 0.05f;       //    Size of sine movement

        float phase = Mathf.Sin(Time.time * frequency) * magnitude;
        Vector2 newVelocity = _rigidbody.velocity;
        newVelocity.y = phase;
        newVelocity.x = transform.up.x * Time.deltaTime * moveSpeed;

        transform.position = new Vector3(transform.position.x + moveSpeed * changeX * Time.deltaTime,
                                          phase + transform.position.y,
                                          0);

        _rigidbody.velocity = newVelocity;
    }

    // Update is called once per frame
    void Update ()
    {
        if (applyingPressure)
        {
            if (!isBurning)
            {
                SinWaveMovement();
            }
            FollowTarget(player);
        }
	}
}
