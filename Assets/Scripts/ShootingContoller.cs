using UnityEngine;

public class ShootingContoller : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GunController[] guns;
    [SerializeField] private RobotController[] robots;

    private bool powerupActive = false;
    private float powerupDuration = 5f;
    private float powerupTimer;

    // Update is called once per frame
    void Update()
    {
        if (powerupActive)
        {
            powerupTimer -= Time.deltaTime;
            
            if (powerupTimer <= 0)
            {
                powerupActive = false;
            }
        }
    }

    public void Fire(Vector2 targetPos)
    {
        int index;

        if (targetPos.x < 0)
        {
            index = 0;
        }
        else
        {
            index = 1;
        }

        GunController selectedGun = guns[index];
        RobotController selectedRobot = robots[index];

        // If the selected robot is not enabled(dead), switch to the other gun and robot
        if (!selectedRobot.IsActive())
        {
            index = 1 - index; // Flip index
            selectedGun = guns[index];
            selectedRobot = robots[index];
        }

        selectedGun.RotateGun(targetPos);
        selectedGun.PlaySound();
        selectedRobot.Flip(targetPos);

        Vector2 gunPos = selectedGun.transform.position;
        Vector2 direction = (targetPos - gunPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!powerupActive)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<PlayerBulletController>().SetTarget(targetPos);
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<PlayerBulletController>().SetTarget(targetPos);

            float distance = Vector2.Distance(gunPos, targetPos);
                    
            // Left bullet (-15 degrees)
            Vector2 leftDir = RotateVector(direction, -15f);
            Vector2 leftTarget = gunPos + (leftDir * distance);
            GameObject leftBullet = Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle - 15f));
            leftBullet.GetComponent<PlayerBulletController>().SetTarget(leftTarget);

            // Right bullet (+15 degrees)
            Vector2 rightDir = RotateVector(direction, 15f);
            Vector2 rightTarget = gunPos + (rightDir * distance);
            GameObject rightBullet = Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle + 15f));
            rightBullet.GetComponent<PlayerBulletController>().SetTarget(rightTarget);
        }
    }

    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        Vector2 newVector = new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);

        return newVector;
    }

    public void ActivatePowerup()
    {
        powerupActive = true;
        powerupTimer = powerupDuration;
    }
}
