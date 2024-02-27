using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Player Stats")]
    public int stamina;
    [SerializeField] private int stamMax;
    public bool b_equiping { get; private set; }
    private bool b_isCharge;
    private float equip_time;

    public ItemSlot equippedSlot;
    public int equippedSlotIndex;

    [Header("Components")]
    private PlayerInput playerInput;
    private Camera cam;
    [SerializeField] Camera[] auxileryCams;
    private Rigidbody rig;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Animator cameraAnim;
    public Inventory inv;
    [SerializeField] CraftingManager craftingUI;
    private UIManager uiManager;
    private HUDManager hudManager;

    [Header("Controller Components")]
    private InteractHandler h_interact;
    private HeathHandler h_health;
    private CraftingHandler h_craft;
    private HungerHandler h_hunger;

    public ItemSlot[] toolSlots;
    [SerializeField] private Transform muzzle;
    private GameObject equippedObject;
    [Header("Mouse")]
    [SerializeField]
    private float mouseSensitivity = 100f;
    float xRotation = 0f;
    public bool mouseLock = false;

    [Header("Movement")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float walk = 8f;
    [SerializeField] private float jumpForce;
    bool sprinting;
    bool walking;


    public int health
    {
        get
        {
            return h_health.health;
        }
    }

    public int hunger
    {
        get
        {
            return h_hunger.hunger;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        findComponents();
        InitAuxCams();
    }
    private void findComponents()
    {
        cam = GetComponentInChildren<Camera>();
        playerInput = GetComponent<PlayerInput>();
        rig = GetComponent<Rigidbody>();
        cameraAnim = cam.GetComponent<Animator>();
        inv = GetComponent<Inventory>();
        h_interact = GetComponent<InteractHandler>();
        h_health = GetComponent<HeathHandler>();
        h_hunger = GetComponent<HungerHandler>();
        equippedSlot = toolSlots[0];
        h_craft = GetComponent<CraftingHandler>();
        uiManager = FindObjectOfType<UIManager>();
        hudManager = FindObjectOfType<HUDManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void InitAuxCams()
    {
        foreach (Camera cam in auxileryCams)
        {
            cam.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseLock)
        {
            mouseInput();
            movement();

        }

        equippingUpdate();
       
    }


    private void equippingUpdate()
    {
        if (b_equiping)
        {
            if (equippedObject)
            {
                if (equippedObject.TryGetComponent<EquipableHandler>(out EquipableHandler h_equip))
                {
                    if (b_isCharge)
                    {
                        equip_time += Time.deltaTime;
                        if (equip_time >= h_equip.cooldown)
                        {
                            h_equip.EventInvoke();
                        }
                    }
                    else
                    {
                        equip_time -= Time.deltaTime;
                        if (equip_time <= 0)
                        {
                            h_equip.EventInvoke();
                        }
                    }

                    hudManager.progressUpdate(equip_time, h_equip.cooldown);

                }
                else
                {
                    b_equiping = false;
                    b_isCharge = false;
                    equip_time = 0;
                }
            }
            else
            {
                b_equiping = false;
                b_isCharge = false;
                equip_time = 0;
            }

        }
    }


    private void movement()
    {
       

        float moveSpeed;
        moveSpeed = walk;

        if (sprinting && stamina > 0)
        {
            stamina -= 1;
            moveSpeed = speed;

        }
       
     
        Vector2 movementV = playerInput.actions["Move"].ReadValue<Vector2>();
        if (movementV.y != 0)
        {
            walking = true;

            cameraAnim.SetBool("is_walking", true);
        }
        else
        {
            walking = false;

            cameraAnim.SetBool("is_walking", false);

        }
        Vector3 move = transform.TransformDirection(new Vector3(movementV.x, 0f, movementV.y) * moveSpeed);


        rig.velocity = new Vector3(move.x, rig.velocity.y, move.z);

        if (walking == false && stamina < stamMax && hunger > 0)
        {
            stamina += 1;
            h_hunger.stamTaken += 1;
        }


    }

    public void GainHunger(int amount)
    {
        h_hunger.GainHunger(amount);
    }

    private void mouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }

    public void jump()
    {
        if (IsGrounded())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }




    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.01f,groundLayer);
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    { 
        jump();
    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprinting = true;
            cameraAnim.SetBool("is_running", true);

        }
        else if (context.canceled)
        {
            sprinting = false;
            cameraAnim.SetBool("is_running", false);
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        inv.toggleInventory();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        h_interact.HandleInteract();
    }

    public void OnScrollInput(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            equippedSlotIndex++;
        }
        else if(context.ReadValue<float>() < 0)
        {
            equippedSlotIndex--;
        }
        if(equippedSlotIndex < 0)
        {
            equippedSlotIndex = toolSlots.Length-1;
        }else if(equippedSlotIndex > toolSlots.Length - 1)
        {
            equippedSlotIndex = 0;
        }

        EquipItem(toolSlots[equippedSlotIndex]);

    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (mouseLock) return;

        if(equippedObject != null)
        {
            if(equippedObject.TryGetComponent(out EquipableHandler h_equip))
            {
                if (h_equip.cooldown > 0)
                {
                    b_isCharge = h_equip.isCharge;
                    b_equiping = true;
                    equip_time = 0;
                    if (!b_isCharge)
                    {
                        equip_time = h_equip.cooldown;
                    }
                }
                else
                {
                    h_equip.EventInvoke();
                }
            }
        }
    }

    public void OnCraftInput(InputAction.CallbackContext context)
    {
        h_craft.toggleCraft();
    }

    public void UpdateToolSlots()
    {
       
        foreach(ItemSlot itemSlot in toolSlots)
        {
            if(equippedSlot == itemSlot)
            {
                EquipItem(itemSlot);   
            }
            
        }
    }


    public void EquipItem(ItemSlot itemSlot)
    {
        equip_time = 0;
        b_equiping = false;
        b_isCharge = false;

        Destroy(equippedObject);
        equippedSlot = itemSlot;   
        if (itemSlot.itemInSlot)
        {
            if (itemSlot.itemInSlot.itemData.equipable)
            {
                equippedObject = Instantiate(itemSlot.itemInSlot.itemData.equipmentPrefab, muzzle);
                equippedObject.GetComponent<EquipableHandler>().invItem = itemSlot.itemInSlot;
            }
        }
    }
    public void EquipItem(int index)
    {
        equip_time = 0;
        b_equiping = false;
        b_isCharge = false;


        ItemSlot itemSlot = toolSlots[index];
        equippedSlotIndex = index;
        Destroy(equippedObject);
        equippedSlot = itemSlot;
        if (itemSlot.itemInSlot)
        {
            if (itemSlot.itemInSlot.itemData.equipable)
            {
                equippedObject = Instantiate(itemSlot.itemInSlot.itemData.equipmentPrefab, muzzle);
                equippedObject.GetComponent<EquipableHandler>().invItem = itemSlot.itemInSlot;
            }
        }
    }

    

}
