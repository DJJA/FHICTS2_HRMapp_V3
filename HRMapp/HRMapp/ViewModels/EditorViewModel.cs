using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public enum EditorType
    {
        New,
        Edit
    }

    public abstract class EditorViewModel
    {
        public EditorType EditorType { get; set; }
        public string ErrorMessage { get; set; }
        public virtual string FormAction
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "New";
                    case EditorType.Edit:
                        return "Edit";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public abstract string FormTitle { get; }

        public int Id { get; set; }
    }
}
