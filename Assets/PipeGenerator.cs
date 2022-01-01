using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PipeGenerator : MonoBehaviour
{
    public static PipeGenerator current;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform firstSpawnPoint, secondSpawnPoint;
    [SerializeField] private float ySpawnHalfExtent, pipeSpeed;
    [SerializeField] private GameObject pipePrefab;
    
    private List<Transform> pipesList = new List<Transform>();

    public Transform closestPipe {get; private set;}
    // Start is called before the first frame update
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }
    private void Start()
    {
        GameEvents.current.pipePassed.AddListener(OnPipePassed);
        GameEvents.current.pipeDestroyed.AddListener(OnPipeDestroyed);
        
        
        ResetPipes();
    }
    private void Update()
    {
        closestPipe = CalculateClosestPipe();
        Debug.DrawLine(closestPipe.position, closestPipe.position + Vector3.up);
    }
    public void ResetPipes()
    {
        pipesList = new List<Transform>();
        List<Pipe> pipes = GameObject.FindObjectsOfType<Pipe>().ToList();
        foreach(Pipe p in pipes)
        {
            Destroy(p.gameObject);
        }
        GeneratePipe(firstSpawnPoint.position);
        GeneratePipe(secondSpawnPoint.position);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(spawnPoint.transform.position, new Vector3(1, ySpawnHalfExtent * 2, 1));
    }
    private void OnPipePassed(bool firstPassage)
    {
        if(firstPassage)
        {
            
            GeneratePipe(spawnPoint.position);
        }
        
    }
    private void OnPipeDestroyed()
    {
       
        pipesList.RemoveAt(0);
    }
    private void GeneratePipe(Vector2 spawnPos)
    {
        GameObject instance = Instantiate(pipePrefab, spawnPos  + Random.Range(-ySpawnHalfExtent, ySpawnHalfExtent)*Vector2.up, Quaternion.identity);
        instance.GetComponent<Pipe>().Set(pipeSpeed);
        pipesList.Add(instance.transform);
    }
    private Transform CalculateClosestPipe()
    {
        float birdX = GameObject.FindObjectOfType<Bird>().transform.position.x;
        float  closestDist = Mathf.Infinity;
        Transform closest = pipesList.FirstOrDefault();
        foreach(Transform p in pipesList)
        {
            float d = p.position.x - birdX;
            
            if(d < closestDist && d > -1f)
            {
                
                closestDist = d;
                closest = p;
            }
        }
       // Debug.Log(closestDist);
        return closest;
    }
    
}
