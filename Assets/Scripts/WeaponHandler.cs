using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHandler : MonoBehaviour
{    
    public static WeaponHandler Instance;

    private Weapon w1 = new Weapon("w1",1,"-");
    private Weapon w2 = new Weapon("w2",1,"+");
    private Weapon w3 = new Weapon("w3",2,"*");
    private Weapon w4 = new Weapon("w4",2,"/");

    public Weapon[] weapons;
    private Weapon currentWeapon;
    public TMP_Text[] weaponLevelText;
    public Image[] Frames;
    public Image[] weaponIconsUI;
    public TMP_Text[] weaponOperationUI;
    public Sprite[] weaponIcons;
    public SpriteRenderer heldWeaponImage;
    public Sprite unselectedFrame;
    public Sprite selectedFrame;

    // Start is called before the first frame update
    void Start()
    {   
        weapons = new Weapon[] {w1,w2,w3,w4};
        currentWeapon = weapons[0];
        Debug.Log($"Starting Weapon: {currentWeapon.getName()}");
        GameManager.beatLevel += UnlockWeapons;

        if(Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        UpdateLevelUI();
        UnlockWeapons(1);
        SwitchWeapon(w1, 0);

    }

    private void OnDestroy()
    {
        GameManager.beatLevel -= UnlockWeapons;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.instance.GetCurrentLevel() >= 1) SwitchWeapon(w1, 0);
        if (Input.GetKeyDown(KeyCode.Alpha2)&& GameManager.instance.GetCurrentLevel() >= 2) SwitchWeapon(w2, 1);
        if (Input.GetKeyDown(KeyCode.Alpha3)&& GameManager.instance.GetCurrentLevel() >= 3) SwitchWeapon(w3, 2);
        if (Input.GetKeyDown(KeyCode.Alpha4)&& GameManager.instance.GetCurrentLevel() >= 4) SwitchWeapon(w4, 3);
        if (Input.GetKeyDown(InputHandler.Instance.levelKeyLeft))
        {
            currentWeapon.DecreaseLevel();
            UpdateLevelUI();
        }
        if (Input.GetKeyDown(InputHandler.Instance.levelKeyRight))
        {
            currentWeapon.IncreaseLevel();
            UpdateLevelUI();
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log($"Spacebar was pressed.");
            string currentWeaponOperation = $"{currentWeapon.getOperation()}";
            int currentWeaponLevel = currentWeapon.getLevel();
        }
    }

    void SwitchWeapon(Weapon weapon, int weaponIndex){
        currentWeapon = weapon;
        Debug.Log($"Switching icon to {weaponIconsUI[weaponIndex].sprite.name}");
        heldWeaponImage.sprite = weaponIconsUI[weaponIndex].sprite;
        for(int i=0; i < weapons.Count(); i++)
        {
            if(i == weaponIndex)
            {
                Frames[weaponIndex].sprite = selectedFrame;
            }
            else
            {
                Frames[i].sprite = unselectedFrame;
            }
        }
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

    public void UpdateLevelUI()
    {
        for(int i=0; i< weaponLevelText.Count(); i++)
        {
            weaponLevelText[i].text = weapons[i].getLevel().ToString();
        }
    }

    public void UnlockWeapons(int weapon)
    {
        weaponIconsUI[weapon - 1].sprite = weaponIcons[weapon - 1];
        weaponOperationUI[weapon - 1].gameObject.SetActive(true);
    }
    
}

public class Weapon
{
    private string name;
    private int level;
    private int maxlevel;
    private int minlevel;
    private int baseDamage;
    private string operation;

    public string getName(){ Debug.Log("getName() called");return this.name;}
    public int getLevel(){ Debug.Log("getLevel() called");return this.level;}
    public int getBaseDamage(){ Debug.Log("getBaseDamage() called");return this.baseDamage;}
    public string getOperation(){ Debug.Log("getOperation() called");return this.operation;}

    public Weapon(string name, int level, string operation){
        this.name = name;
        this.level = level;
        this.maxlevel = level;
        this.minlevel = level;
        this.baseDamage = this.level;
        this.operation = operation;
    }

    public void LevelUp(int up=1){
        Debug.Log("LevelUp() called");
        maxlevel+=up;
        Debug.Log($"{name} levelrf up to level {maxlevel}!");
    }

    public string getcurrOperation()
    {
        return operation;
    }
    public int getcurrLevel()
    {
        return level;
    }

    public void DecreaseLevel()
    {
        if(level == minlevel)
        {
            level = maxlevel;
        }
        else{
            level--;
        }
    }

    public void IncreaseLevel()
    {
        if(level == maxlevel)
        {
            level = minlevel;
        }
        else
        {
            level++;
        }
    }


}