using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]private GameObject SubtractHitbox;

    [SerializeField]private GameObject AttackHitbox;
    [SerializeField]private float attackTimer;
    private int currentWeapon = 0;
    private WeaponHandler wh;

    void Start(){
        wh = GetComponent<WeaponHandler>();
        //wh = FindObjectOfType<WeaponHandler>();
    }

    public string performOperation(){
        string operation = wh.getCurrentWeaponOperation();
        int level = wh.getCurrentWeaponLevel();
        Debug.Log("Operation: " + operation + "     Level: " +  level);
        string output = $"{operation}{level}";

        return output;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log($"SPACE DOWN PLAYER ATTACK SCRIPT");
            //StartCoroutine(performOperation());
            //turn on the subtract hitbox
            if(wh.getCurrentWeaponOperation() == "-")
            {
                StartCoroutine(Attack(SubtractHitbox));
            }
        }
    }

    IEnumerator Attack(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(attackTimer);
        hitbox.SetActive(false);
    }

}
