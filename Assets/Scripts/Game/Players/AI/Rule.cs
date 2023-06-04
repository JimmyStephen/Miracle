public abstract class Rule
{
    //Update rules so that they keep track of cards in hand, to avoid having to for loop through them all each time

    public string RuleName;
    public string RuleDescription;
    public Player player;
    public EnemyAI AI;
    public Rule(string name, string description, Player player, EnemyAI AI) { RuleName = name; RuleDescription = description; this.player = player; this.AI = AI; }
    /// <summary>
    /// Checks how much the AI wants to use this rule.
    /// </summary>
    /// <returns>A int value, normalized between 0-100 of how much the AI wishes for this rule to be run</returns>
    public abstract int CheckRule();
    /// <summary>
    /// Runs the rule
    /// </summary>
    public abstract Gameplay_Card RunRule();
}