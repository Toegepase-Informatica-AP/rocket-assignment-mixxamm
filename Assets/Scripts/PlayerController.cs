using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioSource, audioSourceLose;
    Vector2 movementVector;
    private float up;
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;
    public float speed = 0;
    private Rigidbody rb;
    private int points = 0;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        up = 0.0f;
        // store initial position as respawn location
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
    }

    void OnJump()
    {
        up += 10.0f;
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(movementVector.x, up, movementVector.y) * speed);
        up = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "nextLevel")
        {
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        } else if (other.tag == "dead")
        {
            transform.position = respawnPosition;
            transform.rotation = respawnRotation;
        } else if (other.tag == "pickup")
        {
            other.gameObject.SetActive(false);
            points++;
            audioSource.Play();
            if (points < 3)
                text.text = points + "";
            else
                text.text = "U heeft gewonnen!";
        }
        else if (other.tag == "enemy" && points < 3)
        {
            points--;
            audioSourceLose.Play();
            text.text = points + "";
        }
    }
}
