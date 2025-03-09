using System;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject leftRobot;
    [SerializeField] GameObject rightRobot;

    [SerializeField] private Texture2D cursorTexture;
    private Vector2 hotspot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedRobot;

            if (mousePos.x < 0)
            {
                selectedRobot = leftRobot;
            }
            else
            {
                selectedRobot = rightRobot;
            }

            Vector2 robotPos = selectedRobot.transform.position;
            Vector2 direction = (mousePos - robotPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Instantiate(bulletPrefab, robotPos, Quaternion.Euler(0, 0, angle));
        }
    }
}
