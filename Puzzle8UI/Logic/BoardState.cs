using System.Text;
using Puzzle8UI.Logic.Exceptions;

namespace Puzzle8UI.Logic;

public class BoardState
{   
    public int Size { get; }
    public int[,] Matrix { get; }
    public int RowOfZero { get; set; }
    public int ColOfZero { get; set; }

    public BoardState(int[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new MatrixException("Error! Matrix size is not compatible!");
        }
        Matrix = (int[,])matrix.Clone();
        Size = Matrix.GetLength(0);  
        CalcIndexOfZero();
    }

    public void CalcIndexOfZero()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                if (Matrix[row, col] == 0)
                {
                    RowOfZero = row;
                    ColOfZero = col;
                    return;
                }
            }
        }

        throw new MatrixException("Error! Theres not zero in the matrix! " + ToString());
    }

    public Boolean IsMoveValid(String direction)
    {

        if (direction == "Up")
        {
            if (RowOfZero == 0)
            {
                return false;
            }
        }

        if (direction == "Down")
        {
            if (RowOfZero == Size - 1)
            {
                return false;
            }
        }

        if (direction == "Left")
        {
            if (ColOfZero == 0) 
            {
                return false;
            }
        }

        if (direction == "Right")
        {
            if (ColOfZero == Size - 1)
            {
                return false;
            }
        }
        
        return true;
    }

    public BoardState? Move(String direction)
    {
        if (!IsMoveValid(direction)) 
        {
            return null;
        }
        
        int newRow = RowOfZero;
        int newCol = ColOfZero;
        
        if (direction == "Up") newRow--;
        if (direction == "Down") newRow++;
        if (direction == "Left") newCol--;
        if (direction == "Right") newCol++;
        
        int[,] newMatrix = (int[,])Matrix.Clone(); //
        newMatrix[RowOfZero, ColOfZero] = newMatrix[newRow, newCol];
        newMatrix[newRow, newCol] = 0;
        
        return new BoardState(newMatrix);
    }

    public String GetHashString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (int i in Matrix) sb.Append(i + "-");
        return sb.ToString();
    }

    public int[] ToOneDimensional()
    {
        int[] linearPuzzle = new int[Size*Size];
        int counter = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                linearPuzzle[counter++] = Matrix[i, j];
            }
        }

        return linearPuzzle;
    }

    public int CountInversions()
    {
        int inversionsCount = 0;
        int[] matrix = ToOneDimensional();
        for (int i = 0; i < Size; i++)
        {
            for (int j = i + 1; j < Size; j++)
            {
                if (matrix[i] != 0 && matrix[j] != 0 && matrix[i] > matrix[j])
                {
                    inversionsCount++;
                }
            }
        }
        return inversionsCount;
    }
    
    public Boolean IsSolvable()
    {
        if (Size % 2 == 1)
        {
            return (CountInversions() % 2 == 0);
        }
        if ((Size - RowOfZero) % 2 == 0)
        {
            return (CountInversions() % 2 == 1);
        } 
        return (CountInversions() % 2 == 0);
    }

    public override string ToString()
    {
        String output = "";
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                output += Matrix[i, j];
            }
        }
        return output;
    }
}