using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public GameObject baseballbat, batton, cleaver, club, crowbar, fireaxe, flipknife, hammer, hatchet, heavywrench, katana,
        knife, machete, maul, pick, pitchfork, policebatton, wrench;

    public GameObject [] melee;




    public GameObject handgun, revolver, shotgun, twobarrel;
    public GameObject[] guns;

    public GameObject m4, mac10;
    public GameObject[] autoGuns;


    private void Awake()
    {
        // Assign the game objects to the melee array
        melee = new GameObject[] { baseballbat, batton, cleaver, club, crowbar, fireaxe, flipknife, hammer, hatchet, heavywrench, katana,
        knife, machete, maul, pick, wrench };

        guns = new GameObject[] { handgun, revolver, shotgun, twobarrel, m4, mac10 };

    }


    public void Update()
    {
        int num = baseballbat.GetComponentInChildren<WeaponStat>().attackPoints;
    }

}
