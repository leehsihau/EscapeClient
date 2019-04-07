using UnityEngine;

public static class State
{
    private static int curX;
    private static int curY;
    private static int curZ;
    private static int curHealth;
    private static int maxHealth;
    private static int attack;
    private static int armor;
    // TODO: Keys

    public static int CurX {
        get {
            return curX;
        } set {
            curX = value;
        }
    }

    public static int CurY
    {
        get
        {
            return curY;
        }
        set
        {
            curY = value;
        }
    }

    public static int CurZ
    {
        get
        {
            return curZ;
        }
        set
        {
            curZ = value;
        }
    }

    public static int CurHealth
    {
        get
        {
            return curHealth;
        }
        set
        {
            curHealth = value;
        }
    }

    public static int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public static int Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public static int Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }
}
