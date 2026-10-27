using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI keyText;

    public TextMeshProUGUI ammoText;

    public AmmoManager ammoManager;
    public PlayerHealth playerHealth;
    public WeaponManager weaponManager;

    public GameObject levelCompleteObject;
    public GameObject levelFailedObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        AmmoManager.OnAmmoChange += RefreshAmmoUI;
        PlayerHealth.OnHealthChange += RefreshHealthUI;
        WeaponManager.OnWeaponChanged += RefreshAmmoUI;
    }

    private void OnDisable()
    {
        AmmoManager.OnAmmoChange -= RefreshAmmoUI;
        PlayerHealth.OnHealthChange -= RefreshHealthUI;
        WeaponManager.OnWeaponChanged -= RefreshAmmoUI;
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth.maxHealth;

        RefreshAmmoUI();
        RefreshHealthUI();
    }

    void RefreshHealthUI()
    {
        healthSlider.value = playerHealth.currentHealth;
        healthText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;
    }

    void RefreshAmmoUI()
    {   
        if (weaponManager.currentWeaponIndex != 9)
        {
            AmmoType currentType = weaponManager.currentGun.ammoType;
            ammoText.text = $"{currentType}: {ammoManager.GetAmmoCount(currentType)}";
        }
    }

    public void RefreshObjectiveUI(string newObjectiveText, Color textColor)
    {
        objectiveText.text = newObjectiveText;
        objectiveText.color = textColor;
    }

    public void RefreshKeysUI(int keys)
    {
        keyText.text = "x " + keys.ToString();
    }

    public void LevelCompleteScreen()
    {
        levelCompleteObject.SetActive(true);
    }

    public void LevelFailedScreen()
    {

    }
}
