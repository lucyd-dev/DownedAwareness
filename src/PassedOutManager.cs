using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static DownedAwareness.Plugin;
using UnityEngine.UI;

namespace DownedAwareness;

public class PassedOutManager
{
    private TMP_FontAsset font = null!;
    private Material textMaterial = null!;
    private Canvas passedOutCanvas = null!;
    private GameObject template = null!;
    private readonly Dictionary<Character, GameObject> passedOutInstances = new Dictionary<Character, GameObject>();
    public bool isInitialized
    {
        get
        {
            return instance != null && passedOutCanvas != null && template != null;
        }
    }

    public PassedOutManager()
    {
        Log.LogInfo("PassedOutManager has started");
    }

    public void Initialize()
    {
        if (isInitialized)
            return;

        CreateCanvas();
        CreateTemplate();

        if (isInitialized)
            Log.LogInfo($"PassedOutManager UI initialized");
    }
    public void OnDestroy()
    {
        CleanupUI();
        if (!isInitialized)
            Log.LogInfo("PassedOutManager UI cleaned up");
    }

    private void CleanupUI()
    {
        if (passedOutCanvas != null)
        {
            Object.Destroy(passedOutCanvas.GetComponent<CanvasScaler>());
            Object.Destroy(passedOutCanvas.GetComponent<GraphicRaycaster>());
            Object.Destroy(passedOutCanvas.gameObject);
            passedOutCanvas = null!;
        }
        template = null!;
        passedOutInstances.Clear();
    }

    private void FindFont()
    {
        TextMeshProUGUI[] textComponents = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            if (textComponent.name == "InteractNameText" || textComponent.name == "InteractPromptText" || textComponent.name == "ItemPromptMain")
            {
                font = textComponent.font;
                textMaterial = textComponent.material;
                Log.LogInfo("Found game font: " + font.name);
                return;
            }
        }

        if (font == null)
            Log.LogError("Error finding game font");
    }

    private void CreateCanvas()
    {
        if (passedOutCanvas != null)
            return;

        passedOutInstances.Clear();

        GameObject guiManager = GameObject.Find("GAME/GUIManager");
        GameObject canvasObj = new GameObject("Canvas_PassedOutMarkers");
        canvasObj.transform.SetParent(guiManager.transform);
        passedOutCanvas = canvasObj.AddComponent<Canvas>();
        passedOutCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        passedOutCanvas.sortingOrder = 100;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasObj.AddComponent<GraphicRaycaster>();
        canvasObj.layer = LayerMask.NameToLayer("UI");

        Log.LogInfo("Canvas_PassedOutMarkers created");
    }
    private void CreateTemplate()
    {
        if (template != null) return;

        template = new GameObject("PassedOutMarker", typeof(RectTransform));
        template.transform.SetParent(passedOutCanvas.transform);
        template.AddComponent<CanvasRenderer>();
        template.AddComponent<PassedOut>();

        GameObject statusText = AddTMPChildren("StatusText", template.transform, new Vector2(200f, 80f));
        statusText.transform.SetParent(template.transform, true);
        var statusTextTMP = statusText.GetComponent<TextMeshProUGUI>();
        statusTextTMP.fontSize = 35f;
        statusTextTMP.outlineWidth = 0f;
        statusTextTMP.alignment = TextAlignmentOptions.Top;

        GameObject distanceText = AddTMPChildren("DistanceText", template.transform, new Vector2(200f, 60f));
        var distanceTextComp = distanceText.GetComponent<TextMeshProUGUI>();
        distanceTextComp.fontStyle = FontStyles.Bold;
        distanceTextComp.alignment = TextAlignmentOptions.Bottom;

        template.SetActive(false);

        Log.LogInfo("PassedOutMarker template created");
    }

    private GameObject AddTMPChildren(string name, Transform parent, Vector2 size)
    {
        if (font == null)
            FindFont();

        GameObject text = new GameObject(name, typeof(RectTransform));
        text.transform.SetParent(parent, true);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = size;

        var textTMP = text.AddComponent<TextMeshProUGUI>();
        textTMP.font = font;
        if (textMaterial != null)
            textTMP.material = textMaterial;
        textTMP.fontSize = 20f;
        textTMP.color = Color.white;
        textTMP.outlineWidth = 0.2f;
        textTMP.outlineColor = Color.black;
        textTMP.enableAutoSizing = false;

        Shadow shadow = text.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.95f);
        shadow.effectDistance = new Vector2(2f, -2f);

        return text;
    }

    public void CreateInstance(Character character)
    {
        if (passedOutCanvas == null || template == null || character.name == Character.localCharacter.name)
            return;

        UpdateInstance(character);

        GameObject newInstance = Object.Instantiate(template, passedOutCanvas.transform);

        var passedOutComponent = newInstance.GetComponent<PassedOut>();
        passedOutComponent.position = character.TorsoPos();
        passedOutComponent.character = character;

        newInstance.SetActive(true);
        passedOutInstances[character] = newInstance;
    }

    public void UpdateInstance(Character character, bool isDead = false)
    {
        if (passedOutInstances.TryGetValue(character, out var instance) && instance != null)
        {
            if (isDead)
            {
                instance.GetComponent<PassedOut>().hasDied();
                RemoveInstance(instance, BoundConfig.General.deadTimer.Value);
                return;
            }

            RemoveInstance(instance);
        }
    }

    private void RemoveInstance(GameObject instance, float delay = 0f)
    {
        Object.Destroy(instance, delay);
    }
}
