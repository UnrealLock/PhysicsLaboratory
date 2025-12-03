using System;
using UnityEngine;

public class EmitElectron : MonoBehaviour
{
    [SerializeField] private ParticleSystem PSCathodeToGrid;
    [SerializeField] private ParticleSystem PSGridToAnod;
    [SerializeField] private OnSwitchButtonScript switchButtonScript;
    [SerializeField] private ContraptionZoneData data;
    [SerializeField] private Material emissionMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject anod;
    [SerializeField] private GameObject spiral;

    private Renderer anodRenderer;
    private Renderer spiralRenderer;

    private ParticleSystem.EmissionModule EMGridToAnod;
    private ParticleSystem.EmissionModule EMCathodeToGrid;

    public float minEmission = 400f;
    public float maxEmission = 1000f;

    private void Start()
    {
        anodRenderer = anod.GetComponent<Renderer>();
        spiralRenderer = spiral.GetComponent<Renderer>();

        anodRenderer.material = defaultMaterial;
        spiralRenderer.material = defaultMaterial;

        EMGridToAnod = PSGridToAnod.emission;
        EMCathodeToGrid = PSCathodeToGrid.emission;
    }

    void Update()
    {
        //if (!lampCamera.isActiveAndEnabled)
        //{
        //    lamp.gameObject.SetActive(false);
        //    return;
        //}
        //else
        //{
        //    lamp.gameObject.SetActive(true);
        //}

        if (switchButtonScript.IsActivated)
        {
            EMCathodeToGrid.rateOverTime = 1000f;
        }
        else
        {
            EMCathodeToGrid.rateOverTime = 0f;
        }

        if (data.Voltage > 0 && switchButtonScript.IsActivated)
        {
            anodRenderer.material = emissionMaterial;
            spiralRenderer.material = emissionMaterial;

            UpdateEmission();
        }
        else
        {
            anodRenderer.material = defaultMaterial;
            spiralRenderer.material = defaultMaterial;
            EMGridToAnod.rateOverTime = 0;
        }
    }

    private void UpdateEmission()
    {
        float raw = (float)data.Voltage;
        float voltage = Mathf.Clamp(raw, 0f, 0.46f);
        float factor = voltage / 0.46f;

        float emissionValue = Mathf.Lerp(maxEmission, minEmission, factor);
        EMGridToAnod.rateOverTime = emissionValue;
    }
}