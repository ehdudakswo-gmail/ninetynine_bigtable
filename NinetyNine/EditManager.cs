using NinetyNine.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    class EditManager
    {
        private readonly string CELL_EMPTY_VALUE = "";

        internal void SetUndoData(DataGridView selectedDataGridView)
        {
            DataTable originDataTable = (DataTable)selectedDataGridView.DataSource;
            DataTable copyDataTable = originDataTable.Copy();

            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;
            List<CellPosition> cellPositions = new List<CellPosition>();

            foreach (DataGridViewCell selectedCell in selectedCells)
            {
                CellPosition cellPosition = new CellPosition();
                cellPosition.rowIdx = selectedCell.RowIndex;
                cellPosition.colIdx = selectedCell.ColumnIndex;
                cellPositions.Add(cellPosition);
            }

            EditUndo editUndo = new EditUndo();
            editUndo.dataTable = copyDataTable;
            editUndo.cellPositions = cellPositions;

            EditUndoManager.Instance.Set(editUndo);
        }

        internal void Undo(DataGridView selectedDataGridView, EditUndo editUndo)
        {
            DataTable originDataTable = (DataTable)selectedDataGridView.DataSource;
            DataTable backupDataTable = editUndo.dataTable;

            int rowRemoveCount = originDataTable.Rows.Count - backupDataTable.Rows.Count;
            if (rowRemoveCount > 0)
            {
                for (int i = 0; i < rowRemoveCount; i++)
                {
                    int lastIdx = originDataTable.Rows.Count - 1;
                    originDataTable.Rows.RemoveAt(lastIdx);
                }
            }

            int rowCnt = originDataTable.Rows.Count;
            int colCnt = originDataTable.Columns.Count;

            for (int i = 0; i < rowCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    if (originDataTable.Rows[i][j] != backupDataTable.Rows[i][j])
                    {
                        originDataTable.Rows[i][j] = backupDataTable.Rows[i][j];
                    }
                }
            }

            selectedDataGridView.ClearSelection();
            List<CellPosition> cellPositions = editUndo.cellPositions;
            foreach (CellPosition cellPosition in cellPositions)
            {
                int rowIdx = cellPosition.rowIdx;
                int colIdx = cellPosition.colIdx;
                selectedDataGridView[colIdx, rowIdx].Selected = true;
            }
        }

        internal string[][] GetClipBoardCells(string text)
        {
            const char LINE_SEPARATOR = '\n';
            const char CELL_SEPARATOR = '\t';

            string[] lines = text.Split(LINE_SEPARATOR);
            int lineLen = lines.Length;

            string lastLine = lines[lineLen - 1];
            if (lastLine.Length == 0)
            {
                lineLen--;
            }

            string[][] clipboardCells = new string[lineLen][];
            for (int i = 0; i < clipboardCells.Length; i++)
            {
                string line = lines[i];
                clipboardCells[i] = line.Split(CELL_SEPARATOR);
            }

            return clipboardCells;
        }

        internal bool IsColumnSizeLimit(DataGridView selectedDataGridView, CellIndex selectedCellIndex, string[][] clipboardCells)
        {
            int colCnt = selectedDataGridView.ColumnCount;
            int lastColIdx = colCnt - 1;
            int minColIdx = selectedCellIndex.minColIdx;
            int maxColIdx = 0;

            for (int i = 0; i < clipboardCells.Length; i++)
            {
                int colLen = clipboardCells[i].Length;
                int colIdx = minColIdx + colLen - 1;
                maxColIdx = Math.Max(maxColIdx, colIdx);
            }

            if (maxColIdx > lastColIdx)
            {
                return true;
            }

            return false;
        }

        internal void Paste(DataGridView selectedDataGridView, DataGridViewSelectedCellCollection selectedCells, CellIndex selectedCellIndex, string[][] clipboardCells)
        {
            int minRowIdx = selectedCellIndex.minRowIdx;
            int minColIdx = selectedCellIndex.minColIdx;
            int clipBoardLastRowIdx = minRowIdx + clipboardCells.Length - 1;
            int dataTableLastRowIdx = selectedDataGridView.Rows.Count - 1;

            if (clipBoardLastRowIdx > dataTableLastRowIdx - 1)
            {
                int rowsAddCount = clipBoardLastRowIdx - dataTableLastRowIdx + 1;
                DataTable dataTable = (DataTable)selectedDataGridView.DataSource;

                for (int i = 0; i < rowsAddCount; i++)
                {
                    dataTable.Rows.Add();
                }

            }

            bool isOneCell = (clipboardCells.Length == 1 && clipboardCells[0].Length == 1);
            if (isOneCell)
            {
                string trimValue = clipboardCells[0][0].Trim();
                foreach (DataGridViewCell selectedCell in selectedCells)
                {
                    selectedCell.Value = trimValue;
                }
            }
            else
            {
                selectedDataGridView.ClearSelection();
                for (int i = 0; i < clipboardCells.Length; i++)
                {
                    for (int j = 0; j < clipboardCells[i].Length; j++)
                    {
                        string value = clipboardCells[i][j];
                        int rowIdx = minRowIdx + i;
                        int colIdx = minColIdx + j;

                        DataGridViewCell cell = selectedDataGridView[colIdx, rowIdx];
                        cell.Value = value;
                        cell.Selected = true;
                    }
                }
            }
        }

        internal void Delete(DataGridViewSelectedCellCollection selectedCells)
        {
            foreach (DataGridViewCell cell in selectedCells)
            {
                cell.Value = CELL_EMPTY_VALUE;
            }
        }

        internal void SelectAllRows(DataGridView selectedDataGridView, DataGridViewSelectedCellCollection selectedCells)
        {
            int rowCnt = selectedDataGridView.Rows.Count;

            foreach (DataGridViewCell cell in selectedCells)
            {
                int colIdx = cell.ColumnIndex;
                for (int rowIdx = 0; rowIdx < rowCnt; rowIdx++)
                {
                    selectedDataGridView.Rows[rowIdx].Cells[colIdx].Selected = true;
                }
            }
        }

        internal void SelectAllColumns(DataGridView selectedDataGridView, DataGridViewSelectedCellCollection selectedCells)
        {
            int colCnt = selectedDataGridView.ColumnCount;

            foreach (DataGridViewCell cell in selectedCells)
            {
                int rowIdx = cell.RowIndex;
                for (int colIdx = 0; colIdx < colCnt; colIdx++)
                {
                    selectedDataGridView.Rows[rowIdx].Cells[colIdx].Selected = true;
                }
            }
        }

        internal void SetAutoMapping(DataGridView selectedDataGridView, CellIndex selectedCellIndex, string[] dataArr)
        {
            int minRowIdx = selectedCellIndex.minRowIdx;
            int maxRowIdx = selectedCellIndex.maxRowIdx;
            int minColIdx = selectedCellIndex.minColIdx;

            for (int rowIdx = minRowIdx; rowIdx <= maxRowIdx; rowIdx++)
            {
                for (int i = 0; i < dataArr.Length; i++)
                {
                    string value = dataArr[i];
                    int colIdx = minColIdx + i;
                    selectedDataGridView[colIdx, rowIdx].Value = value;
                }
            }
        }
    }
}