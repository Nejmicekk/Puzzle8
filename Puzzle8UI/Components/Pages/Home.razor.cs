using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Puzzle8UI.Logic;

namespace Puzzle8UI.Components.Pages
{
    public partial class Home : ComponentBase
    {
        public int InputSize { get; set; } = 3;
        public int Size { get; set; } = 3;
        public int[,]? StartMatrix { get; set; } = null;
        public int[,]? TargetMatrix { get; set; } = null;
        
        public string? StatusMessage { get; set; }
        public bool IsError { get; set; } = false;
        public bool IsModal { get; set; } = false;

        public List<SearchNode> SolutionPath = new List<SearchNode>();
        public int? NodesTraversed { get; set; } = null;
        public string TimeElapsed { get; set; } = null;

        public int CurrentIndex { get; set; } = -1;

        public static readonly int[,] DEFAULT_DISPLAY_MATRIX = new int[,]{
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        public int[,]? DisplayMatrix { get; set; } = DEFAULT_DISPLAY_MATRIX; 
        public String CurrentMove { get; set; } = null;

        public async Task ShowMessage(string message, bool isError)
        {
            StatusMessage = message;
            IsError = isError;
            StateHasChanged();
            
            await Task.Delay(5000);
            StatusMessage = null;
            StateHasChanged();
        }

        public int[,] GenerateMatrix()
        {
            int[,] matrix = new int[Size, Size];
            int number = 1;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrix[i, j] = number;
                    number += 1;
                }
            }
            matrix[Size-1, Size-1] = 0;
            return matrix;
        }

        public void OnGenerateClick()
        {
            if (InputSize < 3 || InputSize > 6)
            {
                ShowMessage("Matrix has to be in range <3,6>!", true);
                IsModal = true;
                
                StartMatrix = null;
                TargetMatrix = null;
            }
            else
            {
                Size = InputSize;
                
                StatusMessage = null;
                StartMatrix = GenerateMatrix();
                TargetMatrix = GenerateMatrix();
                
                DisplayMatrix = StartMatrix;
                SolutionPath.Clear();
                NodesTraversed = null;
                TimeElapsed = null;
                CurrentMove = null;
            }
        }
        
        public void Reset()
        {
            StartMatrix = null;
            TargetMatrix = null;
            StatusMessage = "";
            IsError = false;
            Size = 3;
            InputSize = 3;

            SolutionPath.Clear();
            NodesTraversed = null;
            TimeElapsed = null;
            CurrentMove = null;
        }
        
        public void OnResetClick()
        {
            StartMatrix = null;
            TargetMatrix = null;
            StatusMessage = "";
            IsError = false;
            Size = 3;
            InputSize = 3;
            
            DisplayMatrix = DEFAULT_DISPLAY_MATRIX;
            
            SolutionPath.Clear();
            NodesTraversed = null;
            TimeElapsed = null;
            CurrentMove = null;
        }
            
        public async Task OnStartClick()
        {
            IsModal = false;
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Solver solver = new Solver(StartMatrix, TargetMatrix);
            SearchNode targetNode = solver.Solve();
            if (targetNode.LastMove == "Start")
            {
                await ShowMessage("Matrices are the same!", false);
            }
            else
            {
                if (solver.IsSolvable)
                {
                    if (targetNode != null)
                    {
                        SearchNode currentNode = targetNode;

                        while (currentNode.Parent != null)
                        {
                            SolutionPath.Add(currentNode);
                            currentNode = currentNode.Parent;
                        }

                        SolutionPath.Add(currentNode);
                        NodesTraversed = SolutionPath.First().NodesTraversed;
                        SolutionPath.Reverse();
                    
                        SelectNode(0);
                    }
                    stopwatch.Stop();
                    TimeElapsed = $"{stopwatch.ElapsedMilliseconds} ms";
                    await ShowMessage("Successfully solved!", false);
                }
                else
                {
                    DisplayMatrix = StartMatrix;
                    await ShowMessage("These matrices are not solvable!", true);
                    Reset();
                }
            }
        }

        public void SelectNode(int index)
        {
            if (index >= 0 && index < SolutionPath.Count)
            {
                CurrentIndex = index;

                SearchNode selectedNode = SolutionPath[index];
                DisplayMatrix = selectedNode.CurrentState.Matrix;
                CurrentMove = selectedNode.LastMove ?? "Start";
            }
        }
        
        public void LoadEasyPreset()
        {
            StatusMessage = "Loaded 3x3 Example";
            IsError = false;
            IsModal = true;
            Size = 3;
            InputSize = 3;
            
            StartMatrix = new int[,] {
                { 8, 6, 7 },
                { 2, 5, 4 },
                { 3, 0, 1 }
            };
            
            TargetMatrix = new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 }
            };
            
            DisplayMatrix = StartMatrix;
        }
    }
}