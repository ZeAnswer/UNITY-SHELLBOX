using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerDeathEventArgs : EventArgs
    {
        public int playerID { get; set; }
    }

public class PlayerHitEventArgs : EventArgs
    {
        public float damage_amount {get; set;}
    }