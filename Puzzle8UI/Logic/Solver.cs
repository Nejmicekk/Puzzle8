namespace Puzzle8UI.Logic;

public class Solver
{
    public BoardState StartState { get; set; }
    public BoardState TargetState { get; set; }
    public Boolean IsSolvable { get; set; }
    
    public Solver(int[,] startMatrix, int[,] targetMatrix)
    {
        StartState = new BoardState(startMatrix);
        TargetState = new BoardState(targetMatrix);
    }

    public SearchNode? Solve()
    {
        if (StartState.GetHashString() == TargetState.GetHashString())
        {
            IsSolvable = true;
            return new SearchNode(StartState, null, "Start", 0, 0);
        }
        
        IsSolvable = (StartState.IsSolvable() == TargetState.IsSolvable());

        if (IsSolvable)
        {
            PriorityQueue<SearchNode, int> queue = new PriorityQueue<SearchNode, int>();

            HashSet<string> visited = new HashSet<string>();

            SearchNode startNode = new SearchNode(StartState, null, "Start", 0, 0);
            
            queue.Enqueue(startNode, startNode.Price);

            while (queue.Count > 0)
            {
                SearchNode currentNode = queue.Dequeue();
                BoardState currentState = currentNode.CurrentState;
                if (currentState.GetHashString() == TargetState.GetHashString())
                {
                    return currentNode;
                }

                foreach (String direction in new[] { "Up", "Down", "Left", "Right" } )
                {
                    BoardState? newState = currentState.Move(direction);
                    if (newState is null) continue;
                    if(visited.Contains(newState.GetHashString())) continue;
                    SearchNode newNode = new SearchNode(
                        newState,
                        currentNode,
                        direction,
                        currentNode.NodesTraversed + 1,
                        0);
                    queue.Enqueue(newNode, newNode.Price);
                    visited.Add(newState.GetHashString());
                }
            }
        }
        return null;
    }
}