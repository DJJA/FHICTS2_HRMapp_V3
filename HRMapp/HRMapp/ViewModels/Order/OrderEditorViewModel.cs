﻿using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    //public class OrderEditorViewModel : EditorViewModel
    //{
    //    public override string FormTitle
    //    {
    //        get
    //        {
    //            switch (EditorType)
    //            {
    //                case EditorType.New:
    //                    return "Nieuwe order toevoegen";
    //                case EditorType.Edit:
    //                    return "Order bewerken";
    //                default:
    //                    throw new ArgumentOutOfRangeException();
    //            }
    //        }
    //    }

    //    [DisplayName("Naam:")]
    //    public string Name { get; set; }
    //    [DisplayName("Omschrijving:")]
    //    public string Description { get; set; }

    //    /// <summary>
    //    /// Used for giving back a viewmodel and used for new skillset
    //    /// </summary>
    //    public OrderEditorViewModel()
    //    {
    //        EditorType = EditorType.New;
    //    }

    //    /// <summary>
    //    /// Used for editing a skillset
    //    /// </summary>
    //    /// <param name="order"></param>
    //    public OrderEditorViewModel(Order order)
    //    {
    //        EditorType = EditorType.Edit;

    //        Id = order.Id;
    //        Name = order.Name;
    //        Description = order.Description;
    //    }

    //    public Order ToOrder()
    //    {
    //        return new Order(Id, Name, Description);
    //    }
    //}
}
