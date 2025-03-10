using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LiquidSplatterController : MonoBehaviour
{
    [SerializeField] DecalProjector _pro;
    [SerializeField] ParticleSystem _par;
    private Coroutine lifetimeRoutine;
    [SerializeField] float _defaultLifetime = 5f;
    [SerializeField] Color _defaultColor = Color.magenta;

    void SetColor(Color color) {
        _pro.material.SetColor("_SplatterColor", color);

        var col = _par.colorOverLifetime;
        col.enabled = true; 

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(color, 0f),   
                new GradientColorKey(Color.black, 1f) 
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(0f, 1f) 
            }
        );

        // Apply the new gradient
        col.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    void SetLifetime(float lifetime)
    {
        if(lifetimeRoutine!=null) StopCoroutine(lifetimeRoutine);
        lifetimeRoutine = StartCoroutine(LifetimeRoutine(lifetime));
    }

    IEnumerator LifetimeRoutine(float timeToDie)
    {
        _pro.material.DOFloat(1, "_Appear", 0.1f);
        yield return new WaitForSeconds(timeToDie);
        _pro.material.DOFloat(0, "_Appear", 5f);
        _par.Stop();
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetColor(_defaultColor);
        SetLifetime(_defaultLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
