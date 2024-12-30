// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     private PlayerMovement playerMovement;
//     private PlayerSpriteRenderer playerSpriteRenderer;

//     [SerializeField] private Sprite idleSprite;
//     [SerializeField] private Sprite jumpSprite;
//     [SerializeField] private Sprite slideSprite;
//     [SerializeField] private AnimatedSprite runAnimation;

//     private void Awake()
//     {
//         playerMovement = new PlayerMovement(
//             Camera.main,
//             GetComponent<Rigidbody2D>(),
//             GetComponent<Collider2D>()
//         );

//         playerSpriteRenderer = new PlayerSpriteRenderer(
//             playerMovement,
//             GetComponent<SpriteRenderer>(),
//             runAnimation
//         )
//         {
//             IdleSprite = idleSprite,
//             JumpSprite = jumpSprite,
//             SlideSprite = slideSprite
//         };
//     }

//     private void OnEnable()
//     {
//         playerMovement.Enable();
//         playerSpriteRenderer.Enable();
//     }

//     private void OnDisable()
//     {
//         playerMovement.Disable();
//         playerSpriteRenderer.Disable();
//     }

//     private void Update()
//     {
//         playerMovement.UpdateMovement();
//         playerSpriteRenderer.UpdateSprite();
//     }

//     private void FixedUpdate()
//     {
//         playerMovement.FixedUpdateMovement();
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         playerMovement.HandleCollision(collision);
//     }
// }
