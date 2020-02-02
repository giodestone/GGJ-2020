using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Baloon : MonoBehaviour
{
    private WindManager WindManager;

    private Rigidbody balloonRigidBody;
    private float BaloonWeight;
    private float CartWeight;

    [SerializeField] private float BallonTemperatureKelvin = GROUND_TEMP_KELVIN;
    [SerializeField] private float DebugCTemp;
    private const float BALLOON_COOL_TEMP_KELVIN = GROUND_TEMP_KELVIN;
    private const float GROUND_TEMP_KELVIN = 273f + 20f; // 20C
    private const float STRATO_TEMP_KELVIN = 273f - 50f; // -50C

    private const float BALOON_VOLUME_LITRES = 2800000f; // 2.8 million litres
    private const float ATMO_CONSTANT = 0.08206f;

    private float CurrentAirPressure
    {
        get
        {
            //Debug.Log("My AtmoPress" + balloonRigidBody.position.y / StratoYPos);
            return Mathf.Clamp(Mathf.LerpUnclamped(AirPressureAtSea, AirPressureAtStrato, balloonRigidBody.position.y / StratoYPos), 0f, AirPressureAtSea);
        }
        //get { return AirPressureAtSea; }
    }
    private const float AirPressureAtSea = 1.03f; // in atm
    private const float AirPressureAtStrato = 0.4f; // in atm at height

    private float SeaYPos = 0f;
    public static readonly float StratoYPos = 2000f; // At what point does atmospheric pressue become same as AirPressureAtStrato

    private const float TempLossSeaLevel = 0.4f;
    private const float TempLossStrato = 1.5f;
    private float CurrentTemperatureLoss
    {
        get
        {
            return Mathf.Clamp(Mathf.LerpUnclamped(TempLossSeaLevel, TempLossStrato, transform.position.y / StratoYPos),
                TempLossSeaLevel, float.PositiveInfinity);
        }
    }

    private float MaxTempAtCurrentAltitude
    {
        get { return (((STRATO_TEMP_KELVIN - GROUND_TEMP_KELVIN) / StratoYPos) * balloonRigidBody.position.y) + GROUND_TEMP_KELVIN; } // straight line equation.
    }

    // Start is called before the first frame update
    void Start()
    {
        var baloon = GameObject.FindGameObjectWithTag("Baloon");
        var cart = GameObject.FindGameObjectWithTag("Cart");
        balloonRigidBody = baloon.GetComponentInChildren<Rigidbody>();

        BaloonWeight = baloon.GetComponentInChildren<Rigidbody>().mass;
        CartWeight = cart.GetComponentInChildren<Rigidbody>().mass;

        WindManager = GameObject.FindGameObjectWithTag("WindManager").GetComponent<WindManager>();
        var tst = GameObject.FindGameObjectWithTag("WindManager");
    }

    // Update is called once per frame
    void Update()
    {
        BallonTemperatureKelvin -= CurrentTemperatureLoss * Time.deltaTime;
        BallonTemperatureKelvin = Mathf.Clamp(BallonTemperatureKelvin, MaxTempAtCurrentAltitude, float.PositiveInfinity);
    }

    void FixedUpdate()
    {
        // Lift
        var kgOfLift = (GetMassOfAir(GetDensityOfCoolAirInBalloon()) - GetMassOfAir(GetDensityOfAirInBalloon())) /
                       100000f;
        var newtonsOfLift = kgOfLift * -Physics.gravity.y;
        var msminus2 = Mathf.Clamp(newtonsOfLift / BaloonWeight, 0f, float.PositiveInfinity);
        DebugCTemp = BallonTemperatureKelvin - 273f;

        Debug.Log(msminus2);
        balloonRigidBody.AddForce(0f, msminus2, 0f);


        // Wind
        balloonRigidBody.AddForce(WindManager.GetWindAtPoint(balloonRigidBody.position, WindManager.LevelSize));
    }

    /// <summary>
    /// Get how many moles the cool air is.
    /// </summary>
    /// <returns>Moles of air of 80N/20O in moles.</returns>
    float GetMolesOfCoolAirInBaloon()
    {
        return (CurrentAirPressure * BALOON_VOLUME_LITRES) / (ATMO_CONSTANT * BALLOON_COOL_TEMP_KELVIN);
    }

    /// <summary>
    /// Get desinity of air in a cool hot air balloon.
    /// </summary>
    /// <returns>Density of air at cool temp in grams over liters.</returns>
    private float GetDensityOfCoolAirInBalloon()
    {
        return (CurrentAirPressure * GetMolesOfCoolAirInBaloon()) / (ATMO_CONSTANT * BALLOON_COOL_TEMP_KELVIN);
    }

    /// <summary>
    /// How many moles is the air in the balloon currently.
    /// </summary>
    /// <returns>Moles of air in balloon.</returns>
    private float GetCurrentMolesInBalloon()
    {
        return (CurrentAirPressure * BALOON_VOLUME_LITRES) / (ATMO_CONSTANT * BallonTemperatureKelvin);
    }

    /// <summary>
    /// Density of air in balloon.
    /// </summary>
    /// <returns>Density of air in grams over liters.</returns>
    private float GetDensityOfAirInBalloon()
    {
        return (CurrentAirPressure * GetCurrentMolesInBalloon()) / (ATMO_CONSTANT * BallonTemperatureKelvin);
    }

    /// <summary>
    /// Get mass of air in balloon at density. Volume is factored in during calculation.
    /// </summary>
    /// <param name="density"></param>
    /// <returns>Grams of weight in balloon.</returns>
    private float GetMassOfAir(float density)
    {
        return density * BALOON_VOLUME_LITRES;
    }

    public void AddTemp(float tempToAdd)
    {
        BallonTemperatureKelvin += tempToAdd;
    }

    public float GetTempC()
    {
        return BallonTemperatureKelvin - 273f;
    }
}
