using UnityEngine;

public class TrapArrow : Trap_Trampoline
{
    [Header("Additional info")]
    [SerializeField] float cooldown;
    [SerializeField] float rotationSpeed = 120f;
    [SerializeField] bool rotationRight;
    int direction = -1;
    [Space]
    [SerializeField] float scaleUpSpeed = 10f;
    [SerializeField] Vector3 targetScale;

    void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void Update()
    {
        HandleScaleUp();
        HandleRotation();
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        direction = rotationRight ? -1 : 1;
        transform.Rotate(0, 0, rotationSpeed * direction * Time.deltaTime);
    }

    void DestoroyMe()
    {
        GameObject arrowPrefab = ObjectCreator.instance.arrowPrefab;
        ObjectCreator.instance.CreateObject(arrowPrefab, transform, false, cooldown);

        Destroy(gameObject);
    }
}
