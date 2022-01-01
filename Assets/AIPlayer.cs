using NeuralNetwork;
using UnityEngine;
using System.Collections.Generic;
public class AIPlayer : Player
{
    public double score{get; private set;}
    public double fitness;
    public NeuralNet brain{get; private set;} //input : yvelocity, ybirdpos, xpipepos, ypipepos
    private float birthTime;
    public void Set(NeuralNet _brain)
    {
        brain = _brain;
    }
    new void Start()
    {
        base.Start();
        birthTime = Time.time;
        controlledBird.passedPipe.AddListener(OnPassedPipe);
        
       
    }
    private void Think()
    {
        //double yVelocity = controlledBird.GetComponent<Rigidbody2D>().velocity.y/10f;
        double y = controlledBird.transform.position.y/5.0;
        double nextPipeX = PipeGenerator.current.closestPipe.position.x/5.0;
        double nextPipeTopY = (PipeGenerator.current.closestPipe.position.y+6.5)/8.0;
        double nextPipeBottomY = (PipeGenerator.current.closestPipe.position.y-6.5)/8.0;
        double yVelocity = controlledBird.GetComponent<Rigidbody2D>().velocity.y/25.0;
        double[] inputs = new double[5]{nextPipeX,nextPipeTopY,nextPipeBottomY, y, yVelocity};
        brain.ForwardPropagate(inputs);
        //Debug.Log(string.Format("next pipe x : {0}, next pipe top y : {1},  nextPipeBottomY : {2}, y : {3}, yVelocity : {4}", new object[5]{nextPipeX, nextPipeTopY, nextPipeBottomY, y, yVelocity}));
        
        if(brain.OutputLayer[0].Value > 0.5)
        {
            
            controlledBird.Jump();
        }
    }
    private void Update()
    {
        if(canPlay && PipeGenerator.current.closestPipe != null)
        {
            score += Time.deltaTime*(Time.time-birthTime);
            //score += (Time.deltaTime * (Mathf.PI/2-Mathf.Atan(Mathf.Abs(PipeGenerator.current.closestPipe.position.y - transform.position.y))))/5f;
            Think();
        }
        
    }
    private void OnPassedPipe()
    {
        score += canPlay?70:0;
    }
    public void Mutate()
    {
        float mutationProbability = 0.0015f;
        foreach(List<Neuron> layer in brain.HiddenLayers)
        {
            
            foreach(Neuron n in layer)
            {
                if(Random.Range(0f,1f) < mutationProbability)
                {
                    
                    n.Bias *= Random.Range(0.8f, 1.25f);
                }
                foreach(Synapse s in n.InputSynapses)
                {
                    if(Random.Range(0f,1f) < mutationProbability)
                    {
                        
                        //Debug.Log(s.Weight);
                        s.Weight*= Random.Range(0.8f, 1.25f);
                    }
                }
            }
        }
        foreach(Neuron n in brain.OutputLayer)
        {
            if(Random.Range(0f,1f) < mutationProbability)
            {
                n.Bias *= Random.Range(0.8f, 1.25f);
            }
            foreach(Synapse s in n.InputSynapses)
            {
                if(Random.Range(0f,1f) < mutationProbability)
                {
                    s.Weight*= Random.Range(0.8f, 1.25f);
                }
            }
        }
    }
    public void Die()
    {
        Destroy(gameObject, 1f);
    }
  
}
