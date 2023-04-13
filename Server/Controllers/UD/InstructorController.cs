using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using DOOR.Server.Controllers.Common;

namespace CSBA6.Server.Controllers.app
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : BaseController
    {
        public InstructorController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetInstructor")]
        public async Task<IActionResult> GetInstructor()
        {
            List<InstructorDTO> lst = await _context.Instructors
                .Select(sp => new InstructorDTO
                {
                     CreatedBy = sp.CreatedBy,
                     FirstName = sp.FirstName,
                     CreatedDate = DateTime.Now,
                     InstructorId = sp.InstructorId,
                     LastName = sp.LastName,
                     ModifiedBy = sp.ModifiedBy,
                     ModifiedDate = DateTime.Now,
                     Phone = sp.Phone,
                     Salutation = sp.Salutation,
                     SchoolId = sp.SchoolId,
                     StreetAddress = sp.StreetAddress,
                     Zip = sp.Zip
                    
                    
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetInstructor/{_InstructorId}/{_SchoolId}")]
        public async Task<IActionResult> GetInstructor(int _InstructorId, int _SchoolId)
        {
            InstructorDTO? lst = await _context.Instructors
                .Where(x => x.InstructorId == _InstructorId)
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new InstructorDTO
                {
                     CreatedBy = sp.CreatedBy,
                     FirstName = sp.FirstName,
                     CreatedDate = DateTime.Now,
                     InstructorId = sp.InstructorId,
                     LastName = sp.LastName,
                     ModifiedBy = sp.ModifiedBy,
                     ModifiedDate = DateTime.Now,
                     Phone = sp.Phone,
                     Salutation = sp.Salutation,
                     SchoolId = sp.SchoolId,
                     StreetAddress = sp.StreetAddress,
                     Zip = sp.Zip
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostInstructor")]
        public async Task<IActionResult> PostInstructor([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                Instructor? i = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (i == null)
                {
                    i = new Instructor
                    {
                         ModifiedDate = DateTime.Now,
                         Phone = _InstructorDTO.Phone,
                         Salutation = _InstructorDTO.Salutation,
                         CreatedBy = _InstructorDTO.CreatedBy,
                         CreatedDate = _InstructorDTO.CreatedDate,
                         FirstName = _InstructorDTO.FirstName,
                         InstructorId = _InstructorDTO.InstructorId,
                         LastName = _InstructorDTO.LastName,
                         ModifiedBy = _InstructorDTO.ModifiedBy,
                         SchoolId = _InstructorDTO.SchoolId,
                         StreetAddress = _InstructorDTO.StreetAddress,
                         Zip = _InstructorDTO.Zip
 

                    };
                    _context.Instructors.Add(i);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }








        [HttpPut]
        [Route("PutInstructor")]
        public async Task<IActionResult> PutCourse([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                Instructor? i = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (i != null)
                {
                    i.CreatedBy = _InstructorDTO.CreatedBy;
                    i.CreatedDate = _InstructorDTO.CreatedDate;
                    i.FirstName = _InstructorDTO.FirstName;
                    i.InstructorId = _InstructorDTO.InstructorId;
                    i.LastName = _InstructorDTO.LastName;
                    i.ModifiedBy = _InstructorDTO.ModifiedBy;
                    i.ModifiedDate = _InstructorDTO.ModifiedDate;
                    i.Phone = _InstructorDTO.Phone;
                    i.Salutation = _InstructorDTO.Salutation;
                    i.SchoolId = _InstructorDTO.SchoolId;
                    i.StreetAddress = _InstructorDTO.StreetAddress;
                    i.Zip = _InstructorDTO.Zip;

                    _context.Instructors.Update(i);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("DeleteInstructor/{_InstructorDTO}/{_SchoolId}")]
        public async Task<IActionResult> DeleteInstructor(int _InstructorDTO, int _SchoolId)
        {
            try
            {
                Instructor? i = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO).FirstOrDefaultAsync();

                if (i != null)
                {
                    _context.Instructors.Remove(i);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }



    }
}