using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip applePick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound(AudioClip sfx)
    {
        source.clip = sfx;
    }
}
