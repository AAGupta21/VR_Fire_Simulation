using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Controller : MonoBehaviour
{
    private bool Windy = false;

    [SerializeField] private Transform FanPos = null;
    [SerializeField] private float Wind_Magnitude = 10f;
    [SerializeField] private float Fan_Range = 50f;
    [SerializeField] private ParticleSystem Stick_PS = null;
    [SerializeField] private float TimeToDestroyStick = 15f;
    
    private void Update()
    {
        if (Windy)
            WindEffect();
    }

    private void WindEffect()
    {
        Vector3 Direc = (transform.position - FanPos.position).normalized;

        var Forc = transform.GetChild(0).GetComponent<ParticleSystem>().forceOverLifetime;

        Forc.x = Direc.x * (1 - (Vector3.Distance(transform.position, FanPos.position) / Fan_Range)) * Wind_Magnitude;
        Forc.y = Direc.y * (1 - (Vector3.Distance(transform.position, FanPos.position) / Fan_Range)) * Wind_Magnitude;
        Forc.z = Direc.z * (1 - (Vector3.Distance(transform.position, FanPos.position) / Fan_Range)) * Wind_Magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Oil_Block")
        {
            Destroy(other.gameObject);
            FireUp();
        }

        if (other.tag == "Water_Block")
        {
            Destroy(other.gameObject);
            FireExt();
        }

        if (other.tag == "Fan")
        {
            Windy = true;
            Debug.Log("Felt it");
        }

        if(other.tag == "Wooden_Stick")
        {
            Stick_PS.gameObject.SetActive(true);

            Destroy(other.gameObject, TimeToDestroyStick);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fan")
        {
            Windy = false;
            var Forc = transform.GetChild(0).GetComponent<ParticleSystem>().forceOverLifetime;

            Forc.x = Forc.y = Forc.z = 0f;
        }
    }

    private void FireUp()
    {
        ParticleSystem Fire = transform.GetChild(0).GetComponent<ParticleSystem>();

        var emi = Fire.emission;

        emi.rateOverTime = 100f;

        var shape = Fire.shape;

        shape.angle = 15f;

        var forc = Fire.forceOverLifetime;

        forc.y = 0.8f;
    }

    private void FireExt()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
