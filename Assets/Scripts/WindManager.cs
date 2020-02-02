using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private Baloon baloon;

    //private Perlin PerlinX = new Perlin();
    //private Perlin PerlinY = new Perlin();
    //private Perlin PerlinZ = new Perlin();

    //private Perlin Perlin = new Perlin();
    [SerializeField] private List<Transform> IslandPositions = new List<Transform>(); // For creating drag towards locations.
    [SerializeField] private float IslandPull = 1000f;
    [SerializeField] private float IslandPullDistanceRadius = 250f;

    [SerializeField] public Vector3 LevelSize = new Vector3(2000f, 2000f, 2000f);
    [SerializeField] private float LevelScale = 1f;

    [SerializeField] private bool DrawDebug = true;

    [SerializeField] private Vector3 WindDirection = new Vector3(1f, 0f, 0f); // Generally blow in the X axis.

    private Rigidbody balloonRigidBody;


    private const float MinWindMultiplierAtSeaLevel = 5f;
    private const float WindMultiperAtStrato = 200f;

    private float WindForceMultiplyer
    {
        get
        {
            return Mathf.Clamp(Mathf.LerpUnclamped(MinWindMultiplierAtSeaLevel, WindMultiperAtStrato,
                balloonRigidBody.position.y / Baloon.StratoYPos), MinWindMultiplierAtSeaLevel, float.PositiveInfinity );
        }
    }
    

    void Start()
    {
        //PerlinX.SetSeed(0);
        //PerlinY.SetSeed(1);
        //PerlinZ.SetSeed(2);

        balloonRigidBody = GameObject.FindGameObjectWithTag("Baloon").GetComponentInChildren<Rigidbody>();
        baloon = GameObject.FindObjectOfType<Baloon>();
    }

    public Vector3 GetWindAtPoint(Vector3 point, Vector3 worldSize)
    {

        //float noisePerlinX = PerlinX.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinX = Mathf.PerlinNoise( point.y / worldSize.y, point.z / worldSize.z);
        //float noisePerlinY = PerlinY.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinY = Mathf.PerlinNoise(point.x / worldSize.x, point.y / worldSize.y);
        //float noisePerlinZ = PerlinZ.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinZ = Mathf.PerlinNoise(point.x / worldSize.x,  point.z / worldSize.z);

        Vector3 finalPullToIslandVector3 = Vector3.zero;
        for (int i = 0; i < IslandPositions.Count; ++i)
        {
            Vector3 pullToIslandDirection = -(point - IslandPositions[i].position);
            float pullToIslandDistance = pullToIslandDirection.magnitude;
            pullToIslandDirection.Normalize();

            if (pullToIslandDistance < IslandPullDistanceRadius)
            {
                finalPullToIslandVector3 += pullToIslandDirection * Mathf.Lerp(0f, IslandPull, pullToIslandDistance);
            }
        }

        var final = new Vector3(noisePerlinX, noisePerlinY, noisePerlinZ); // Add windiness
        final += WindDirection; // Add the overall wind direction.
        final *= WindForceMultiplyer; // Make sure it gets windier the higher up you go.
        final += finalPullToIslandVector3; // Pull towards islands to make player life easier.

        return final;
    }

    private void OnDrawGizmos()
    {
        if (!DrawDebug || balloonRigidBody == null) return;

        const int step = 128;

        const int xzSize = 1024;
        const int ySize = 1000;

        for (int x = -xzSize; x < xzSize; x+= step)
        {
            for (int y = -ySize; y < ySize; y+= step)
            {
                for (int z = -xzSize; z < xzSize; z+= step)
                {
                    var posVector = new Vector3(x, y, z);
                    posVector += balloonRigidBody.position;

                    var windVector = GetWindAtPoint(posVector, LevelSize);
                    //Gizmos.color = new Color(windVector.magnitude, 0f, 0f);
                    Gizmos.DrawLine(posVector, windVector + posVector);
                    Gizmos.DrawCube(posVector, new Vector3(0.5f, 0.5f, 0.5f));
                }
            }
        }
    }
}
