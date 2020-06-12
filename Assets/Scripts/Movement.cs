using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Title("Parameters")] 
    [SerializeField] private float speed = 1500f;

    [SerializeField] private float directionChangementSpeed = 2f;
    [SerializeField] private float speedMax = 50f;
    [SerializeField, Range(-1, 1)] private float sameDirection = 0.85f;

    
    [SerializeField, Range(0, 1)] private float reducteurTerrestre = 0.5f;
    [SerializeField, Range(0, 1)] private float  reducteurAerien = 0.2f; 
    
    [SerializeField] private ForceMode forceMode = ForceMode.Force;
    [SerializeField, Range(0, 1)] private float limits = 0.5f;
    private int upgrade = 1;
    [Title("Other")]
    [SerializeField] private Text upgradeText;

    private bool upgradeDone;
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
        
        //Gestion des Upgrade
        var upgrd = Input.GetAxis("Upgrade");
        if (Mathf.Abs(upgrd) >limits)
        {
            if (!upgradeDone)
            {
                upgradeDone = true;
                var sign = Mathf.Sign(upgrd);
                upgrade += (int) sign;
                upgrade = Mathf.Clamp(upgrade, 1, 5);
            }

        }
        else
        {
            upgradeDone = false;
        }
        
        upgradeText.text = upgrade.ToString();
        //Fin gestion upgrade
        
        //GEstion des mouveent
        if ((Mathf.Abs(h) > limits || Mathf.Abs(v) > limits) )
        {
            //Vérifié que l'on ne dépasse pas le maximum voulu
            //Bonus : On a déjà indiquer la direction

            var direction = v * transform.forward;
            direction += h * transform.right;

            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            bool sameDirection = (Vector3.Dot(velocity.normalized, direction.normalized) >= this.sameDirection) || velocity.magnitude <= 1;
            bool speedIsntMax = velocity.magnitude < speedMax * upgrade;

            if (sameDirection)
            {
                if (speedIsntMax)
                {
                    //On indique la forcé à appliquer au joueur
                    rb.AddForce(direction * (speed), forceMode);
                }
            }
            else 
            {
                var speed = velocity.magnitude;
                velocity = velocity.normalized;
                velocity += direction * (directionChangementSpeed * dt);
                velocity = velocity.normalized * speed;
                velocity.y = y;
                rb.velocity = velocity;
            }
            
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
                ((overlapSomething) ? reducteurTerrestre : reducteurAerien) * (velocity.magnitude));
            
            velocity.y = y;
            rb.velocity = velocity;
        }
    }
}
