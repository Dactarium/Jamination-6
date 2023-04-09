using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Managers;

public class Music : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip maviMusic;
    [SerializeField]
    private AudioClip kirmiziMusic;
    [SerializeField]
    private AudioClip yesilMusic;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        source.clip = kirmiziMusic;
        source.Play();
        source.loop = true;
        GameManager.Instance.DimensionController.OnDimensionChange += OnDimensionsChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDimensionsChange(Dimension previous, Dimension next)
    {
        source.Stop();
        switch (next)
        {
            case Dimension.Red:
                source.clip = kirmiziMusic;
                source.Play();
                break;
            case Dimension.Blue:
                source.clip = maviMusic;
                source.Play();
                break;
            case Dimension.Green:
                source.clip = yesilMusic;
                source.Play();
                break;
        }
    }
}
