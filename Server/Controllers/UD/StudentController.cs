using System;
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
using static Duende.IdentityServer.Models.IdentityResources;

namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]

    public class StudentController : BaseController
    {
        public StudentController(DOOROracleContext _DBcontext,
    OraTransMsgs _OraTransMsgs)
    : base(_DBcontext, _OraTransMsgs)

        {
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {
            List<StudentDTO> lst = await _context.Students
                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Phone = sp.Phone,
                    RegistrationDate = sp.RegistrationDate,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip
                }).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetStudent/{_SchoolID}/{_StudentID}")]
        public async Task<IActionResult> GetStudent(int _StudentID,int _SchoolID)
        {
            StudentDTO? lst = await _context.Students
                .Where(x => x.StudentId == _StudentID)
                .Where(x => x.SchoolId == _SchoolID)
                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Phone = sp.Phone,
                    RegistrationDate = sp.RegistrationDate,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("PostStudent")]
        public async Task<IActionResult> PostStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student? c = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Student
                    {

                        CreatedBy = _StudentDTO.CreatedBy,
                        CreatedDate = _StudentDTO.CreatedDate,
                        Employer = _StudentDTO.Employer,
                        FirstName = _StudentDTO.FirstName,
                        LastName = _StudentDTO.LastName,
                        ModifiedBy = _StudentDTO.ModifiedBy,
                        ModifiedDate = _StudentDTO.ModifiedDate,
                        Phone = _StudentDTO.Phone,
                        RegistrationDate = _StudentDTO.RegistrationDate,
                        Salutation = _StudentDTO.Salutation,
                        SchoolId = _StudentDTO.SchoolId,
                        StreetAddress = _StudentDTO.StreetAddress,
                        StudentId = _StudentDTO.StudentId,
                        Zip = _StudentDTO.Zip

                    };
                    _context.Students.Add(c);
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
        [Route("PutStudent")]
        public async Task<IActionResult> PutStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student? c = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (c != null)
                {
                    c.CreatedBy = _StudentDTO.CreatedBy;
                    c.CreatedDate = _StudentDTO.CreatedDate;
                    c.Employer = _StudentDTO.Employer;
                    c.FirstName = _StudentDTO.FirstName;
                    c.LastName = _StudentDTO.LastName;
                    c.ModifiedBy = _StudentDTO.ModifiedBy;
                    c.ModifiedDate = _StudentDTO.ModifiedDate;
                    c.Phone = _StudentDTO.Phone;
                    c.RegistrationDate = _StudentDTO.RegistrationDate;
                    c.Salutation = _StudentDTO.Salutation;
                    c.SchoolId = _StudentDTO.SchoolId;
                    c.StreetAddress = _StudentDTO.StreetAddress;
                    c.StudentId = _StudentDTO.StudentId;
                    c.Zip = _StudentDTO.Zip;



                    _context.Students.Update(c);
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
        [Route("DeleteStudent/{_StudentID}/{_SchoolId}")]
        public async Task<IActionResult> DeleteCourse(int _StudentID, int _SchoolId)
        {
            try
            {
                Student? c = await _context.Students.Where(x => x.StudentId == _StudentID).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.Students.Remove(c);
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