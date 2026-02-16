namespace Puzzle8UI.Logic;

public class SearchNode
{
    public BoardState CurrentState { get;}
    public SearchNode? Parent { get;}
    public String LastMove { get;}
    public int NodesTraversed { get; } //g
    public int HeuristicCost { get; } //h
    public int Price => NodesTraversed + HeuristicCost;

    public SearchNode(BoardState currentState, SearchNode? parent, string lastMove, int nodesTraversed, int heuristicCost)
    {
        CurrentState = currentState;
        Parent = parent;
        LastMove = lastMove;
        NodesTraversed = nodesTraversed;
        HeuristicCost = heuristicCost;
    }

    public override string ToString()
    {
        return $"{CurrentState} {LastMove} {NodesTraversed} {HeuristicCost}";
    }
}