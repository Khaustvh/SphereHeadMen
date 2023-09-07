public sealed class SaveData
{
    public byte NumberOfLevel;
    
    public int AllValueOfCoins;

    public int HitPointsEnemyAverageValue;
    public int CoinsFromTheEnemyAverageValue;
    public int HitPointsBossAverageValue;
    public int CoinsFromTheBossAverageValue;
    
    public float AttackSpeed;
    public byte Damage;
    public float ProjectileDistance;
    public int ProjectileSpeed;
    
    public byte AttackSpeedLevel;
    public byte DamageLevel;
    public byte ProjectileDistanceLevel;
    public byte ProjectileSpeedLevel;
    
    public int AttackSpeedCost;
    public int DamageCost;
    public int ProjectileDistanceCost;
    public int ProjectileSpeedCost;

    public SaveData()
    {
        NumberOfLevel = 1;

        HitPointsEnemyAverageValue = 5;
        CoinsFromTheEnemyAverageValue = 5;
        HitPointsBossAverageValue = 30;
        CoinsFromTheBossAverageValue = 50;
            
        AllValueOfCoins = 0;
        
        AttackSpeed = 0.7f;
        Damage = 1;
        ProjectileDistance = 1f;
        ProjectileSpeed = 300;
        
        AttackSpeedLevel = 1;
        DamageLevel = 1;
        ProjectileDistanceLevel = 1;
        ProjectileSpeedLevel = 1;
        
        AttackSpeedCost = 25;
        DamageCost = 40;
        ProjectileDistanceCost = 35;
        ProjectileSpeedCost = 30;
    }
}
