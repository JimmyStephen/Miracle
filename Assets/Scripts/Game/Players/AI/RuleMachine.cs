using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RuleMachine
{
    public List<Rule> Rules = new();
    Rule CurrentRule;

    public void AddRule(Rule Rule)
    {
        if(!Rules.Contains(Rule)) Rules.Add(Rule);
    }

    public Rule GetBestRule()
    {
        CurrentRule = null;
        int currentPriority = -1;
        foreach (Rule rule in Rules)
        {
            int tempPriority = rule.CheckRule();
            (currentPriority, CurrentRule) = tempPriority > currentPriority ? (tempPriority, rule) : (currentPriority, CurrentRule);
        }
        return CurrentRule;
    }

    public Rule RuleFromName(string name)
    {
        foreach (var Rule in Rules)
        {
            if (string.Equals(Rule.RuleName, name, System.StringComparison.OrdinalIgnoreCase))
            {
                return Rule;
            }
        }
        return null;
    }

    public string GetRuleName()
    {
        return CurrentRule?.RuleName;
    }
}
