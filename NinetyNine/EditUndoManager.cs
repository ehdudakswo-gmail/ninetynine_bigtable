using NinetyNine.Data;

namespace NinetyNine
{
    class EditUndoManager
    {
        private static EditUndoManager instance = null;
        private EditUndo editUndo;

        private EditUndoManager()
        {
        }

        internal static EditUndoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EditUndoManager();
                }
                return instance;
            }
        }

        internal void Set(EditUndo editUndo)
        {
            this.editUndo = editUndo;
        }

        internal EditUndo Get()
        {
            EditUndo result = editUndo;
            editUndo = null;

            return result;
        }
    }
}