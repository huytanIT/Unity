using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 8f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 3f;
    [SerializeField]
    TextMeshProUGUI myText;
    public Canvas winCanvas;
    public Canvas loseCanvas;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Transform cameraTransform;
    private bool gameEnded;
    public Material[] material;
    Renderer rend;
    private int point;
    private void Start()
    {
        gameEnded = false;
        int randomIndex = Random.Range(0, material.Length);
        rend = GetComponent<Renderer>();
        point = 0;
        myText.text = "Point: " + point.ToString();
        rend.enabled = true;
        rend.sharedMaterial = material[randomIndex];
        cameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

    }

    void Update()
    {
        myText.text = "Point: " + point.ToString();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        if(!gameEnded && point == 10)
        {
            Debug.Log("You win!");
            Time.timeScale = 0f;
            winCanvas.enabled = true;
            gameEnded = true;
            rotationSpeed = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Renderer>().sharedMaterial.name 
            == rend.sharedMaterial.name)
        {
            int randomInt = Random.Range(0, material.Length);
            rend.sharedMaterial = material[randomInt];
            Destroy(other.gameObject);
            point++;
        }
        else
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0f;
            loseCanvas.enabled = true;
            Destroy(other.gameObject);
            gameEnded = true;
            rotationSpeed = 0f;
        }

    }
}