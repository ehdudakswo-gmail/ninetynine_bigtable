using NinetyNine.Auto;
using NinetyNine.BigTable;
using NinetyNine.BigTable.Dictionary.Mapping;
using NinetyNine.Data;
using NinetyNine.Template.Mapping;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

//GitHub Test

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string FORM_TITLE = "BigTable (평택-골조)";
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
        private readonly string CELL_EMPTY_VALUE = "";
        private readonly string CELLS_NOT_SELECTED = "CELL 선택이 필요합니다.";
        private readonly string CLIPBOARD_CONTENT_NULL = "CLIPBOARD_CONTENT_NULL";
        private readonly string CLIPBOARD_CONTENT_EMPTY = "CLIPBOARD_CONTENT_EMPTY";
        private readonly string CLIPBOARD_CONTENT_COLUMN_SIZE_LIMIT = "열 범위를 초과했습니다.";
        private readonly string EDIT_UNDO_NULL = "취소할 내용이 없습니다.";
        private readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private TabControlManager tabControlManager;
        private ExcelEPPlusManager excelEPPlusManager = new ExcelEPPlusManager();
        private BigTableManager bigTableManager = new BigTableManager();
        private EditManager editManager = new EditManager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
            SetTabControl();
            SetFileDialog();
            SetDataGridViewEvent();
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

        private void SetDataGridViewEvent()
        {
            List<DataGridView> dataGridViews = tabControlManager.GetDataGridViews();
            foreach (DataGridView dataGridView in dataGridViews)
            {
                dataGridView.CellEndEdit += new DataGridViewCellEventHandler(dataGridView_CellEndEdit);
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            editManager.AddUndoData(dataGridView);
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

        private void 잘라내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedCells(true);
        }

        private void 복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedCells(false);
        }

        private void CopySelectedCells(bool isCut)
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

            if (isCut)
            {
                editManager.AddUndoData(selectedDataGridView);
                foreach (DataGridViewCell cell in selectedCells)
                {
                    cell.Value = CELL_EMPTY_VALUE;
                }
            }
        }

        private void 붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;
            CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);

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

            if (text == CELL_EMPTY_VALUE)
            {
                MessageBox.Show(CLIPBOARD_CONTENT_EMPTY);
                return;
            }

            string[][] clipboardCells = editManager.GetClipBoardCells(text);
            bool isColumnSizeLimit = editManager.IsColumnSizeLimit(selectedDataGridView, selectedCellIndex, clipboardCells);

            if (isColumnSizeLimit)
            {
                MessageBox.Show(CLIPBOARD_CONTENT_COLUMN_SIZE_LIMIT);
                return;
            }

            editManager.AddUndoData(selectedDataGridView);
            editManager.Paste(selectedDataGridView, selectedCells, selectedCellIndex, clipboardCells);
            tabControlManager.RefreshRowHeaderValue();
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

            editManager.AddUndoData(selectedDataGridView);
            editManager.Delete(selectedCells);
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

            editManager.SelectAllRows(selectedDataGridView, selectedCells);
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

            editManager.SelectAllColumns(selectedDataGridView, selectedCells);
        }

        private void 실행취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            EditUndo editUndo = EditUndoManager.Instance.Get();

            if (editUndo == null)
            {
                MessageBox.Show(EDIT_UNDO_NULL);
                return;
            }

            editManager.Undo(selectedDataGridView, editUndo);
            tabControlManager.RefreshRowHeaderValue();
        }

        private void 맵핑자동화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = tabControlManager.GetSelectedDataGridView();
            DataGridViewSelectedCellCollection selectedCells = selectedDataGridView.SelectedCells;
            CellIndex selectedCellIndex = tabControlManager.GetCellIndex(selectedCells);

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

            string[] dataArr = autoMappingForm.GetDataArr();
            editManager.AddUndoData(selectedDataGridView);
            editManager.SetAutoMapping(selectedDataGridView, selectedCellIndex, dataArr);
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage_Who)
            {
                SetWhoKeys();
            }
        }

        private void SetWhoKeys()
        {
            DataTable whoDataTable = (DataTable)dataGridView_Who.DataSource;
            DataTable howDataTable = (DataTable)dataGridView_How.DataSource;

            BigTableDictionaryHow howBigTableDictionary = new BigTableDictionaryHow(howDataTable, new DataTableTemplateHow());
            List<string> dataList = howBigTableDictionary.GetDataList(HowTitle.BigTable_WorkSmall);
            HashSet<string> set = new HashSet<string>(dataList);
            set.Remove(CELL_EMPTY_VALUE);
            List<string> keys = new List<string>(set);

            int contentRowIdx = 1;
            int colIdx = EnumManager.GetIndex(WhoTitle.BigTable_WorkSmall);

            editManager.RefreshRows(whoDataTable, keys, contentRowIdx, colIdx);
            tabControlManager.RefreshRowHeaderValue();
        }
    }
}