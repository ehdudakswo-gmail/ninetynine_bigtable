using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NinetyNine.Auto
{
    public partial class AutoMappingForm : Form
    {
        private class DataType
        {
            internal string text { get; }
            internal List<string> dataList { get; }

            internal DataType(string text, List<string> dataList)
            {
                this.text = text;
                this.dataList = dataList;
            }
        }

        private DataSet dataSet;
        private List<DataType> dataTypeList = new List<DataType>();
        private DataType selectedDataType;
        private string selectedData;

        public AutoMappingForm(DataSet dataSet)
        {
            InitializeComponent();
            this.dataSet = dataSet;
        }

        private void AutoMappingForm_Load(object sender, EventArgs e)
        {
            SetConfig();
            SetDataTypeList();
            SetDataTypeComboBox();
        }

        private void AutoMappingForm_Resize(object sender, EventArgs e)
        {
            int resizeWidth = Width - 40;

            comboBox_DataType.Width = resizeWidth;
            textBox_Search.Width = resizeWidth;
            listBox_DataList.Width = resizeWidth;
            button_Enter.Width = resizeWidth;
        }

        private void SetConfig()
        {
            MinimizeBox = false;
            MaximizeBox = false;

            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(1000, Height);
        }

        private void SetDataTypeList()
        {
            DataTable statementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Statement);
            DataTable scheduleTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Schedule);

            BigTableDictionary statementBigTableDictionary = new BigTableDictionaryStatement(statementTable, new DataTableTemplateStatement());
            BigTableDictionarySchedule scheduleBigTableDictionary = new BigTableDictionaryScheduleWeek(scheduleTable, new DataTableTemplateSchedule());

            Dictionary<string, DataRow> statementDictionary = statementBigTableDictionary.Create();
            Dictionary<string, DataRow> scheduleDictionary = scheduleBigTableDictionary.Create();

            List<string> statementDataList = CreateDataList(statementDictionary);
            List<string> scheduleWorkDataList = CreateDataList(scheduleDictionary, 1);
            List<string> scheduleFloorDataList = CreateDataList(scheduleDictionary, 0);

            dataTypeList.Add(new DataType("Work - 내역서", statementDataList));
            dataTypeList.Add(new DataType("Work - 공정표", scheduleWorkDataList));
            dataTypeList.Add(new DataType("Floor - 공정표", scheduleFloorDataList));
        }

        private List<string> CreateDataList(Dictionary<string, DataRow> dictionary)
        {
            List<string> dataList = new List<string>();

            foreach (string key in dictionary.Keys)
            {
                dataList.Add(key);
            }

            return dataList;
        }

        private List<string> CreateDataList(Dictionary<string, DataRow> dictionary, int idx)
        {
            HashSet<string> set = new HashSet<string>();

            foreach (string key in dictionary.Keys)
            {
                string[] dataArr = BigTableDictionaryStatement.GetKeyArr(key);
                string selectedData = dataArr[idx];
                string[] selectedDataArr = new string[] { selectedData };
                string selectedDataKey = BigTableDictionaryStatement.GetKey(selectedDataArr);
                set.Add(selectedDataKey);
            }

            List<string> dataList = new List<string>(set);
            return dataList;
        }

        private void SetDataTypeComboBox()
        {
            foreach (DataType dataType in dataTypeList)
            {
                string text = dataType.text;
                comboBox_DataType.Items.Add(text);
            }

            comboBox_DataType.SelectedIndex = 0;
        }

        private void comboBox_DataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIdx = comboBox_DataType.SelectedIndex;
            selectedDataType = dataTypeList[selectedIdx];

            Clear();
            SetDataListItems();
        }

        private void Clear()
        {
            textBox_Search.Clear();
            listBox_DataList.Items.Clear();
        }

        private void SetDataListItems()
        {
            foreach (string data in selectedDataType.dataList)
            {
                listBox_DataList.Items.Add(data);
            }
        }

        private void comboBox_DataType_Enter(object sender, EventArgs e)
        {
            comboBox_DataType.DroppedDown = true;
        }

        private void comboBox_DataType_Click(object sender, EventArgs e)
        {
            comboBox_DataType.DroppedDown = true;
        }

        private void comboBox_DataType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBox_Search.Focus();
        }

        private void textBox_Search_TextChanged(object sender, EventArgs e)
        {
            string text = textBox_Search.Text;
            var items = listBox_DataList.Items;
            items.Clear();

            foreach (string data in selectedDataType.dataList)
            {
                if (data.Contains(text))
                {
                    items.Add(data);
                }
            }
        }

        private void button_Enter_Click(object sender, EventArgs e)
        {
            OK();
        }

        private void listBox_DataList_DoubleClick(object sender, EventArgs e)
        {
            OK();
        }

        private void OK()
        {
            selectedData = listBox_DataList.Text;
            if (selectedData == null || selectedData == "")
            {
                MessageBox.Show("데이터를 선택해주세요.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        internal string[] GetDataArr()
        {
            string[] data = BigTableDictionary.GetKeyArr(selectedData);

            return data;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);

            switch (key)
            {
                case Keys.Enter:
                    if (comboBox_DataType.DroppedDown)
                    {
                        return false;
                    }
                    else
                    {
                        OK();
                    }
                    break;
                case Keys.Escape:
                    Close();
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            return true;
        }
    }
}
