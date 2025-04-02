using UnityEngine;

public class ShootingContoller : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GunController[] guns;
    [SerializeField] private RobotController[] robots;

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

        // If the selected robot is null, switch to the other gun and robot
        if (selectedRobot == null)
        {
            index = 1 - index; // Flip index
            selectedGun = guns[index];
            selectedRobot = robots[index];
        }

        Vector2 gunPos = selectedGun.transform.position;
        Vector2 direction = (targetPos - gunPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        selectedGun.RotateGun(targetPos);
        selectedGun.PlaySound();
        selectedRobot.Flip(targetPos);
        Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle));
    }
}
