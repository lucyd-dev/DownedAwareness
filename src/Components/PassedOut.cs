using UnityEngine;
using TMPro;
using static DownedAwareness.Plugin;

namespace DownedAwareness;

public class PassedOut : MonoBehaviour
{
    public Character? character;
    private TextMeshProUGUI distanceText = null!;
    private TextMeshProUGUI statusText = null!;
    private bool isDead = false;

    void Start()
    {
        distanceText = transform.Find("DistanceText").GetComponentInChildren<TextMeshProUGUI>();
        statusText = transform.Find("StatusText").GetComponentInChildren<TextMeshProUGUI>();

        statusText.text = $"(@_@;)";

    }

    void LateUpdate()
    {
        if (Camera.main == null || character == null)
            return;

        Vector3 position = character.TorsoPos();

        transform.position = Camera.main.WorldToScreenPoint(position);
        statusText.color = character.refs.customization.PlayerColor;

        float distance = Mathf.Round(Vector3.Distance(position, Camera.main.transform.position));
        int percLeft = 100 - Mathf.CeilToInt(character?.data.deathTimer * 100f ?? 0f);
        string percText = !isDead ? $"{percLeft}%" : "DEAD";
        distanceText.text = $"{distance}m | {percText}";

        float angle = Vector3.Angle(Camera.main.transform.forward, position - Camera.main.transform.position);
        bool visible = angle < 90f;
        statusText.gameObject.SetActive(visible);
        distanceText.gameObject.SetActive(visible);
    }

    public void hasDied()
    {
        if (statusText == null || distanceText == null) return;

        statusText.text = $"(x_x;)";
        isDead = true;
    }
}
