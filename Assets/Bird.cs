using UnityEngine;
using UnityEngine.Events;
public class Bird : MonoBehaviour
{
   
   private Rigidbody2D rb;
   [SerializeField] private float jumpForce;
   [HideInInspector] public UnityEvent collided;
   [HideInInspector] public UnityEvent passedPipe;
   private void Start()
   {
       rb = GetComponent<Rigidbody2D>();
      
   }
   public void Jump()
   {
       rb.velocity = jumpForce*Vector2.up;
   }
   private void OnCollisionEnter2D(Collision2D coll)
   {
       collided.Invoke();
       Destroy(gameObject);
   }
   private void OnTriggerEnter2D(Collider2D coll)
   {
       if(coll.GetComponent<Pipe>() != null)
       {
           passedPipe.Invoke();
       }
   }
  
}
