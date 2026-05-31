using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Graphics - FPS")]
    public TMP_Dropdown fpsDropdown;
    public Toggle vsyncToggle;

    [Header("Display Mode")]
    public TMP_Dropdown displayDropdown;

    [Header("Monitor")]
    public TMP_Dropdown monitorDropdown;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;

    private List<int> fpsOptions = new List<int> { 30, 45, 60, 90, 120 };

    private Resolution[] resolutions;
    private int currentMonitor = 0;

    void Start()
    {
        // ================= AUDIO =================
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // ================= GRAPHICS =================
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 0) == 1;

        SetupFPSDropdown();
        SetupDisplayDropdown();
        SetupMonitorDropdown();
        SetupResolutionDropdown();

        LoadFPSSettings();
        LoadDisplaySettings();

        fpsDropdown.onValueChanged.AddListener(SetFPS);
        vsyncToggle.onValueChanged.AddListener(SetVSync);
        displayDropdown.onValueChanged.AddListener(SetDisplayMode);
        monitorDropdown.onValueChanged.AddListener(SetMonitor);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // ================= AUDIO =================
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        AudioManager.Instance.SetSFXVolume(volume);
    }

    // ================= FPS =================
    void AddRefreshRateOption()
    {
        int hz = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);

        if (!fpsOptions.Contains(hz))
            fpsOptions.Add(hz);

        fpsOptions.Sort();
    }

    void SetupFPSDropdown()
    {
        fpsDropdown.ClearOptions();

        AddRefreshRateOption();

        List<string> options = new List<string>();

        foreach (int fps in fpsOptions)
            options.Add(fps + " FPS");

        options.Add("Unlimited");

        fpsDropdown.AddOptions(options);
    }

    void LoadFPSSettings()
    {
        bool vSync = PlayerPrefs.GetInt("VSync", 0) == 1;
        vsyncToggle.isOn = vSync;
        fpsDropdown.interactable = !vSync;

        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
            return;
        }

        int savedFPS = PlayerPrefs.GetInt("TargetFPS", 60);

        int index = fpsOptions.IndexOf(savedFPS);
        if (index < 0) index = fpsOptions.IndexOf(60);
        if (index < 0) index = 0;

        fpsDropdown.value = index;
        ApplyFPS(index);
    }

    public void SetFPS(int index)
    {
        if (vsyncToggle.isOn) return;

        PlayerPrefs.SetInt("TargetFPS", fpsOptions[index]);
        ApplyFPS(index);
    }

    void ApplyFPS(int index)
    {
        if (index >= fpsOptions.Count)
        {
            Application.targetFrameRate = -1;
            return;
        }

        Application.targetFrameRate = fpsOptions[index];
    }

    // ================= VSYNC =================
    public void SetVSync(bool enabled)
    {
        PlayerPrefs.SetInt("VSync", enabled ? 1 : 0);

        fpsDropdown.interactable = !enabled;

        if (enabled)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;

            int savedFPS = PlayerPrefs.GetInt("TargetFPS", 60);

            int index = fpsOptions.IndexOf(savedFPS);
            if (index < 0) index = fpsOptions.IndexOf(60);
            if (index < 0) index = 0;

            ApplyFPS(index);
        }
    }

    // ================= DISPLAY MODE =================
    void SetupDisplayDropdown()
    {
        displayDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "Windowed",
            "Fullscreen",
            "Borderless"
        };

        displayDropdown.AddOptions(options);
    }

    void LoadDisplaySettings()
    {
        int saved = PlayerPrefs.GetInt("DisplayMode", 2);
        displayDropdown.value = saved;
        ApplyDisplay(saved);
    }

    public void SetDisplayMode(int index)
    {
        PlayerPrefs.SetInt("DisplayMode", index);
        ApplyDisplay(index);
    }

    void ApplyDisplay(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    // ================= MONITOR =================
    void SetupMonitorDropdown()
    {
        monitorDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < Display.displays.Length; i++)
            options.Add("Monitor " + (i + 1));

        monitorDropdown.AddOptions(options);

        currentMonitor = PlayerPrefs.GetInt("MonitorIndex", 0);
        currentMonitor = Mathf.Clamp(currentMonitor, 0, Display.displays.Length - 1);

        monitorDropdown.value = currentMonitor;
    }

    public void SetMonitor(int index)
    {
        currentMonitor = index;
        PlayerPrefs.SetInt("MonitorIndex", index);

        if (index < Display.displays.Length)
            Display.displays[index].Activate();
    }

    // ================= RESOLUTION =================
    void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            options.Add(res.width + " x " + res.height);
        }

        resolutionDropdown.AddOptions(options);

        int saved = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);
        saved = Mathf.Clamp(saved, 0, resolutions.Length - 1);

        resolutionDropdown.value = saved;
        ApplyResolution(saved);
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        ApplyResolution(index);
    }

    void ApplyResolution(int index)
    {
        Resolution res = resolutions[index];

        Screen.SetResolution(
            res.width,
            res.height,
            Screen.fullScreenMode
        );
    }
}