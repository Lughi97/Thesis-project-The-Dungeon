using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpecialAttack {Explosion, Projectile}

public class SpecialAttackScripts : MonoBehaviour
{
    private PlayerStats _playerStats;
    private GameManager _gameManager;
    public SpecialAttack _spk;
    public float ExpolsionPower;
    public bool activeExplosion=false;
    public float coolDownExp;
    public float usedMana;
    public SpriteRenderer ImageExp;
   // public GameObject Explosion;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        // _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        switch (_spk)
        {
            case SpecialAttack.Explosion:
                ExpolsionPower = _playerStats.powerExplosion;
                coolDownExp = _playerStats.coolDown;
                usedMana = _playerStats.UsageManaExplosion;
                ImageExp = GetComponent<SpriteRenderer>();
                ImageExp.enabled = false;
                break;
            case SpecialAttack.Projectile:
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        //  ExpolsionPower = _playerStats.powerExplosion;
        //coolDownExp = _playerStats.coolDown;
        //  this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(_gameManager._setUp==false )
    }


    public void ActivateAttack()
    {
        ImageExp.enabled = true;
        activeExplosion = true;

        StartCoroutine(explosionDruation());
        StartCoroutine(coolDownExplosion());
    }

    IEnumerator explosionDruation()
    {

        yield return new WaitForSeconds(1f);
        //this.gameObject.SetActive(false);
      //  StartCoroutine(coolDownExplosion());
    }
    IEnumerator coolDownExplosion()
    {
        // float time = 2f;
        while (coolDownExp > 0.1)
        {
            coolDownExp--;
            Debug.Log("CoolDown " + coolDownExp);
            activeExplosion = false;
            yield return new WaitForSeconds(coolDownExp);

        }
        yield return new WaitForSeconds(1f);
        activeExplosion = false;
        this.gameObject.SetActive(false);
        Debug.Log("Able to use again!");

    }
}
