using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using data_doc_api.Models;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System;

namespace data_doc_api.Controllers
{
    /// <summary>
    /// Provides services to manage calculations within a project
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for CalculationController class
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public CalculationController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all calculations for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>The list of calculations for the selected project</returns>
        [HttpGet("/Calculations/{projectId}")]
        public ActionResult<IEnumerable<CalculationInfo>> GetCalculations(int projectId)
        {
            return Ok(
                MetadataRepository.GetCalculations(projectId));
        }

        /// <summary>
        /// Gets a list of all calculations for an entity in a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns>The list of calculations for the selected project and entity</returns>
        [HttpGet("/Calculations/{projectId}/{entityName}")]
        public ActionResult<IEnumerable<CalculationInfo>> GetCalculations(int projectId, string entityName)
        {
            return Ok(
                MetadataRepository.GetCalculations(projectId)
                .Where(c => c.EntityName.Equals(entityName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gets a single calculation by id
        /// </summary>
        /// <param name="calculationId">The calculation id</param>
        /// <returns>The selected relationship</returns>
        [HttpGet("/Calculations/Single/{calculationId}")]
        public ActionResult<RelationshipInfo> GetSingle(int calculationId)
        {
            return Ok(MetadataRepository.GetCalculation(calculationId));
        }

        /// <summary>
        /// Creates a new calculation
        /// </summary>
        /// <param name="calculation">The calculation to create</param>
        /// <returns>The new relationship</returns>
        [HttpPost("/Calculations")]
        public ActionResult<CalculationInfo> CreateCalculation(CalculationInfo calculation)
        {
            return Ok(MetadataRepository.CreateCalculation(calculation));
        }

        /// <summary>
        /// Updates an existing calculation
        /// </summary>
        /// <param name="calculationId">The calculation id</param>
        /// <param name="calculation">The updated calculation</param>
        /// <returns>The updated relationship</returns>
        [HttpPut("/Calculations/{calculationId}")]
        public ActionResult<CalculationInfo> UpdateCalculation(int calculationId, [FromBody] CalculationInfo calculation)
        {
            if (calculation.CalculationId != calculationId)
            {
                throw new Exception("Invalid CalculationId");
            }
            MetadataRepository.DeleteCalculation(calculationId);
            return Ok(MetadataRepository.CreateCalculation(calculation));
        }

        /// <summary>
        /// Deletes an existing calculation
        /// </summary>
        /// <param name="calculationId">The calculation id</param>
        /// <returns>No content</returns>
        [HttpDelete("/Calculations/{calculationId}")]
        public ActionResult DeleteCalculation(int calculationId)
        {
            MetadataRepository.DeleteCalculation(calculationId);
            return NoContent();
        }
    }
}
