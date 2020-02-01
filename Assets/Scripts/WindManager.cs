using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private Baloon baloon;

    private Perlin PerlinX = new Perlin();
    private Perlin PerlinY = new Perlin();
    private Perlin PerlinZ = new Perlin();

    private Perlin Perlin = new Perlin();
    [SerializeField] private List<Transform> IslandPositions = new List<Transform>(); // For creating drag towards locations.
    [SerializeField] private float IslandPull = 10000f;
    [SerializeField] private float IslandPullDistanceRadius = 200f;

    [SerializeField] public Vector3 LevelSize = new Vector3(2000f, 2000f, 2000f);
    [SerializeField] private float LevelScale = 1f;


    [SerializeField] private bool DrawDebug = true;

    private Rigidbody balloonRigidBody;


    private const float MinWindMultiplierAtSeaLevel = 50f;
    private const float WindMultiperAtStrato = 2000f;

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
        PerlinX.SetSeed(0);
        PerlinY.SetSeed(1);
        PerlinZ.SetSeed(2);

        balloonRigidBody = GameObject.FindGameObjectWithTag("Baloon").GetComponentInChildren<Rigidbody>();
        baloon = GameObject.FindObjectOfType<Baloon>();
    }

    public Vector3 GetWindAtPoint(Vector3 point, Vector3 worldSize)
    {

        float noisePerlinX = PerlinX.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinY = PerlinY.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinZ = PerlinZ.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);

        Vector3 pullToIslandDirection = point - IslandPositions[0].position;
        float pullToIslandDistance = pullToIslandDirection.magnitude;
        pullToIslandDirection.Normalize();

        Vector3 finalPullToIsland = Vector3.zero;
        if (pullToIslandDistance < IslandPullDistanceRadius)
        {
            ///TODO ADD THOSE PULL INTO SPHERES.
        }

        return new Vector3(noisePerlinX, noisePerlinY, noisePerlinZ) * WindForceMultiplyer + finalPullToIsland;
    }

    private void OnDrawGizmos()
    {
        if (!DrawDebug) return;

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
                    Gizmos.DrawSphere(posVector, 0.5f);
                }
            }
        }
    }
}
