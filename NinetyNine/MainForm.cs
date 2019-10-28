using NinetyNine.BigTable;
using System;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly string FILE_OPEN_COMPLETE_MESSAGE = "열기 완료";
        private readonly string FILE_SAVE_COMPLETE_MESSAGE = "저장 완료";

        private readonly string BIGTABLE_COMPLETE_MESSAGE = "빅테이블 {0} 완료";
        private readonly string BIGTABLE_ERROR_TAB_IDX_NOT_FOUND = "main tab idx not found";

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

        private async void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
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
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    SetDefaultState();
                }
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
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private void BigTable1_Parsing_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBigTable(BigTable1_Parsing_ToolStripMenuItem);
        }

        private void BigTable2_Mapping_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBigTable(BigTable2_Mapping_ToolStripMenuItem);
        }

        private async void SetBigTable(ToolStripMenuItem menuItem)
        {
            try
            {
                SetWaitState();
                BigTableManagerState state = GetBigTableManagerState(menuItem);
                DataSet dataSet = tabControlManager.GetDataSet();
                string bigTableResult = await bigTableManager.Refresh(state, dataSet);

                tabControl.SelectedTab = tabPage_BigTable;
                string menuText = menuItem.Text;
                string completeMessage = string.Format(BIGTABLE_COMPLETE_MESSAGE, menuText);
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
                MessageBox.Show(exception.Message);
            }
            finally
            {
                SetDefaultState();
            }
        }

        private BigTableManagerState GetBigTableManagerState(ToolStripMenuItem menuItem)
        {
            if (menuItem.Equals(BigTable1_Parsing_ToolStripMenuItem))
            {
                return BigTableManagerState.Parsing;
            }
            else if (menuItem.Equals(BigTable2_Mapping_ToolStripMenuItem))
            {
                return BigTableManagerState.Mapping;
            }
            else
            {
                return BigTableManagerState.Unknown;
            }
        }
    }
}