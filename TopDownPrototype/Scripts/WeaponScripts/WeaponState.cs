using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponState : MonoBehaviour
{
    public enum Weapons
    {
        Pistol,
        Shotgun,
        Icegun,
    }

    public Weapons curWeapon;

    public Image reloadBar;
    public GameObject reloadText;
    //public Text curWeaponText;
    public Text PistolText;
    public Text ShotgunText;
    public Text IceText;
    public Text ammoText;

    public bool hasIceGun = false;
    public bool hasShotgun = false;
    
    [Header("Damages")]
    public float pistolDamage = 1f;
    public float shotgunDamage = 1f;
    public float iceGunDamage = 0f;

    private bool isShooting = false;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public bool isReloading = false;
    public float reloadTime;

    [Header("Bullet")]
    public MagicBase magic;
    public MagicBase shotgunBullet;
    public MagicBase IceBullet;
    public float magicSpeed = 5f;
    public Transform firePoint;
    public Transform firePoint01;
    public Transform firePoint02;

    [Header("Fire Rate")]
    public float timeBetweenShots;
    public float origTBS;
    public float pistolTime = .25f;
    public float shotgunTime = .5f;
    public float iceGunTIme = 1f;

    private PlayerStateMachine player;
    [Header("Audio")]
    public AudioClip pistolSound;
    public AudioClip shotgunSound;
    public AudioClip icegunSound;
    //public AudioSource audioSource;
    //public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStateMachine>();
        curWeapon = Weapons.Pistol;
        //curWeaponText = PistolText;
        //currentAmmo = maxAmmo;
        ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        //audioSource = GetComponent<AudioSource>();
        //reloadText.SetActive(false);
        //origTBS = timeBetweenShots;
    }
    /*
    public void UpdateWeapons()
    {
        hasIceGun = player.hasIceGun;
        hasShotgun = player.hasShotGun;
    }
    */
    // Update is called once per frame
    void Update()
    {
        switch (curWeapon)
        {
            case Weapons.Pistol:
                Pistol();
                break;
            case Weapons.Shotgun:
                Shotgun();
                break;
            case Weapons.Icegun:
                Icegun();
                break;
        }
        
        if (isShooting && !isReloading)
        {
            timeBetweenShots -= Time.deltaTime;
            if (timeBetweenShots <= 0)
                isShooting = false;
        }

        
        if (currentAmmo <= 0 && !isReloading)
        {
            isReloading = true;
            reloadText.SetActive(true);
            StartCoroutine(Reloading(reloadTime));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curWeapon = Weapons.Pistol;
            //curWeaponText = PistolText;
            isReloading = true;
            StartCoroutine(Reloading(reloadTime));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hasShotgun)
            {
                curWeapon = Weapons.Shotgun;
                //curWeaponText = ShotgunText;
                isReloading = true;
                StartCoroutine(Reloading(reloadTime));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (hasIceGun)
            {
                curWeapon = Weapons.Icegun;
                //curWeaponText = IceText;
                isReloading = true;
                StartCoroutine(Reloading(reloadTime));
            }
        }



        //curWeaponText.text = curWeapon.ToString();
        //curWeaponText.fontSize = 30;
        ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        reloadBar.transform.localScale = new Vector2(Mathf.Clamp(timeBetweenShots/origTBS,0f, 1f), reloadBar.transform.localScale.y);

        //Debug.Log("The current weapon is " + curWeapon + " and has a damage of " + magic.magicDamage);
    }

    public void Pistol()
    {
        magic.usingIceBullet = false;
        maxAmmo = 5;
        magic.magicDamage = pistolDamage;
        origTBS = pistolTime;
        PistolText.fontSize = 30;
        PistolText.color = Color.green;
        ShotgunText.fontSize = 20;
        ShotgunText.color = Color.red;
        IceText.color = Color.red;
        IceText.fontSize = 20;
        reloadTime = 1.5f;
        //audioSource.clip = audioClips[0];
    }

    public void Shotgun()
    {
        magic.usingIceBullet = false;
        maxAmmo = 3;
        magic.magicDamage = shotgunDamage;
        origTBS = shotgunTime;
        ShotgunText.fontSize = 30;
        ShotgunText.color = Color.green;
        PistolText.fontSize = 20;
        PistolText.color = Color.red;
        IceText.fontSize = 20;
        IceText.color = Color.red;
        reloadTime = 2.5f;
        //audioSource.clip = audioClips[1];
    }

    public void Icegun()
    {
        magic.usingIceBullet = true;
        maxAmmo = 3;
        magic.magicDamage = iceGunDamage;
        origTBS = iceGunTIme;
        IceText.fontSize = 30;
        IceText.color = Color.green;
        ShotgunText.fontSize = 20;
        ShotgunText.color = Color.red;
        PistolText.fontSize = 20;
        PistolText.color = Color.red;
        reloadTime = 2.5f;
        //audioSource.clip = audioClips[2];
    }

    public void ShootGun()
    {
        if (curWeapon == Weapons.Pistol)
        {
            //StartCoroutine(playerShooting(origTBS));

            if (timeBetweenShots <= 0 && currentAmmo > 0)
            {
                currentAmmo--;
                MagicBase newMagic = Instantiate(magic, firePoint.position, firePoint.rotation) as MagicBase;
                SoundManager.instance.PlaySingle(pistolSound);
                //audioSource.Play();
                isShooting = true;
                timeBetweenShots += origTBS;
            }/*
            else if(currentAmmo <= 0)
            {
                StartCoroutine(Reloading(reloadTime));
            }*/
        }
        else if(curWeapon == Weapons.Shotgun)
        {
            //StartCoroutine(playerShooting(origTBS));

            if (timeBetweenShots <= 0 && currentAmmo > 0)
            {
                currentAmmo--;
                MagicBase newMagic = Instantiate(shotgunBullet, firePoint.position, firePoint.rotation) as MagicBase;
                MagicBase newBullet = Instantiate(shotgunBullet, firePoint01.position, firePoint01.rotation) as MagicBase;
                MagicBase newBullet2 = Instantiate(shotgunBullet, firePoint02.position, firePoint02.rotation) as MagicBase;
                SoundManager.instance.PlaySingle(shotgunSound);
                //audioSource.Play();
                isShooting = true;
                timeBetweenShots += origTBS;
            }/*
            else if (currentAmmo <= 0)
            {
                StartCoroutine(Reloading(reloadTime));
            }*/
        }
        else if(curWeapon == Weapons.Icegun)
        {
            //StartCoroutine(playerShooting(origTBS));

            if (timeBetweenShots <= 0 && currentAmmo > 0)
            {
                currentAmmo--;
                MagicBase newMagic = Instantiate(IceBullet, firePoint.position, firePoint.rotation) as MagicBase;
                SoundManager.instance.PlaySingle(icegunSound);
                //audioSource.Play();
                isShooting = true;
                timeBetweenShots += origTBS;
            }
        }
        
        //Debug.Log("Shooting weapon with " + magic.magicDamage);
    }

    /*
    public IEnumerator playerShooting(float shotDelay)
    {
        MagicBase newMagic = Instantiate(magic, firePoint.position, firePoint.rotation) as MagicBase;

        yield return new WaitForSeconds(shotDelay);
    }
    */
    
    public IEnumerator Reloading(float reloadTime)
    {
        Debug.Log("Weapon is reloading at " + reloadTime + " seconds");

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        reloadText.SetActive(false);
        ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }
}
