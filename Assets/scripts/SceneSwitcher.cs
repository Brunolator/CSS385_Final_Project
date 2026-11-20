using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName;
    Collider2D collider;
    Camera cam;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        cam = Camera.main;
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        Vector3 coord = getMouseCoordinates();
        // if mouse was just clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // select object that mouse is on
            Physics2D.SyncTransforms();
            Collider2D hit = Physics2D.OverlapPoint(coord);
            // set object as true and dragging
            if (hit == collider)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    Vector3 getMouseCoordinates()
    {
        Vector3 coord = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // prevents vector from going off coordinates
        coord.z = 0;
        return coord;
    }
}
