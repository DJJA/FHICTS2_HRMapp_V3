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
        public abstract string FormAction { get; }

        public abstract string FormTitle { get; }

        public int Id { get; set; }
    }
}
