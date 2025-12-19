using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Rotation (torque)")]
    [SerializeField] private float torqueAmount = 1f;

    [Header("Throttle / Speed")]
    [SerializeField] private float speedForce = 8f;
    [SerializeField] private float maxSpeed = 12f;

    private InputAction moveAction;
    private Rigidbody2D rb2d;

    private float prevAngle;
    private float accumulatedAbsAngle = 0f;

    private int collectedCount = 0;

    private bool isAirborne = false;
    private float airAccumulatedAngle = 0f;
    private float airPrevAngle;
    private int trickPoints = 0;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction?.Enable();
         
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0f;  

        prevAngle = rb2d.rotation;
        airPrevAngle = rb2d.rotation;
    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        // Обертання (A / D)
        if (moveVector.x < 0f)
            rb2d.AddTorque(torqueAmount);
        else if (moveVector.x > 0f)
            rb2d.AddTorque(-torqueAmount);

        // Рух (W / S)
        float throttle = moveVector.y;
        if (Mathf.Abs(throttle) > 0.01f)
        {
            rb2d.AddForce((Vector2)transform.right * throttle * speedForce, ForceMode2D.Force);

            if (rb2d.linearVelocity.magnitude > maxSpeed)
                rb2d.linearVelocity = rb2d.linearVelocity.normalized * maxSpeed;
        }

        // Загальний підрахунок обертів
        float currAngle = rb2d.rotation;
        float delta = Mathf.DeltaAngle(prevAngle, currAngle);
        accumulatedAbsAngle += Mathf.Abs(delta);
        prevAngle = currAngle;

        // Підрахунок обертів у повітрі
        if (isAirborne)
        {
            float airDelta = Mathf.DeltaAngle(airPrevAngle, currAngle);
            airAccumulatedAngle += Mathf.Abs(airDelta);
            airPrevAngle = currAngle;
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAirborne)
        {
            int spins = Mathf.FloorToInt(airAccumulatedAngle / 360f);
            if (spins > 0)
            {
                int points = CalculateTrickPoints(spins);
                trickPoints += points;
                Debug.Log($"Приземлення! Оберти у повітрі: {spins}, Очки: {points}, Загальний рахунок: {trickPoints}");
            }

            airAccumulatedAngle = 0f; 
            isAirborne = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isAirborne)
        {
            isAirborne = true;
            airPrevAngle = rb2d.rotation;
        }
    }

    private int CalculateTrickPoints(int spins)
    {
        if (spins == 1) return 100;
        if (spins == 2) return 220;
        if (spins == 3) return 360;
        return spins * 150; 
    }

    public int GetRotationCount()
    {
        return Mathf.FloorToInt(accumulatedAbsAngle / 360f);
    }

    public void AddCollectedItem()
    {
        collectedCount++;
    }

    public int GetCollectedCount()
    {
        return collectedCount;
    }

    public int GetTrickPoints()
    {
        return trickPoints;
    }

    void OnDisable()
    {
        moveAction?.Disable();
    }
}
