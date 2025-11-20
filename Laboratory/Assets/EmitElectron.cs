using System;
using UnityEngine;

public class EmitElectron : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private ParticleSystem particleSystem2;
    [SerializeField] private GameObject lamp;
    [SerializeField] private OnSwitchButtonScript switchButtonScript;
    [SerializeField] private ContraptionZoneData data;
    [SerializeField] private Material emissionMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Camera lampCamera;
    [SerializeField] private GameObject anod;
    [SerializeField] private GameObject spiral;

    private Renderer anodRenderer;
    private Renderer spiralRenderer;

    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.EmissionModule emissionModule2;

    public float minEmission = 40f;
    public float maxEmission = 200f;

    private void Start()
    {
        anodRenderer = anod.GetComponent<Renderer>();
        spiralRenderer = spiral.GetComponent<Renderer>();

        anodRenderer.material = defaultMaterial;
        spiralRenderer.material = defaultMaterial;

        emissionModule = particleSystem.emission;
        emissionModule2 = particleSystem2.emission;
    }

    void Update()
    {
        if (!lampCamera.isActiveAndEnabled)
        {
            lamp.gameObject.SetActive(false);
            return;
        }
        else
        {
            lamp.gameObject.SetActive(true);
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
            emissionModule.rateOverTime = 0;
            emissionModule2.rateOverTime = 0;
        }
    }

    private void UpdateEmission()
    {
        float raw = (float)data.Voltage;
        float voltage = Mathf.Clamp(raw, 0f, 0.46f);
        float factor = voltage / 0.46f;

        float emissionValue = Mathf.Lerp(maxEmission, minEmission, factor);
        emissionModule.rateOverTime = emissionValue;
        emissionModule2.rateOverTime = emissionValue;
    }
}