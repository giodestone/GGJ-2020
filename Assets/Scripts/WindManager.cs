using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private Perlin PerlinX = new Perlin();
    private Perlin PerlinY = new Perlin();
    private Perlin PerlinZ = new Perlin();

    private Perlin Perlin = new Perlin();
    [SerializeField] private List<Vector3> IslandPositions; // For creating drag towards locations.

    [SerializeField] public Vector3 LevelSize = new Vector3(1024f, 256f, 1024f);
    [SerializeField] private float LevelScale = 1f;


    [SerializeField] private bool DrawDebug = true;

    private Rigidbody balloonRigidBody;

    private float WindForceMultiplyer = 10f;

    void Start()
    {
        PerlinX.SetSeed(0);
        PerlinY.SetSeed(1);
        PerlinZ.SetSeed(2);

        balloonRigidBody = GameObject.FindGameObjectWithTag("Baloon").GetComponentInChildren<Rigidbody>();
    }

    //private float GetNoise(Vector3 pos)
    //{
    //    return Perlin.Noise(pos.x / (LevelSize.x * LevelScale),
    //        pos.y / (LevelSize.y * LevelScale),
    //        pos.z / (LevelSize.z * LevelScale)); // Apply a scale to make the perlin noise a bit more spread out.
    //}

    public Vector3 GetWindAtPoint(Vector3 point, Vector3 worldSize)
    {

        float noisePerlinX = PerlinX.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinY = PerlinY.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);
        float noisePerlinZ = PerlinZ.Noise(point.x / worldSize.x, point.y / worldSize.y, point.z / worldSize.z);

        return new Vector3(noisePerlinX, noisePerlinY, noisePerlinZ) * WindForceMultiplyer;
    }

    private void OnDrawGizmos()
    {
        if (!DrawDebug) return;

        for (int x = -256; x < 256; x+=16)
        {
            for (int y = -128; y < 128; y+=16)
            {
                for (int z = -256; z < 256; z+=16)
                {
                    var posVector = new Vector3(x, y, z);
                    //posVector += balloonRigidBody.position;

                    var windVector = GetWindAtPoint(posVector, LevelSize);
                    //Gizmos.color = new Color(windVector.magnitude, 0f, 0f);
                    Gizmos.DrawLine(posVector, windVector + posVector);
                    Gizmos.DrawSphere(posVector, 0.05f);
                }
            }
        }
    }
}
