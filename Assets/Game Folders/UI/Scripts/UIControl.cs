using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class UIControl : MonoBehaviour
{
    [Header("UI Info For Main Menu")] 
    [SerializeField] private Text[] attackSpeed;
    [SerializeField] private Text[] damage;
    [SerializeField] private Text[] projectileDistance;
    [SerializeField] private Text[] projectileSpeed;
    [SerializeField] private Text attackSpeedCost;
    [SerializeField] private Text damageCost;
    [SerializeField] private Text projectileDistanceCost;
    [SerializeField] private Text projectileSpeedCost;
    [SerializeField] private Text numberOfCoins;
    
    private readonly string _dataKey = "PlayerStatistics";

    private SaveData _data;
    
    private void Start()
    {
        if (gameObject.name == "CanvasMenu")
        {
            LoadData();

            // _data = new SaveData();
            // SaveData();
            // LoadData();
        }
    }

    void LoadData()
    {
        _data = SaveManager.Load<SaveData>(_dataKey);

        for (byte i = 0; i < 2; i++)
        {
            attackSpeed[i].text = _data.AttackSpeedLevel.ToString();
            damage[i].text = _data.DamageLevel.ToString();
            projectileDistance[i].text = _data.ProjectileDistanceLevel.ToString();
            projectileSpeed[i].text = _data.ProjectileSpeedLevel.ToString();
        }

        attackSpeedCost.text = _data.AttackSpeedCost.ToString();
        damageCost.text = _data.DamageCost.ToString();
        projectileDistanceCost.text = _data.ProjectileDistanceCost.ToString();
        projectileSpeedCost.text = _data.ProjectileSpeedCost.ToString();
        numberOfCoins.text = _data.AllValueOfCoins.ToString();
    }

    void SaveData()
    {
        SaveManager.Save(_dataKey, _data);
    }

    public void UpAttackSpeed()
    {
        if (_data.AllValueOfCoins - _data.AttackSpeedCost >= 0)
        {
            _data.AllValueOfCoins = (short)(_data.AllValueOfCoins - _data.AttackSpeedCost);
            _data.AttackSpeedLevel++;
            _data.AttackSpeedCost *= 2;
            _data.AttackSpeed -= 0.05f;

            SaveData();
            LoadData();
        }
    }
    
    public void UpDamage()
    {
        if (_data.AllValueOfCoins - _data.DamageCost >= 0)
        {
            _data.AllValueOfCoins = (short)(_data.AllValueOfCoins - _data.DamageCost);
            _data.DamageLevel++;
            _data.DamageCost *= 2;
            _data.Damage++;

            SaveData();
            LoadData();
        }
    }
    
    public void UpProjectileSpeed()
    {
        if (_data.AllValueOfCoins - _data.ProjectileSpeedCost >= 0)
        {
            _data.AllValueOfCoins = (short)(_data.AllValueOfCoins - _data.ProjectileSpeedCost);
            _data.ProjectileSpeedLevel++;
            _data.ProjectileSpeedCost *= 2;
            _data.ProjectileSpeed += 100;

            SaveData();
            LoadData();
        }
    }
    
    public void UpProjectileDistance()
    {
        if (_data.AllValueOfCoins - _data.ProjectileDistanceCost >= 0)
        {
            _data.AllValueOfCoins = (short)(_data.AllValueOfCoins - _data.ProjectileDistanceCost);
            _data.ProjectileDistanceLevel++;
            _data.ProjectileDistanceCost *= 2;
            _data.ProjectileDistance += 0.1f;

            SaveData();
            LoadData();
        }
    }
    
    public static void StartLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
