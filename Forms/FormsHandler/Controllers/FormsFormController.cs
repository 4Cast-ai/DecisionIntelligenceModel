using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Controllers;
using Infrastructure.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Enums;
using Infrastructure.Models;
using Model.Data;
using Model.Entities;
using System.Collections.Generic;


namespace FormsHandler.Controllers
{
    [Route("api/FormsHandlerApi/[controller]")]
    [ApiController]
    public class FormsFormController : GeneralControllerBase
    {
        [HttpGet("GetGenderList")]
        [SwaggerOperation(Summary = "", Description = "GetGenderList")]
        public async Task<List<Evaluated>> GetGenderList()
        {
            string url = $"General/GetGenderList";
            var result = await DBGate.GetAsync<List<Evaluated>>(url);
            return result;
        }

        [HttpGet("GetEvaluatorActivityData")]
        [SwaggerOperation(Summary = "", Description = "Get Evaluator Activity Data")]
        public async Task<List<EvaluatorActivityData>> GetEvaluatorActivityData(string evaluator_guid)
        {
            //string url = $"General/GetOrgObjActivitiesForFiller?evaluator_guid={evaluator_guid}";
            //var result = await DBGate.GetAsync<List<EvaluatorActivityData>>(url);
            var data = new List<EvaluatorActivityData>
            {
                new EvaluatorActivityData
                {
                   ActivityGuid = "1",
                   ActivityName = "פעילות 1",
                   EndDate = "09/02/2022",
                   EvaluatorFormTemplateList = new List<EvaluatorFormTemplate> 
                   {
                       new EvaluatorFormTemplate
                       {
                           FormTemplateGuid = "11" ,
                           FormTemplateName =" תבנית פעילות 11",
                           FormData = new List<FormsFormData>
                           {
                              new FormsFormData
                              {
                               FormGuid="111",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="11",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "bb",
                               EvaluatedName = "ברוך הרזה",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="112",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="11",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "cc",
                               EvaluatedName = "דוד המצחיק",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="113",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="11",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "dd",
                               EvaluatedName = "גדעון החנון",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                           }
                       },
                       new EvaluatorFormTemplate
                       {
                           FormTemplateGuid = "22" ,
                           FormTemplateName =" תבנית פעילות 22",
                           FormData = new List<FormsFormData>
                           {
                              new FormsFormData
                              {
                               FormGuid="221",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="22",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "bb",
                               EvaluatedName = "ברוך הרזה",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="222",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="22",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "cc",
                               EvaluatedName = "דוד המצחיק",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="223",
                               FormStatus = 1,
                               ActivityGuid = "1" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="22",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "dd",
                               EvaluatedName = "גדעון החנון",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                           }
                       },

                   },
                },
                 new EvaluatorActivityData
                {
                   ActivityGuid = "2",
                   ActivityName = "פעילות 2",
                   EndDate = "09/02/2022",
                   EvaluatorFormTemplateList = new List<EvaluatorFormTemplate>
                   {
                       new EvaluatorFormTemplate
                       {
                           FormTemplateGuid = "55" ,
                           FormTemplateName =" תבנית פעילות 55",
                           FormData = new List<FormsFormData>
                           {
                              new FormsFormData
                              {
                               FormGuid="444",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="55",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "bb",
                               EvaluatedName = "ברוך הרזה",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="445",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="55",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "cc",
                               EvaluatedName = "דוד המצחיק",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="446",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="55",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "dd",
                               EvaluatedName = "גדעון החנון",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                           }
                       },
                       new EvaluatorFormTemplate
                       {
                           FormTemplateGuid = "66" ,
                           FormTemplateName =" תבנית פעילות 66",
                           FormData = new List<FormsFormData>
                           {
                              new FormsFormData
                              {
                               FormGuid="771",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="66",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "bb",
                               EvaluatedName = "ברוך הרזה",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="772",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="66",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "cc",
                               EvaluatedName = "דוד המצחיק",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                              new FormsFormData
                              {
                               FormGuid="773",
                               FormStatus = 1,
                               ActivityGuid = "2" ,
                               ActivityName = "פעילות 1",
                               FormTemplateGuid ="66",
                               EvaluatorGuid ="aa",
                               EvaluatorName = "שמעון השמן",
                               EvaluatorType = 1,
                               EvaluatedGuid = "dd",
                               EvaluatedName = "גדעון החנון",
                               Evaluatedtype = 1,
                               CreationDate = "09.09.22",
                               UpdateDate = "",
                               LastUpdateGuid = ""
                              },
                           }
                       },

                   },
                },
            };

            return data;
        }
    }
}