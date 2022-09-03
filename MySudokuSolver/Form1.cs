using System.Windows.Forms;

namespace MySudokuSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int[,] SudokuMatrix = new int[9, 9];

     
        //Validate and assemble Array based on Grid values
        private bool ValidGrid()
        {
            int num;
            for (int i = 0; i < DataGridView1.RowCount; i++)
            {
                for (int j = 0; j < DataGridView1.ColumnCount; j++)
                {

                    if (DataGridView1.Rows[i].Cells[j].Value == null || DataGridView1.Rows[i].Cells[j].Value == "")
                    {
                        SudokuMatrix[i, j] = 0;
                    }
                    else if (Int32.TryParse(DataGridView1.Rows[i].Cells[j].Value.ToString(), out num))
                    {
                        if (!ValidNum(i, j, num)) return false;
                        else SudokuMatrix[i, j] = num;
                    }
                    else return false;

                }
            }
            return true;
        }
        private bool Solver(int r, int c)
        {

            int column = c, row = r;

            //Stop condition
            if (c == 9) return true;
            //Get next position
            if (row < 8) row++;
            else
            {
                row = 0;
                column++;
            }

            //Check if the  position is empty
            if (SudokuMatrix[r, c] != 0) return Solver(row, column);
            else
            {
                //Loop in the possible elements to be filled
                for (int num = 1; num < 10; num++)
                {
                    //Call function to validate if element is valid
                    if (ValidNum(r, c, num))
                    {
                        SudokuMatrix[r, c] = num;
                        DataGridView1.Rows[r].Cells[c].Value = num;
                        DataGridView1.Rows[r].Cells[c].Style.ForeColor = Color.Black;
                       
                        //Call function with next position
                        if (Solver(row, column)) return true;
                        //If it is not possible to fill in the box, reset the value to try again
                        SudokuMatrix[r, c] = 0;
                        DataGridView1.Rows[r].Cells[c].Value = "";
                    }
                }
            }
            return false;
        }
        //Returns if a given number is valid 
        private bool ValidNum(int r, int c, int num)
        {
            //Check if it repeats in rows and columns
            for (int i = 0; i < 9; i++)
            {
                if (SudokuMatrix[r, i] == num)
                    return false;
                if (SudokuMatrix[i, c] == num)
                    return false;
            }

            //Get the quadrant of the number
            int row = (r / 3) * 3;
            int column = (c / 3) * 3;

            //Check if it repeats in the quadrant
            for (int j = row; j < row + 3; j++)
            {
                for (int k = column; k < column + 3; k++)
                {
                    if (SudokuMatrix[j, k] == num) return false;
                }
            }

            return true;
        }
        //Clear all Grid cells
        private void ClearGrid()
        {
            for (int i = 0; i < DataGridView1.RowCount; i++)
            {
                for (int j = 0; j < DataGridView1.ColumnCount; j++)
                {
                    DataGridView1.Rows[i].Cells[j].Value = null;
                    DataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.Crimson;
                }
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearGrid();
            //grid unlock
            DataGridView1.ReadOnly = false;


        }
        // Grid Fill


        private void Form1_Load(object sender, EventArgs e)
        {
            DataGridView1.Rows.Add(9);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    DataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightGoldenrodYellow;
                    DataGridView1.Rows[i].Cells[j + 6].Style.BackColor = Color.LightGoldenrodYellow;
                    DataGridView1.Rows[i + 3].Cells[j + 3].Style.BackColor = Color.LightGoldenrodYellow;
                    DataGridView1.Rows[i + 6].Cells[j].Style.BackColor = Color.LightGoldenrodYellow;
                    DataGridView1.Rows[i + 6].Cells[j + 6].Style.BackColor = Color.LightGoldenrodYellow;
                }
            }
        }

        private void btnSolve_Click_1(object sender, EventArgs e)
        {
            //Array Clear
            SudokuMatrix = new int[9, 9];
            //Validate the grid information
            if (!ValidGrid())
            {
                MessageBox.Show("Invalid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Call function to solve Sudoku
            if (Solver(0, 0)) MessageBox.Show("Sudoku solved successfully!", "successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Impossible to solve Sudoku!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //Grid lock
            DataGridView1.ReadOnly = true;

        }
    }
}