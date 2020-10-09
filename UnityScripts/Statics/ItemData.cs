using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{

    public enum Type {main, special, ultimate, mod, head, back, shirt, pants, shoes };

    public int itemID;
    public string itemName;
    public Type itemType;

}
