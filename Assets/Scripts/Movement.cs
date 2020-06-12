using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Title("Parameters")] 
    [SerializeField] private float speed = 1500f;

    [SerializeField] private float speedMax = 50f;
    
    [SerializeField, Range(0, 1)] private float reducteurTerrestre = 0.5f;
    [SerializeField, Range(0, 1)] private float  reducteurAerien = 0.2f; 
    
    [SerializeField] private ForceMode forceMode = ForceMode.Force;
    [SerializeField, Range(0, 1)] private float limits = 0.5f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Récupéré les Inputs
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var dt = Time.fixedDeltaTime; //Récupération du delta Time au cas ou je voudrais le modifié
        var velocity = rb.velocity;
        var y = velocity.y;
        velocity.y = 0;
        if ((Mathf.Abs(h) > limits || Mathf.Abs(v) > limits) && velocity.magnitude < speedMax)
        {
            //Vérifié que l'on ne dépasse pas le maximum voulu
            //Bonus : On a déjà indiquer la direction

            var direction = v * transform.forward;
            direction += h * transform.right;

            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            //On indique la forcé à appliquer au joueur
            rb.AddForce(direction * (speed), forceMode);
        }
        else
        {
            //Réducteur de vitesse en fonction de si l'on est en l'air ou au sol
            
            bool overlapSomething = false;
            var test = Physics.OverlapSphere(transform.position + (Vector3.down), 0.2f);
            for (int i = 0; i < test.Length; i++)
            {
                if (!test[i].CompareTag("Player"))
                {
                    overlapSomething = true;
                    break;
                }
            }

            Debug.Log("On utilise " + ((overlapSomething) ? "un reducteur terrestre" : "un reducteur aerien"));
            velocity = Vector3.MoveTowards(velocity, Vector3.zero,
                ((overlapSomething) ? reducteurTerrestre : reducteurAerien) * (speed));
            
            velocity.y = y;
            rb.velocity = velocity;
        }
    }
}
