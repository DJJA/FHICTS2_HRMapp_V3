﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models.Exceptions;
using HRMapp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMapp.Controllers
{
    public class SkillsetController : Controller
    {
        private static CrossActionMessageHolder errorMessage = new CrossActionMessageHolder();
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private SkillsetLogic skillsetLogic = new SkillsetLogic();

        public IActionResult Index(int id)
        {
            var skillsets = skillsetLogic.GetAll().ToList();    // Where do I use a List and where an IEnumerable? Where do I convert?
            if (id == 0 && skillsets.Count > 0)                 // Default id, no parameter passed
            {
                id = skillsets[0].Id;
            }
            var model = new SkillsetCollectionViewModel(id, skillsets) { InfoMessage = infoMessage.Message };    
            return View("Skillset", model);
        }

        public IActionResult SkillsetView(int id)
        {
            var skillset = skillsetLogic.GetById(id);
            return PartialView("../Partial/_SkillsetView", skillset);
        }

        public IActionResult New()
        {
            return View("SkillsetEditor", new SkillsetEditorViewModel() { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult New(SkillsetEditorViewModel model)
        {
            try
            {
                var addedSkillsetId = skillsetLogic.Add(model.ToSkillset());
                infoMessage.Message = $"'{model.Title}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new {id = addedSkillsetId});
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("New");
            }
            catch (DBException dbEx)
            {
                errorMessage.Message = dbEx.Message;
                return RedirectToAction("New");
            }
        }

        public IActionResult Edit(int id)
        {
            var skillset = skillsetLogic.GetById(id);
            return View("SkillsetEditor", new SkillsetEditorViewModel(skillset) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult Edit(SkillsetEditorViewModel model)
        {
            try
            {
                var success = skillsetLogic.Update(model.ToSkillset());
                infoMessage.Message = $"'{model.Title}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("Edit", new { id = model.Id });
            }

        }
    }
}