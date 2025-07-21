using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
//Warning: this is not the ability, this is the actual doppelganger gameobject
public class Doppelganger : MonoBehaviour
{
    private Player player; // Il player da seguire
    public float followSpeed = 0.1f; // La velocità con cui seguire il player
    private Animator animator;
    private WeaponTopHierarchy weaponGo;
    private ProjectileWeapon weapon;
    //private GameObject weaponsHolder;
    private PlayerAim playerAim;
    private void Awake() {
        //player copy initialization
        player = GameManager.instance.player;
        playerAim = player.GetComponent<PlayerAim>();
        GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteLibrary>().spriteLibraryAsset = player.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        animator = GetComponent<Animator>();
        //weapon copy initialization
        //weaponsHolder = transform.Find("Weapons").gameObject;
        weaponGo = Instantiate(player.inventory.getCurrRootWeapon(), transform.GetChild(0));
        weaponGo.name = "Doppelganger Weapon";
        weapon = weaponGo.getWeaponCore();
        weapon.spriteRenderer.color = Color.black;
        weapon.spriteRenderer.sortingOrder = 9;
        weapon.SetWielder(player);
    }
    private void OnEnable() {
        playerPositions.Clear();
        playerAimDirections.Clear();

        
        if(player.inventory.getCurrRootWeapon().getWeaponCore() is MeleeWeapon currMw && weapon is MeleeWeapon doppelWpn) {
            if(!currMw.IsFirstSwing()) {
                doppelWpn.StartSwingAnimation("SkipSwing");
                Debug.Log("SkipSwing");
            }
        }
        
    }
    public float delay = 0.5f; // Il ritardo con cui il doppelganger imita i movimenti del player
    private readonly Queue<Vector3> playerPositions = new(); // Coda delle posizioni del player
    private void Update() {
        ImitatePlayerMovements();
        //ImitatePlayerAim(weaponsHolder.transform); 
        //l'ho commentata perchè playerAim è più sensato utilizzarlo:
        //è molto meglio che il dopppelganger miri direttamente al mouse invece che imitare le direzioni precedenti del mouse
    }

    private void ImitatePlayerMovements() {
        // Memorizza la posizione attuale del player
        playerPositions.Enqueue(player.transform.position);

        // Se il ritardo è passato, fa muovere il doppelganger verso la posizione in coda
        if (playerPositions.Count > delay / Time.deltaTime) {
            Vector3 targetPosition = playerPositions.Dequeue();
            float x = targetPosition.x - transform.position.x;
            float y = targetPosition.y - transform.position.y;
            animator.SetFloat("Moving", x + y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }
    //unused
    private Queue<Vector3> playerAimDirections = new(); // Coda delle direzioni di mira del player
    private void ImitatePlayerAim(Transform aimTransform) {
        // Memorizza la posizione e la direzione di mira attuali del player
        playerAimDirections.Enqueue(playerAim.aimDirection);

        // Se il ritardo è passato, fa muovere e mirare il doppelganger verso la posizione e la direzione in coda
        if (playerAimDirections.Count > delay / Time.deltaTime) {
            Vector3 targetAimDirection = playerAimDirections.Dequeue();
            float angle = Mathf.Atan2(targetAimDirection.y, targetAimDirection.x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0,0,angle);

            aimTransform.right = targetAimDirection;

            Vector3 aimLocalScale = Vector3.one;
            if(angle > 90 || angle < -90) 
                aimLocalScale.y = -1f;
            else
                aimLocalScale.y = +1f;
            
            aimTransform.localScale = aimLocalScale;
        }
    }
}
