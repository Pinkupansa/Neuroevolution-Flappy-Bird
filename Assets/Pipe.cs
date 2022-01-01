using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private float speed;
    private bool canMove = false;
    private bool passed = false;
   
    private void Move()
    {
        transform.Translate(speed*Time.deltaTime*Vector2.left);
    }
    private void Update()
    {
        if(canMove)
        {
           
            Move();
        }
        if(transform.position.x < -10)
        {
            GameEvents.current.PipeDestroyed();
            Destroy(gameObject);
        }
    }
    public void Set(float _speed)
    {
        canMove = true;
        passed = false;
        speed = _speed;
    }
    public void OnTriggerEnter2D(Collider2D coll)
    {
        
        if(coll.gameObject.tag == "Player")
        {
            
            GameEvents.current.PlayerPassedPipe(!passed);
            passed = true;
        }
    }
    
}
