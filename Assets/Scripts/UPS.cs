using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class UPS : MonoBehaviour
{
    [TitleGroup("UPS Parameters")]
    [SerializeField,Range(0,1)] private float refreshTime = 0.5f;
    [SerializeField] private Text UPSAffichagePlayer;
    [SerializeField] private Text RealVelocity;
    [SerializeField] private int numberOfNumberOfterComa = 2;

    private int textSize;
    private float nextTimer = -1;
    
    // [SerializeField] private GameObject[] players = new GameObject[2];

    private List<Vector3> previousPositionPlayer = new List<Vector3>();
    [HideInInspector] public float actualsUPSPlayer = 0;

    private float playerNumber;

    private float timePass;
    // Start is called before the first frame update
    void Awake()
    {
        previousPositionPlayer = new List<Vector3>();
        nextTimer = Time.time + refreshTime;
        timePass = 0;
    }

    private void Start()
    {
        textSize = UPSAffichagePlayer.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        previousPositionPlayer.Add(transform.position);
        timePass += Time.deltaTime;
        if (Time.time >= nextTimer)
        {
            ActualizeSpeed();
        }
    }

    private void ActualizeSpeed()
    {
        actualsUPSPlayer = 0;
        var distance = new List<float>();
        if (previousPositionPlayer.Count > 0)
        {
            for (int j = 1; j < previousPositionPlayer.Count; j++)
            {
                distance.Add(Vector3.Distance(previousPositionPlayer[j], previousPositionPlayer[j - 1]));
            }

            for (int j = 0; j < distance.Count; j++)
            {
                actualsUPSPlayer += distance[j];
            }

            actualsUPSPlayer /= timePass;
        }
        
        var vitesse = (actualsUPSPlayer < 0.5f)? 0f : actualsUPSPlayer;
        if (vitesse<1000)
        {
            UPSAffichagePlayer.fontSize = textSize;
            UPSAffichagePlayer.text =
                        LeadboardSetter.RoundValue(vitesse, Mathf.Pow(10, numberOfNumberOfterComa))
                            .ToString(CultureInfo.CurrentUICulture);
        }
        else
        {
            UPSAffichagePlayer.fontSize = (int)(textSize / 1.8f);
            UPSAffichagePlayer.text = "Trop Rapide";
        }


        var velocity = GetComponent<Rigidbody>().velocity;
        RealVelocity.text = LeadboardSetter.RoundValue(velocity.magnitude, Mathf.Pow(10, numberOfNumberOfterComa))
            .ToString(CultureInfo.CurrentUICulture);;
        previousPositionPlayer.Clear();
        //actualsUPSPlayer = 0;
        nextTimer = Time.time + refreshTime;
        timePass = 0;
    }
}
