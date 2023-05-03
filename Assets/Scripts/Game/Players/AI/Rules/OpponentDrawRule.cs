using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OpponentDrawRule : Rule
{
    public OpponentDrawRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        throw new NotImplementedException();
    }

    public override Card RunRule()
    {
        throw new NotImplementedException();
    }
}

