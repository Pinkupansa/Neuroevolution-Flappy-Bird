using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
using UnityEngine.UI;
using System.Linq;
public class EvolutionManager : MonoBehaviour
{
    [SerializeField] private int populationSize;
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private Transform birdSpawnPoint;
    [SerializeField] private Text fitnessText, generationNumberText;
    private int currentGeneration = 1;
    private int currentAlivePlayers;
    private AIPlayer[] currentPopulation;

    [SerializeField] int hiddenSize, hiddenNumber;
    // Start is called before the first frame update
    void Start()
    {
        currentPopulation = new AIPlayer[populationSize];
        for(int i = 0; i < populationSize; i++)
        {
            currentPopulation[i] = GeneratePlayer(new NeuralNet(5,hiddenSize,1, hiddenNumber));
            
           
        }
        currentAlivePlayers = populationSize;
        generationNumberText.text = currentGeneration.ToString();
    }
    private void NextGeneration()
    {
        currentGeneration ++;
        CalculateFitness();
        fitnessText.text = "Last max fitness : " + currentPopulation.Select(x => x.score).Max().ToString();
        PipeGenerator.current.ResetPipes();
        AIPlayer[] newPop = new AIPlayer[populationSize];
        List<AIPlayer> pool = ConstructPool();
        for(int i = 0; i < populationSize; i++)
        {
            //Destroy(currentPopulation[i].controlledBird.gameObject);
            currentPopulation[i].Die();
            
            newPop[i] = GeneratePlayer(pool[Random.Range(0, pool.Count - 1)].brain.DeepCopy());
            newPop[i].Mutate();
        }
        currentPopulation = newPop;
        currentAlivePlayers = populationSize;
        PipeGenerator.current.ResetPipes();
        generationNumberText.text = "Generation #" + currentGeneration.ToString();
    }
    // Update is called once per frame
    private AIPlayer GeneratePlayer(NeuralNet nn)
    {
        GameObject instance = new GameObject("AIPlayer");
        GameObject bird = Instantiate(birdPrefab, birdSpawnPoint.position, Quaternion.identity);
        instance.AddComponent<AIPlayer>().controlledBird = bird.GetComponent<Bird>();
        instance.GetComponent<AIPlayer>().died = new UnityEngine.Events.UnityEvent();
        instance.GetComponent<AIPlayer>().died.AddListener(OnPlayerDied);
        instance.GetComponent<AIPlayer>().Set(nn);
        
        return instance.GetComponent<AIPlayer>();
    }
    private void OnPlayerDied()
    {
        currentAlivePlayers -= 1;
        //Debug.Log(currentAlivePlayers);
        if(currentAlivePlayers == 0)
        {
            
            NextGeneration();
        }
    }   
    private void CalculateFitness()
    {
        double sum = 0;
        foreach(AIPlayer player in currentPopulation)
        {
            sum += player.score;
        }
        foreach(AIPlayer player in currentPopulation)
        {
            player.fitness = player.score/sum;
        }
    }
    private List<AIPlayer> ConstructPool()
    {
        List<AIPlayer> pool = new List<AIPlayer>();
        foreach(AIPlayer p in currentPopulation)
        {
            
            for(int i = 0; i < p.fitness*populationSize;i++)
            {
                pool.Add(p);
            }
        }
        return pool;
    }

}
