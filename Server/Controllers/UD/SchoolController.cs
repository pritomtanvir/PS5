using DOOR.EF.Data;
using DOOR.EF.Models;
using DOOR.Server.Controllers.Common;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : BaseController
    {
        public SchoolController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)
        {
        }

        [HttpGet]
        [Route("GetSchool")]
        public async Task<IActionResult> GetSchool()
        {
            List<SchoolDTO> lst = await _context.Schools
                .Select(sp => new SchoolDTO
                {
                    SchoolId = sp.SchoolId,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    SchoolName = sp.SchoolName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,



                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetSchool/{_SchoolId}")]
        public async Task<IActionResult> GetSchool(int _SchoolId)
        {
            SchoolDTO? lst = await _context.Schools
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new SchoolDTO
                {
                    SchoolId = sp.SchoolId,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    SchoolName = sp.SchoolName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,



                }).FirstOrDefaultAsync();
            return Ok(lst);



        }

        [HttpPost]
        [Route("PostSchool")]
        public async Task<IActionResult> PostSchool([FromBody] School _SchoolDTO)
        {
            try
            {
                School? sch = await _context.Schools.Where(x => x.SchoolId == _SchoolDTO.SchoolId).FirstOrDefaultAsync();

                if (sch == null)
                {
                    sch = new School
                    {
                        SchoolName = _SchoolDTO.SchoolName,
                        ModifiedDate = _SchoolDTO.ModifiedDate,
                        ModifiedBy = _SchoolDTO.ModifiedBy,
                        SchoolId = _SchoolDTO.SchoolId,
                        CreatedDate = _SchoolDTO.CreatedDate,
                        CreatedBy = _SchoolDTO.CreatedBy,
                        Students = _SchoolDTO.Students,


                    };
                    _context.Schools.Add(sch);
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
        [Route("PutSchool")]
        public async Task<IActionResult> PutSchool([FromBody] SchoolDTO _SchoolDTO)
        {
            try
            {
                School? sch = await _context.Schools.Where(x => x.SchoolId == _SchoolDTO.SchoolId).FirstOrDefaultAsync();

                if (sch != null)
                {
                    sch.SchoolName = _SchoolDTO.SchoolName;
                    sch.ModifiedDate = _SchoolDTO.ModifiedDate;
                    sch.ModifiedBy = _SchoolDTO.ModifiedBy;
                    sch.SchoolId = _SchoolDTO.SchoolId;
                    sch.CreatedDate = _SchoolDTO.CreatedDate;
                    sch.CreatedBy = _SchoolDTO.CreatedBy;


                    _context.Schools.Update(sch);
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
        [Route("DeleteSchool/{_}")]
        public async Task<IActionResult> DeleteSchool(int _SchoolId)
        {
            try
            {
                School? sch = await _context.Schools.Where(x => x.SchoolId == _SchoolId).FirstOrDefaultAsync();

                if (sch != null)
                {
                    _context.Schools.Remove(sch);
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