using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Item")]
public class Itemdata : ScriptableObject
{
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public int cost;
}
