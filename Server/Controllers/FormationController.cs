using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Mediwatch.Shared.Models;
using Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Fichier avec les fonctions relatif aux controleurs des Formation.
/// </summary>
namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormationController : ControllerBase
    {
        #region //Config Part
        private readonly DbContextMediwatch _context;
        private IConfiguration _configuration;

        public FormationController(DbContextMediwatch context,
        IConfiguration configuration = null)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region //GET Part
        //GET: /Formation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<formation>>> GetFormation(
        [FromQuery(Name = "idtag")] String strIdTag = ""){
            /// <summary>
            /// Obtenez toutes les formations
            /// </summary>
            /// <returns>Retourner la liste des formations disponibles</returns>
            
            //Une partie obtient la formation à partir de IdTag
            if (strIdTag != "")
            {
                TagController _tagController = new TagController(_context);
                List<JoinFormationTag> Join = JoinTagFormationController
                .GetJoinTagForm(_context, "", "", strIdTag)
                .Result
                .Value
                .ToList();
                List<formation> result = new List<formation>();

                foreach (var item in Join)
                {
                    result.Add(GetFormation(item.idFormation)
                    .Result
                    .Value);
                }
                return result;
            }

            return await _context.formations.ToListAsync();
        }

        // //GET: /Formation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<formation>> GetFormation(Guid id)
        {
            /// <summary>
            /// Obtenez une formation spécifique par est ID
            /// </summary>
            /// <returns>Retourne les informations de formation</returns>
            formation formationResult = await _context.formations.FindAsync(id);
            if (formationResult == null)
                return null;
            return formationResult;
        }
        #endregion

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<formation>>> search(string name,
        DateTime? startDate, 
        DateTime? endDate,
        string location,
        string contact,
        string organizationName,
        float? price,
        string former)
        {
            try
            {
                IQueryable<formation> query = _context.formations;
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => e.Name.Contains(name));
                }
                if (startDate != null)
                {
                    query = query.Where(e => e.StartDate == startDate);
                }
                if (endDate != null)
                {
                    query = query.Where(e => e.StartDate == endDate);
                }
                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(e => e.Location.Contains(location));
                }
                if (!string.IsNullOrEmpty(contact))
                {
                    query = query.Where(e => e.Contact.Contains(contact));
                }
                if (!string.IsNullOrEmpty(organizationName))
                {
                    query = query.Where(e => e.OrganizationName.Contains(organizationName));
                }
                if (price != null)
                {
                    query = query.Where(e => e.Price == (decimal)price);
                }
                if (!string.IsNullOrEmpty(former))
                {
                    query = query.Where(e => e.Former.Contains(former));
                }
                var resultSearch = await query.ToListAsync();
                if (resultSearch.Any())
                    return Ok(resultSearch);
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            } 
        }


        #region //PUT POST DELETE
        //Put: /Formation/5
        [Authorize(Roles = "Admin,Tutor")]
        [HttpPut("{id}")]
        public async Task<ActionResult<formation>> PutFormation(Guid id, formation formationPut)
        {
            /// <summary>
            /// Mettre à jour la formation spécifique par son ID
            /// </summary>
            formationPut.id = id;
            if (!FormationExists(id))
            {
                return NotFound();
            }
            _context.Entry(formationPut).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return StatusCode(200);
        }

        // POST: /Formation
        [HttpPost]
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<ActionResult<formation>> PostFormation(formation formationBody)
        {
            /// <summary>
            /// Créer une nouvelle formation
            /// </summary>
            _context.formations.Add(formationBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormation), new { id = formationBody.id }, formationBody);
        }

        // DELETE: Formation/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<ActionResult<formation>> DeleteFormation(Guid id)
        {
            /// <summary>
            /// Supprimer la formation par ID
            /// </summary>
            /// <returns></returns>
            var formationResult = await _context.formations.FindAsync(id);
            if (formationResult == null)
            {
                return NotFound();
            }

            _context.formations.Remove(formationResult);
            await _context.SaveChangesAsync();

            return formationResult;
        }
        
        private bool FormationExists(Guid id) {
            return _context.formations.Any(e => e.id == id);
        }
        #endregion


        [HttpGet("testclient")]
        public ActionResult<formation> testclient()
        {
            /// <summary>
            /// Créer une nouvelle formation
            /// </summary>

            return new formation() { ArticleID = _configuration["Authentication:PayPal:SandboxClientId"] };
        }
    }
}
