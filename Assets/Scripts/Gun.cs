using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum Status
{
    normal,
    used,
    rendted
};
[System.Serializable]
public class Gun 
{
    public Sprite sprite;
    public string Name;
    public int damage;
    public int dispersion;
    public int rateOfFire;
    public int reloadSpeed;
    public int ammunition;
    public Status status;
    
}
