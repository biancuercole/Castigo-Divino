using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnProximity : MonoBehaviour
{
    [SerializeField] private Transform player;  
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private float maxDistance = 10f;  
    [SerializeField] private float minDistance = 2f;   

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        
        audioSource.playOnAwake = false;
        audioSource.loop = true; 
    }

    private void Update()
    {
       
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

      
        if (distanceToPlayer <= maxDistance)
        {
         
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

           
            float volume = Mathf.InverseLerp(maxDistance, minDistance, distanceToPlayer);
            audioSource.volume = volume;
        }
        else
        {
           
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
