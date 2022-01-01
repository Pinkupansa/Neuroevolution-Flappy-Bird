using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
   [HideInInspector] public UnityEvent<bool> pipePassed;
   [HideInInspector] public UnityEvent pipeDestroyed;
    
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }
    public void PlayerPassedPipe(bool firstPassage)
    {
        pipePassed.Invoke(firstPassage);
        
    }
    public void PipeDestroyed()
    {
        pipeDestroyed.Invoke();
    }
   
    
}
