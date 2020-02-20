using NinetyNine.Data;
using System;
using System.Collections.Generic;

namespace NinetyNine
{
    class EditUndoManager
    {
        private static EditUndoManager instance = null;
        private const int LIMIT_SIZE = 10;
        private List<EditUndo> list = new List<EditUndo>();

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

        internal void Init()
        {
            list = new List<EditUndo>();
        }

        internal void Add(EditUndo editUndo)
        {
            if (list.Count < LIMIT_SIZE)
            {
                list.Add(editUndo);
            }
            else
            {
                list.RemoveAt(0);
                list.Add(editUndo);
            }
        }

        internal EditUndo Get()
        {
            int lastIdx = list.Count - 1;
            if (lastIdx < 0)
            {
                return null;
            }

            EditUndo result = list[lastIdx];
            list.RemoveAt(lastIdx);

            return result;
        }
    }
}