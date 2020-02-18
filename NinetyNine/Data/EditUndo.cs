using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Data
{
    class EditUndo
    {
        internal DataTable dataTable { get; set; }
        internal List<CellPosition> cellPositions { get; set; }
    }
}