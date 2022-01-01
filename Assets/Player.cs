using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    public UnityEvent died;
    public Bird controlledBird;
    public bool canPlay {get; protected set;}
    protected void Start()
    {
       
        canPlay = true;
        controlledBird.collided.AddListener(Die);
    }
    private void Die()
    {
        
        if(canPlay)
        {
            died.Invoke();
        }

        canPlay = false;
        
        
    }
    

}
