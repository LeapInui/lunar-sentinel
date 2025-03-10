using System;
using UnityEngine;

public class ShootingContoller : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GunController leftGun;
    [SerializeField] private GunController rightGun;
    [SerializeField] private RobotController leftRobot;
    [SerializeField] private RobotController rightRobot;

    public void Fire(Vector2 targetPos)
    {
        GunController selectedGun;
        RobotController selectedRobot;

        if (targetPos.x < 0)
        {
            selectedGun = leftGun;
            selectedRobot = leftRobot;
        }
        else
        {
            selectedGun = rightGun;
            selectedRobot = rightRobot;
        }

        Vector2 gunPos = selectedGun.transform.position;
        Vector2 direction = (targetPos - gunPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        selectedGun.RotateGun(targetPos);
        selectedRobot.Flip(targetPos);
        Instantiate(bulletPrefab, gunPos, Quaternion.Euler(0, 0, angle));
    }
}
