using System;
using System.Collections;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace AngryBritishBug
{
    [BepInPlugin(PluginMetadata.GUID, PluginMetadata.NAME, PluginMetadata.VERSION)]
    [BepInProcess("Lethal Company.exe")]
    public class AngryBritishBugPlugin : BaseUnityPlugin
    {
        internal static AngryBritishBugPlugin Instance;
        internal new static ManualLogSource Logger;

        private static bool _messageShown;
        private GameObject _textObject;
        private TextMeshProUGUI _textComponent;

        private void Awake()
        {
            Instance = this;
            Logger = base.Logger;
            new Harmony(PluginMetadata.GUID).PatchAll();
            Logger.LogInfo($"Angry British Bug v{PluginMetadata.VERSION} loaded.");
        }

        internal void ShowStartupMessage()
        {
            if (_messageShown) return;
            _messageShown = true;
            StartCoroutine(ShowMessageCoroutine());
        }

        private IEnumerator ShowMessageCoroutine()
        {
            if (_textObject == null)
            {
                if (!CreateMessageUI())
                    yield break;
            }

            _textComponent.text = "The bugs are angry and British!";
            _textObject.SetActive(true);

            yield return new WaitForSeconds(5f);

            _textObject.SetActive(false);
        }

        private bool CreateMessageUI()
        {
            GameObject canvasObj = GameObject.Find("Systems/UI/Canvas");
            if (canvasObj == null)
            {
                Logger.LogWarning("Could not find game Canvas; startup message will not show.");
                return false;
            }

            _textObject = new GameObject("AngryBritishBug_StartupMessage");
            _textObject.SetActive(false);

            RectTransform rect = _textObject.AddComponent<RectTransform>();
            rect.SetParent(canvasObj.transform);
            rect.SetAsLastSibling();
            _textObject.layer = LayerMask.NameToLayer("UI");

            rect.anchoredPosition3D = new Vector3(0f, -180f, 0f);
            rect.localScale = Vector3.one;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(600f, 80f);

            _textComponent = _textObject.AddComponent<TextMeshProUGUI>();

            TMP_FontAsset font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                .FirstOrDefault(f => f.name == "b");
            if (font == null)
            {
                Logger.LogWarning("Could not find game font; startup message may not display correctly.");
            }
            else
            {
                _textComponent.font = font;
                if (font.material != null)
                    _textComponent.fontMaterial = font.material;
            }

            _textComponent.color = new Color(0.65f, 0.96f, 1f, 0.9f);
            _textComponent.fontSize = 22f;
            _textComponent.alignment = TextAlignmentOptions.Center;
            _textComponent.enableWordWrapping = true;

            return true;
        }
    }

    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void OnRoundStart(StartOfRound __instance)
        {
            if (AngryBritishBugPlugin.Instance != null)
                AngryBritishBugPlugin.Instance.ShowStartupMessage();
        }
    }

    internal static class PluginMetadata
    {
        public const string GUID = "HoffmanTV.AngryBritishBug";
        public const string NAME = "Angry British Bug";
        public const string VERSION = "1.6.0";
    }
}
