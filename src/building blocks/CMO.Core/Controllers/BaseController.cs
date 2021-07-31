﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CMO.Core.Controllers

{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected List<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null, bool isNotFound = false)
        {
            if (ValidateOperation())
            {
                return Ok(result);
            }

            var errosDetail = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            });

            if (isNotFound)
            {
                return NotFound(errosDetail);
            }
            else
            {
                return BadRequest(errosDetail);
            }

        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddProcessingError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool ValidateOperation()
        {
            return !Errors.Any();
        }

        protected void AddProcessingError(string error)
        {
            Errors.Add(error);
        }
    }
}