using NinetyNine.Auto;
using NinetyNine.BigTable;
using NinetyNine.Data;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string FORM_TITLE = "Multi Navi Cons (평택-골조)";
        private readonly string SAVEFILE_NAME = "빅테이블";
        private readonly string FILE_OPEN_TITLE = "열기 ({0})";
        private readonly string FILE_OPEN_TITLE_ALL_SHEETS = "All Sheets";
        private readonly string FILE_OPEN_TITLE_SELECTED_SHEET = "Selected Sheet";
        private readonly string FILE_OPEN_COMPLETE_MESSAGE = "열기 완료";
        private readonly string FILE_SAVE_COMPLETE_MESSAGE = "저장 완료";
        private readonly string BIGTABLE_CHECK_TITLE = "빅테이블";
        private readonly string BIGTABLE_CHECK_PARSING = "Parsing";
        private readonly string BIGTABLE_CHECK_MAPPING = "Mapping";
        private readonly string BIGTABLE_COMPLETE_MESSAGE = "{0} 완료";
        private readonly string BIGTABLE_ERROR_TAB_IDX_NOT_FOUND = "main tab idx not found";
        private readonly string CELLS_NOT_SELECTED = "CELL 선택이 필요합니다.";
        private readonly string CELLS_NOT_ONE_SELECTED = "CELL 1개 선택만 필요합니다.";
        private readonly string CLIPBOARD_CONTENT_NULL = "CLIPBOARD_CONTENT_NULL";
        private readonly string CLIPBOARD_CONTENT_EMPTY = "CLIPBOARD_CONTENT_EMPTY";
        private readonly string CLIPBOARD_CONTENT_NOT_ONE_CELL = "CELL 한개만 가능합니다.";
        private readonly string EDIT_UNDO_NULL = "취소할 내용이 없습니다.";
        private readonly string EDIT_UNDO_COMPLETE = "취소 완료";

        private readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private TabControlManager tabControlManager;
        private ExcelEPPlusManager excelEPPlusManager = new ExcelEPPlusManager();
        private BigTableManager bigTableManager = new BigTableManager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
            SetTabControl();
            SetFileDialog();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            tabControl.Width = Width - 35;
            tabControl.Height = Height - 85;

            if (tabControlManager != null)
            {
                tabControlManager.Resize();
            }
        }


        private void SetForm()
        {
            Text = FORM_TITLE;
            WindowState = FormWindowState.Maximized;
        }

        private void SetTabControl()
        {
            tabControlManager = new TabControlManager(tabControl);
            tabControlManager.Resize();

            DataSet dataSet = MainDataTableEnum.GetDataSetTemplate();
            tabControlManager.Refresh(dataSet);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControlManager.SelectedIndexChanged();
        }

        private void SetFileDialog()
        {
            openFileDialog.InitialDirectory = DESKTOP_PATH;
            openFileDialog.Filter =
                "엑셀 파일 (*.xlsx)|*.xlsx";

            saveFileDialog.InitialDirectory = DESKTOP_PATH;
            saveFileDialog.Filter =
                "엑셀 파일 (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = SAVEFILE_NAME;
        }

        private void SetWaitState()
        {
            Application.UseWaitCursor = true;
            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }
        }

        private void SetDefaultState()
        {
            Application.UseWaitCursor = false;
            foreach (Control control in Controls)
            {
                control.Enabled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
                    selectedDataGridView.ClearSelection();
                    break;
                case (Keys.Shift | Keys.Space):
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            return true;
        }

        private async void allSheetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = string.Format(FILE_OPEN_TITLE, FILE_OPEN_TITLE_ALL_SHEETS);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = openFileDialog.FileName;
                    DataSet dataSet = await excelEPPlusManager.GetDataSet(fileName);

                    tabControlManager.Check(dataSet);
                    tabControlManager.Refresh(dataSet);
                    tabControl.SelectedIndex = 0;

                    MessageBox.Show(FILE_OPEN_COMPLETE_MESSAGE);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private async void selectedSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = string.Format(FILE_OPEN_TITLE, FILE_OPEN_TITLE_SELECTED_SHEET);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = openFileDialog.FileName;
                    ExcelWorksheets workSheets = await excelEPPlusManager.GetExcelWorksheets(fileName);

                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                    ToolStripItemCollection items = contextMenu.Items;

                    foreach (ExcelWorksheet workSheet in workSheets)
                    {
                        string sheetName = workSheet.Name;
                        items.Add(sheetName);

                        int lastIdx = items.Count - 1;
                        ToolStripItem item = items[lastIdx];
                        item.Tag = workSheet;
                        item.Click += SelectedSheetItem_Click;
                    }

                    Point point = PointToScreen(tabControl.Location);
                    contextMenu.Show(point);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private async void SelectedSheetItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetWaitState();
                ToolStripItem item = sender as ToolStripItem;
                ExcelWorksheet workSheet = (ExcelWorksheet)item.Tag;
                await tabControlManager.RefreshSelectedDataTable(workSheet);
                MessageBox.Show(FILE_OPEN_COMPLETE_MESSAGE);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                SetDefaultState();
            }
        }

        private async void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = saveFileDialog.FileName;
                    DataSet dataSet = tabControlManager.GetDataSet();
                    string result = await excelEPPlusManager.Save(fileName, dataSet);
                    MessageBox.Show(FILE_SAVE_COMPLETE_MESSAGE);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private void BigTable_Parsing_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckBigTable(BIGTABLE_CHECK_PARSING, BigTableManagerState.Parsing);
        }

        private void BigTable_Mapping_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckBigTable(BIGTABLE_CHECK_MAPPING, BigTableManagerState.Mapping);
        }

        private void CheckBigTable(string message, BigTableManagerState state)
        {
            DialogResult dialogResult = MessageBox.Show(message, BIGTABLE_CHECK_TITLE, MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                RefreshBigTable(state);
            }
        }

        private async void RefreshBigTable(BigTableManagerState state)
        {
            try
            {
                SetWaitState();
                DataSet dataSet = tabControlManager.GetDataSet();
                await bigTableManager.Refresh(dataSet, state);

                tabControl.SelectedTab = tabPage_BigTable;
                string completeMessage = string.Format(BIGTABLE_COMPLETE_MESSAGE, state.ToString());
                MessageBox.Show(completeMessage);
            }
            catch (BigTableError bigTableError)
            {
                DataTable dataTable = bigTableError.GetDataTable();
                string tableName = dataTable.TableName;
                BigTableErrorCell[] cells = bigTableError.GetCells();
                string message = bigTableError.GetMessage();

                Array values = Enum.GetValues(typeof(MainDataTable));
                int tabIdx = EnumManager.GetIndex(values, tableName);
                if (tabIdx == -1)
                {
                    MessageBox.Show(BIGTABLE_ERROR_TAB_IDX_NOT_FOUND);
                    return;
                }

                tabControl.SelectedIndex = tabIdx;
                tabControlManager.HighLight(tabIdx, cells);
                MessageBox.Show(message);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                SetDefaultState();
            }
        }

        private void 내역서맵핑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            DataSet dataSet = tabControlManager.GetDataSet();
            AutoMappingForm autoMappingForm = new AutoMappingForm(dataSet);
            DialogResult dialogResult = autoMappingForm.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            //SetEditUndo
            SetEditUndo();

            string[] dataArr = autoMappingForm.GetDataArr();
            CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);

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

        private void 복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            DataObject dataObj = selectedDataGridView.GetClipboardContent();

            if (dataObj == null)
            {
                MessageBox.Show(CLIPBOARD_CONTENT_NULL);
                return;
            }

            Clipboard.SetDataObject(dataObj);
        }

        private void 붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            string text = Clipboard.GetText();
            if (text == null)
            {
                MessageBox.Show(CLIPBOARD_CONTENT_NULL);
                return;
            }

            if (text == "")
            {
                MessageBox.Show(CLIPBOARD_CONTENT_EMPTY);
                return;
            }

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

            //SetEditUndo
            SetEditUndo();

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
                CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);
                int minRowIdx = selectedCellIndex.minRowIdx;
                int minColIdx = selectedCellIndex.minColIdx;

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

        private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            //SetEditUndo
            SetEditUndo();

            foreach (DataGridViewCell cell in selectedCells)
            {
                cell.Value = "";
            }
        }

        private void 전체열선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            if (selectedCells.Count != 1)
            {
                MessageBox.Show(CELLS_NOT_ONE_SELECTED);
                return;
            }

            CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);
            int colIdx = selectedCellIndex.minColIdx;
            int rowCnt = selectedDataGridView.Rows.Count;

            for (int rowIdx = 0; rowIdx < rowCnt; rowIdx++)
            {
                selectedDataGridView.Rows[rowIdx].Cells[colIdx].Selected = true;
            }
        }

        private void 전체행선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;

            if (selectedCells == null || selectedCells.Count == 0)
            {
                MessageBox.Show(CELLS_NOT_SELECTED);
                return;
            }

            if (selectedCells.Count != 1)
            {
                MessageBox.Show(CELLS_NOT_ONE_SELECTED);
                return;
            }

            CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);
            int rowIdx = selectedCellIndex.minRowIdx;
            int colCnt = selectedDataGridView.ColumnCount;

            for (int colIdx = 0; colIdx < colCnt; colIdx++)
            {
                selectedDataGridView.Rows[rowIdx].Cells[colIdx].Selected = true;
            }
        }

        private void 실행취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditUndo editUndo = EditUndoManager.Instance.Get();
            if (editUndo == null)
            {
                MessageBox.Show(EDIT_UNDO_NULL);
                return;
            }

            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataTable originDataTable = (DataTable)selectedDataGridView.DataSource;
            DataTable backupDataTable = editUndo.dataTable;

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
            MessageBox.Show(EDIT_UNDO_COMPLETE);
        }

        private void SetEditUndo()
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataTable originDataTable = (DataTable)selectedDataGridView.DataSource;
            DataTable copyDataTable = originDataTable.Copy();

            EditUndo editUndo = new EditUndo();
            editUndo.dataTable = copyDataTable;

            EditUndoManager.Instance.Set(editUndo);
        }
    }
}