using UnityEngine;

public class Coffee : Item, IUsable
{
    [SerializeField] [SyncValue] float throwDistance;
    [SerializeField] [SyncValue] float stunTime;
    [SerializeField] [SyncValue] GameObject coffeeTrail;
    [SerializeField] [SyncValue] GameObject coffeeDroplet;

    EnemyNavigation navigation;

    protected override void Start()
    {
        base.Start();
        navigation = FindObjectOfType<EnemyNavigation>();
    }

    public bool OnUse()
    {
        if (navigation == null)
        {
            Debug.Log("No enemy");
        }
        else
        {
            if (Vector3.Distance(navigation.transform.position, transform.position) <= throwDistance &&
                navigation.Visible)
            {
                GameObject instance = Instantiate(coffeeTrail, transform.position, Quaternion.identity);
                if (instance.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = HitTargetByAngle(transform.position, navigation.transform.position, Physics.gravity, 45);
                }
                if (instance.TryGetComponent(out CoffeeTrail trail))
                {
                    trail.stunTime = stunTime;
                }
                return true;
            }
        }

        GameObject instance2 = Instantiate(coffeeTrail, transform.position, Quaternion.identity);
        if (instance2.TryGetComponent(out Rigidbody rb2))
        {
            rb2.velocity = 5 * transform.forward;
        }
        if (instance2.TryGetComponent(out CoffeeTrail trail2))
        {
            trail2.stunTime = stunTime;
        }

        return true;
    }

    public Vector3 HitTargetByAngle(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float limitAngle)
    {
        if (limitAngle >= 90f || limitAngle <= -90f)
        {
            return Vector3.zero;
        }

        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));

        float radAngle = Mathf.Deg2Rad * limitAngle;
        float angleX = Mathf.Cos(radAngle);
        float angleY = Mathf.Sin(radAngle);

        float gravityMag = gravityBase.magnitude;

        if (verticalDistance / horizontalDistance > angleY / angleX)
        {
            return Vector3.zero;
        }

        float destSpeed = (1 / Mathf.Cos(radAngle)) * Mathf.Sqrt((0.5f * gravityMag * horizontalDistance * horizontalDistance) / ((horizontalDistance * Mathf.Tan(radAngle)) - verticalDistance));

        Vector3 launch = ((horizontal.normalized * angleX) - (gravityBase.normalized * angleY)) * destSpeed;
        return launch;
    }

    public Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase);
        perpendicular = Vector3.Cross(gravityBase, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        return output;
    }

    public Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityBase);
        return output;
    }
}
