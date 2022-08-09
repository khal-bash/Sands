using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void IntUpdated(int value);

public class Health : MonoBehaviour
{

    public int HP;
    public event IntUpdated HPUpdated;

    public void ChangeHP(int change)
    {
        HP += change;
        OnHPUpdate();
    }

    protected virtual void OnHPUpdate()
    {
        HPUpdated?.Invoke(HP);
    }

    void Start()
    {
        ChangeHP(0);
    }

}
