using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{    
    private Weapon w1 = new Weapon("w1",1,"-");
    private Weapon w2 = new Weapon("w2",1,"+");
    private Weapon w3 = new Weapon("w3",2,"*");
    private Weapon w4 = new Weapon("w4",2,"/");

    private Weapon[] weapons;
    private Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {   
        weapons = new Weapon[] {w1,w2,w3,w4};
        currentWeapon = weapons[0];
        Debug.Log($"Starting Weapon: {currentWeapon.getName()}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(w1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(w2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(w3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(w4);

        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log($"Spacebar was pressed.");
            string currentWeaponOperation = $"{currentWeapon.getOperation()}";
            int currentWeaponLevel = currentWeapon.getLevel();
        }
    }

    void SwitchWeapon(Weapon weapon){
        currentWeapon = weapon;
        Debug.Log($"Switched to: {currentWeapon.getName()}");
    }

    public string getCurrentWeaponOperation()
    {
        return currentWeapon.getcurrOperation();
    }
    public int getCurrentWeaponLevel()
    {
        return currentWeapon.getcurrLevel();
    }
    
}

public class Weapon
{
    private string name;
    private int level;
    private int baseDamage;
    private string operation;

    public string getName(){ Debug.Log("getName() called");return this.name;}
    public int getLevel(){ Debug.Log("getLevel() called");return this.level;}
    public int getBaseDamage(){ Debug.Log("getBaseDamage() called");return this.baseDamage;}
    public string getOperation(){ Debug.Log("getOperation() called");return this.operation;}

    public Weapon(string name, int level, string operation){
        this.name = name;
        this.level = level;
        this.baseDamage = this.level;
        this.operation = operation;
    }

    public void LevelUp(int up=1){
        Debug.Log("LevelUp() called");
        level+=up;
        Debug.Log($"{name} levelrf up to level {level}!");
    }

    public string getcurrOperation()
    {
        return operation;
    }
    public int getcurrLevel()
    {
        return level;
    }
}