using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SequenceType {
    Neutral,
    Fire,
    Ice,
    Earth,
    Air
}

/*
    @class Sequence
    @description
        extends List<bool> as opposed to Array<bool> because the Array is a special class that is not extendable.
        See Compiler Error CS0644 @ https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0644
    @param {SequenceType} type
        The type of sequence as defined in the Enum SequenceType above.
    @param {int} capacity
        The capacity of the sequence. A sequence must have a capacity of 8.
*/
public class Sequence : List<bool> {
    public SequenceType type;

    private readonly SequenceType _type = SequenceType.Neutral;

    public Sequence(SequenceType type, int capacity) : base(capacity) {
        this.type = type;
    }

}

public class Runesong {
    private readonly Sequence neutral = new(SequenceType.Neutral, 8);
    private readonly Sequence fire = new(SequenceType.Fire, 8);
    private readonly Sequence ice = new(SequenceType.Ice, 8);
    private readonly Sequence earth = new(SequenceType.Earth, 8);
    private readonly Sequence air = new(SequenceType.Air, 8);

    public Runesong( Sequence neutral, Sequence fire, Sequence ice, Sequence earth, Sequence air) {
        this.neutral = neutral;
        this.fire = fire;
        this.ice = ice;
        this.earth = earth;
        this.air = air;
    }
    public Sequence GetNeutral() {
        return neutral;
    }
    public Sequence GetFire() {
        return fire;
    }
    public Sequence GetIce() {
        return ice;
    }
    public Sequence GetEarth() {
        return earth;
    }
    public Sequence GetAir() {
        return air;
    }
    public void Set(SequenceType type, List<bool> sequence) {
        try {
            var result = this.GetNeutral();

            if(sequence.Capacity != 8) throw new Exception("Invalid sequence Capacity; a sequence must have Capacity 8");

            if(type == SequenceType.Neutral) result = this.GetNeutral();
            else if(type == SequenceType.Fire) result = this.GetFire();
            else if(type == SequenceType.Ice) result = this.GetIce();
            else if(type == SequenceType.Earth) result = this.GetEarth();
            else if(type == SequenceType.Air) result = this.GetAir();
            else throw new Exception("Invalid sequence type");

            for(int i = 0; i < sequence.Capacity; i++) result.Add(sequence[i]);

        } catch (Exception e) {
            Debug.Log(e);
        } finally {
            // return to creation?
        }
    }
}

public class RunesongLibrary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
