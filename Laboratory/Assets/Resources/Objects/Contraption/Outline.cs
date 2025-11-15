using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Outline : MonoBehaviour
{
    [Header("Настройки свечения")]
    [Range(10f, 40f)]
    [SerializeField] private float flickerSpeed = 20f; //скорость мерцания
    [SerializeField] private Color outlineColor; //цвет, которым будет мерцать
    private float colorChangeDuration = 1.5f; //сколько времени будет мерцать до смены цвета на оригинальный
    private Color originalOutlineColor; //сохраняем цвет, который выставили в Inspector

    private MeshRenderer meshRenderer;
    private Material material;
    private bool isOutlining;
    public bool IsOutlining { get => isOutlining; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;

        if (outlineColor == default(Color)) outlineColor = Color.green; //стандартный цвет мерцания, если не выбрали в Inspector
    }

    private void Update()
    {
        if (!isOutlining) return;

        //PingPong применяется для плавного пульсирования
        float alpha = Mathf.PingPong(Time.time * flickerSpeed * 0.1f, 1f);
        SetAlpha(alpha);
    }

    /// <summary>
    /// Устанавливает альфа-канал текущего цвета
    /// </summary>
    private void SetAlpha(float alpha)
    {
        Color color = outlineColor;
        color.a = alpha;
        material.color = color;
    }

    /// <summary>
    /// Запустить пульсирующий контур
    /// </summary>
    public void StartOutline()
    {
        isOutlining = true;
    }

    /// <summary>
    /// Остановить пульсацию и сбросить альфу
    /// </summary>
    public void StopOutline()
    {
        isOutlining = false;
        SetAlpha(0f);
    }

    /// <summary>
    /// На короткое время делает контур красным
    /// </summary>
    public void FlashRed()
    {
        originalOutlineColor = outlineColor;
        isOutlining = true;
        outlineColor = Color.red;
        Invoke(nameof(RestoreOutlineColor), colorChangeDuration);
    }

    /// <summary>
    /// Восстанавливаем цвет выбраный в Inspector и останавливаем мерцание
    /// </summary>
    private void RestoreOutlineColor()
    {
        outlineColor = originalOutlineColor;
        StopOutline();
    }
}