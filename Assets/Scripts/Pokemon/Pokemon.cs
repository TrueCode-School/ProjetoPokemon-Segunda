using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base { get { return _base; } }
    public int Level { get{ return level; } }
    public List<Move> Moves { get; set; }
    public int HP { get; set;}
    
    public void init()
    {
        HP = MaxHp;

        //Movimentos
        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.MoveBase));
            if (Moves.Count >= 4)
                break;
        }
 
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 48f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 48f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 48f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 48f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 48f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 22f) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if(Random.value * 100f <= 6.5f)
        {
            critical = 2f;
            Debug.Log("Cr�tico");
        }

        float type = TypeChart.GetEffectivess(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectivess(move.Base.Type, this.Base.Type2);
        Debug.Log("Deu dano no type:" + type);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };
        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type;
        float a = (2 * attacker.Level * critical)/5f + 2;
        float d = (a * move.Base.Power * (attack/ defense))/50f + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }


}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
