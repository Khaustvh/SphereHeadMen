public sealed class SaveData
{
    public byte NumberOfLevel;
    
    public short AllValueOfCoins;

    public short HitPointsEnemyAverageValue;
    public short CoinsFromTheEnemyAverageValue;
    public short HitPointsBossAverageValue;
    public short CoinsFromTheBossAverageValue;
    
    public float AttackSpeed;
    public byte Damage;
    public float ProjectileDistance;
    public short ProjectileSpeed;
    
    public byte AttackSpeedLevel;
    public byte DamageLevel;
    public byte ProjectileDistanceLevel;
    public byte ProjectileSpeedLevel;
    
    public short AttackSpeedCost;
    public short DamageCost;
    public short ProjectileDistanceCost;
    public short ProjectileSpeedCost;

    public SaveData()
    {
        NumberOfLevel = 1;

        HitPointsEnemyAverageValue = 5;
        CoinsFromTheEnemyAverageValue = 5;
        HitPointsBossAverageValue = 10;
        CoinsFromTheBossAverageValue = 10;
            
        AllValueOfCoins = 0;
        
        AttackSpeed = 1f;
        Damage = 1;
        ProjectileDistance = 1f;
        ProjectileSpeed = 300;
        
        AttackSpeedLevel = 1;
        DamageLevel = 1;
        ProjectileDistanceLevel = 1;
        ProjectileSpeedLevel = 1;
        
        AttackSpeedCost = 25;
        DamageCost = 35;
        ProjectileDistanceCost = 35;
        ProjectileSpeedCost = 40;
    }
}
