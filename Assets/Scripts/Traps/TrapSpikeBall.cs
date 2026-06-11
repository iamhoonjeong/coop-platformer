using UnityEngine;

public class TrapSpikeBall : MonoBehaviour
{
    [SerializeField] Rigidbody2D spikeRb;
    [SerializeField] float pushForce;

    void Start()
    {
        Vector2 pushVector = new Vector2(pushForce, 0);
        spikeRb.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
