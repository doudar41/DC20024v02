using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequencerWeapon : MonoBehaviour
{

    SequenceRow[] sequenceWeapon = new SequenceRow[3];
    public Sprite[] effectSprites;

    int countSteps = 0;
    public int countStepMax = 4; //should be twice bigger than toggle buttons on panel
    bool action = true;
    Step chosenStep = new Step();
    public int ownedRunes = 5; 
    int quantityOfRunes = 5;
    public DetectBeat detectBeat;
    List<StepEffect> memorizedEffects = new List<StepEffect>();

    int runesInRow01, runesInRow02, runesInRow03, runesInRow04, runesInRow05;
    public UISpriteAnimation[] vfxs;


    [SerializeField] TextMeshProUGUI runeQuantity;

    // Start is called before the first frame update
    void Start()
    {
        quantityOfRunes = ownedRunes;
        runeQuantity.text = (quantityOfRunes-1).ToString();
        sequenceWeapon[0] = new SequenceRow();
        sequenceWeapon[0].rowEffect = StepEffect.neutral;
        sequenceWeapon[1] = new SequenceRow();
        sequenceWeapon[1].rowEffect = StepEffect.fire;
        sequenceWeapon[2] = new SequenceRow();
        sequenceWeapon[2].rowEffect = StepEffect.water;
    }

    public void StepReader()
    {

        //It will fire each marker but only even ones will have a effect
        if (!action) return;
        List<StepEffect> effectsCombo = new List<StepEffect>();
        //Need to be list of quantity of runes in different rows 

        foreach (StepEffect ef in memorizedEffects)
        {
            PlayVFXAndApplyDamage(ef);
            //play graphic VFX with sound

        }

        memorizedEffects.Clear();

        //foreach(SequenceRow s in sequenceWeapon)
        for (int i =0;i<sequenceWeapon.Length;i++)
        {
            if (sequenceWeapon[i].GetStepFromRow(countSteps) != null)
            {
                int damage = 0;
                
                //print("effect "+ sequenceWeapon[i].GetStepFromRow(countSteps).stepEffect);
                string nameParameter = "";
                effectsCombo.Add(sequenceWeapon[i].rowEffect);
                //launch effect associated with row using sequenceWeapon[i].rowEffect
                //also need delay of graphics so it will in sync with sounds
                memorizedEffects.Add(sequenceWeapon[i].rowEffect);
                switch (i)
                {
                    case 0:
                        nameParameter = "SequenceStep01";
                        runesInRow01++;
                        break;
                    case 1:
                        //launch fire damage
                        nameParameter = "SequenceStep02";
                        runesInRow02++;
                        break;
                    case 2:
                        //launch fire damage
                        nameParameter = "SequenceStep03";
                        runesInRow03++;
                        break;
                }
                detectBeat.SetParameterToInstance(nameParameter, i);
                // this is not working
            }
            else
            {
                string nameParameter = "";
                switch (i)
                {
                    case 0:
                        nameParameter = "SequenceStep01";
                        break;
                    case 1:
                        nameParameter = "SequenceStep02";
                        break;
                    case 2:
                        nameParameter = "SequenceStep03";
                        break;
                }
                detectBeat.SetParameterToInstance(nameParameter, 17);
            }
        }

        //Rules of combos
        if (effectsCombo.Contains(StepEffect.neutral) && effectsCombo.Contains(StepEffect.fire))
        {
            print("Combo");
            //heal other
        }
        
        //Combo or not use damage calculation to damage opponent? 
        
        countSteps++;
        if (countSteps >= countStepMax) {
            //make some use of number of runes in rows, compare them with amount of cells in row, and reset it 
            runesInRow01 = 0; runesInRow02 = 0; runesInRow03 = 0; runesInRow04 = 0; runesInRow05 = 0;
            countSteps = 0; }
    }

    public void PlayVFXAndApplyDamage(StepEffect ef)
    {
        switch (ef)
        {
            case StepEffect.air:
                print("playing VFX");
                
                break;
            case StepEffect.fire:
                print("playing VFX");
                vfxs[1].Func_PlayUIAnim();
                break;
            case StepEffect.earth:
                print("playing VFX");
                break;
            case StepEffect.water:
                print("playing VFX");
                break;
            case StepEffect.neutral:
                print("playing VFX");
                vfxs[0].Func_PlayUIAnim();
                break;
        }
    }

    public void ReadIndexFromSequencerRow(int rowIndex, int row, bool isOn, Image runeFace )
    {
        
        print(rowIndex+" "+ isOn);
        if (isOn)
        {
            if (quantityOfRunes > 0)
            {
                quantityOfRunes--;
                quantityOfRunes = Mathf.Clamp(quantityOfRunes, 0, ownedRunes);
                runeQuantity.text = (quantityOfRunes - 1).ToString();
                sequenceWeapon[row].SetStepInRow(rowIndex, chosenStep);
            }
            
            //runeFace.sprite = StepEffectToSprite(chosenStep.stepEffect);
        }

        if (!isOn)
        {
            quantityOfRunes++;
            quantityOfRunes = Mathf.Clamp(quantityOfRunes, 0, ownedRunes);
            runeQuantity.text = (quantityOfRunes - 1).ToString();
            sequenceWeapon[row].RemoveStepFromRow(rowIndex);
        }
    }

    public bool CheckIfRunesLeft()
    {
        runeQuantity.text = (quantityOfRunes - 1).ToString();
        return quantityOfRunes>0;
    }

    public void SetChosenStep(Step sf)
    {
        chosenStep = sf;
    }

    public Sprite StepEffectToSprite(StepEffect sf)
    {
        switch (sf)
        {
            case StepEffect.missile:
                return effectSprites[(int)sf];
            case StepEffect.shield:
                return effectSprites[(int)sf];
        }
        return null;

    }

    public void GetStepFromButton(Step step)
    {
        chosenStep = step;
    }

}

[System.Serializable]
public class SequenceRow
{
    Dictionary<int, Step> Row = new Dictionary<int, Step>();
    public StepEffect rowEffect;
    public int rowLevel = 1;
    public void SetStepInRow(int index, Step step)
    {
        Row.Add(index, step);
    }

    public Step GetStepFromRow(int index)
    {
        Step newStep = new Step();
        if (Row.TryGetValue(index, out newStep))
        {
            return newStep;
        }
        return null;
    }

    public void RemoveStepFromRow(int index)
    {
        Row.Remove(index);
    }

}

[System.Serializable]
public class Step
{
    public StepEffect stepEffect = StepEffect.missile;
}

[System.Serializable]
public enum StepEffect
{
    missile,
    shield,
    heal,
    air,
    fire,
    earth,
    water,
    neutral
}