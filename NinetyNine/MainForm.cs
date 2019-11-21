using NinetyNine.BigTable;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string FORM_TITLE = "Muti Navi Cons";
        private readonly string SAVEFILE_NAME = "빅테이블";
        private readonly string FILE_OPEN_COMPLETE_MESSAGE = "열기 완료";
        private readonly string FILE_SAVE_COMPLETE_MESSAGE = "저장 완료";
        private readonly string BIGTABLE_COMPLETE_MESSAGE = "{0} 완료";
        private readonly string BIGTABLE_ERROR_TAB_IDX_NOT_FOUND = "main tab idx not found";

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

        private async void allSheetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            RefreshBigTable(BigTableManagerState.Parsing);
        }

        private void BigTable_Mapping_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshBigTable(BigTableManagerState.Mapping);
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
    }
}