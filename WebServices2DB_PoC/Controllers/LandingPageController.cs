using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices2DB_PoC.Models;
using WebServices2DB_PoC.Services;

namespace WebServices2DB_PoC.Controllers
{
    [Route("api/landingpage")]
    public class LandingPageController : Controller
    {
        private ILandingPageRepository _landingPageRepository;

        public LandingPageController(ILandingPageRepository landingPageRepository)
        {
            _landingPageRepository = landingPageRepository;
        }

        [HttpGet()]
        public IActionResult GetLandingPageSummaries()
        {
            // return Ok(new string[] { "Google", "Apple" });

            // DB
            var lpSummaries = _landingPageRepository.GetLandingPageSummaries();

            // Version 1
            //var results = new List<LandingPageSimple>();
            //foreach (var landingPageSummary in lpSummaries)
            //{
            //    results.Add(new LandingPageSimple
            //    {
            //        Id = landingPageSummary.Id,
            //        Name = landingPageSummary.Name,
            //        Url = landingPageSummary.Url,
            //        Description = landingPageSummary.Description
            //    });
            //}

            // Version 2
            var results = Mapper.Map<IEnumerable<LandingPageSimple>>(lpSummaries);
            return Ok(results);
        }

        [HttpGet("{summaryId}")]
        public IActionResult GetAccountSummary(int summaryId, bool includeDetails = false)
        {
            // return Ok("{UserId: myemail@apple.com, Password: password}");
            var landingPageSummary = _landingPageRepository.GetLandingPageSummary(summaryId, includeDetails);
            if (landingPageSummary == null)
            {
                return NotFound();
            }

            if (includeDetails)
            {
                // Version 1
                //var landingPageResult = new LandingPageSummaryDto()
                //{
                //    Id = landingPageSummary.Id,
                //    Name = landingPageSummary.Name,
                //    Url = landingPageSummary.Url,
                //    Description = landingPageSummary.Description
                //};

                //foreach (var landingPageDetailItem in landingPageSummary.LandingPageDetails)
                //{
                //    landingPageResult.LandingPageDetails.Add(new LandingPageDetailDto()
                //    {
                //        UserId = landingPageDetailItem.UserId,
                //        Password = landingPageDetailItem.Password,
                //        Description = landingPageDetailItem.Description
                //    });
                //}

                // Version 2
                var landingPageResult = Mapper.Map<LandingPageSummaryDto>(landingPageSummary);
                return Ok(landingPageResult);
            }

            // Version 1: Without Automapper
            //var landingPageSimple = new LandingPageSimple()
            //{
            //    Id = landingPageSummary.Id,
            //    Name = landingPageSummary.Name,
            //    Url = landingPageSummary.Url,
            //    Description = landingPageSummary.Description
            //};
            var landingPageSimple = Mapper.Map<LandingPageSimple>(landingPageSummary);
            return Ok(landingPageSimple);
        }

        // Version 2
        [HttpGet("{mainAccountId}/individualaccounts")]
        public IActionResult GetSubAccountsForMainAccount(int mainAccountId)
        {
            try
            {
                if (!_landingPageRepository.AccountExists(mainAccountId))
                {
                    return NotFound();
                }
                var individualAccounts = _landingPageRepository.GetLandingPageDetails(mainAccountId);

                // Version 1
                //var individualAccountsForSummaryAccount = new List<LandingPageDetailDto>();
                //foreach (var item in individualAccounts)
                //{
                //    individualAccountsForSummaryAccount.Add(new LandingPageDetailDto()
                //    {
                //        UserId = item.UserId,
                //        Description = item.Description,
                //        Password = item.Password
                //    });
                //}

                // Version 2
                var individualAccountsForSummaryAccount = Mapper.Map<IEnumerable<LandingPageDetailDto>>(individualAccounts);
                return Ok(individualAccountsForSummaryAccount);
            }
            catch (Exception ex)
            {
                // returning an status code... is not a good suggestion to a consumer
                return StatusCode(500, "A problem happened while handling your request.\n" + ex.Message);
            }
        }

        // Version 2
        [HttpGet("{mainAccountId}/individualaccounts/{subAccountId}", Name = "GetSingleSubAccountForMainAccount")]
        public IActionResult GetSingleSubAccountForMainAccount(int mainAccountId, int subAccountId)
        {
            if (!_landingPageRepository.AccountExists(mainAccountId))
            {
                return NotFound();
            }

            var subAccount = _landingPageRepository.GetLandingPageDetail(mainAccountId, subAccountId);
            if (subAccount == null)
            {
                return NotFound();
            }

            // Version 1
            //var landingPageDetailDto = new LandingPageDetailDto()
            //{
            //    UserId = subAccount.UserId,
            //    Password = subAccount.Password,
            //    Description = subAccount.Description
            //};

            // Version 2
            var landingPageDetailDto = Mapper.Map<LandingPageDetailDto>(subAccount);

            return Ok(landingPageDetailDto);
        }

        [HttpPost("{mainAccountId}/individualaccounts")]
        public IActionResult CreatePointOfInterest(int mainAccountId,
                [FromBody] LandingPageDetailCreationDto landingPageDetail)
        {
            // Validation of PointOfInterest
            if (landingPageDetail == null)
            {
                return BadRequest();
            }
            if (landingPageDetail.UserId == landingPageDetail.Password)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_landingPageRepository.AccountExists(mainAccountId))
            {
                return NotFound();
            }

            var newIndividualAccount = Mapper.Map<Entities.LandingPageDetail>(landingPageDetail);

            _landingPageRepository.AddIndividualAccountForSummaryAccount(mainAccountId, newIndividualAccount);
            if (!_landingPageRepository.Save())
            {
                return StatusCode(500, "A problem happened while handlig your request.");
            }

            var createdIndividualAccount = Mapper.Map<Models.LandingPageDetailDto>(newIndividualAccount);

            return CreatedAtRoute("GetSingleSubAccountForMainAccount", new { mainAccountId = mainAccountId, subAccountId = createdIndividualAccount.Id }, createdIndividualAccount);
        }

        // This updates the entire object
        // For 'partial' updates... use a HttpPatch() attribute
        [HttpPut("{mainAccountId}/individualaccounts/{subAccountId}")]
        public IActionResult UpdateIndividualAccount(int mainAccountId, int subAccountId,
            [FromBody] LandingPageDetailCreationDto landingPageDetail)
        {
            // Validation of PointOfInterest
            if (landingPageDetail == null)
            {
                return BadRequest();
            }
            if (landingPageDetail.UserId == landingPageDetail.Password)
            {
                ModelState.AddModelError("Description", "The provided name and password should be different.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_landingPageRepository.AccountExists(mainAccountId))
            {
                return NotFound();
            }

            var subAccountEntity = _landingPageRepository.GetLandingPageDetail(mainAccountId, subAccountId);
            if (subAccountEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(landingPageDetail, subAccountEntity);

            if (!_landingPageRepository.Save())
            {
                return StatusCode(500, "A problem happened while handlig your request.");
            }

            return NoContent();
        }

        [HttpPatch("{mainAccountId}/individualaccounts/{subAccountId}")]
        public IActionResult PartialUpdatePointOfInterest(int mainAccountId, int subAccountId,
                    [FromBody] JsonPatchDocument<LandingPageDetailCreationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_landingPageRepository.AccountExists(mainAccountId))
            {
                return NotFound();
            }

            var subAccountEntity = _landingPageRepository.GetLandingPageDetail(mainAccountId, subAccountId);
            if (subAccountEntity == null)
            {
                return NotFound();
            }

            // Get the original first...
            var subAccountToPatch = Mapper.Map<LandingPageDetailCreationDto>(subAccountEntity);
            // Apply the patch
            // The 'ModelState' parameter validates the object... based on attributes
            // and returns the state, if fails
            // The 'patchDoc' identifies...and patches (??) automatically!
            patchDoc.ApplyTo(subAccountToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Complex validation can go here
            if (subAccountToPatch.UserId == subAccountToPatch.Password)
            {
                ModelState.AddModelError("UserId", "The provided UserId & Password should be different.");
            }

            TryValidateModel(subAccountToPatch);
            // Do this again... to ensure the validity of the object
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(subAccountToPatch, subAccountEntity);
            if (!_landingPageRepository.Save())
            {
                return StatusCode(500, "A problem happened while handlig your request.");
            }
            return NoContent();
        }

        [HttpDelete("{mainAccountId}/individualaccounts/{subAccountId}")]
        public IActionResult DeleteIndividualAccount(int mainAccountId, int subAccountId)
        {
            if (!_landingPageRepository.AccountExists(mainAccountId))
            {
                return NotFound();
            }

            var subAccountEntity = _landingPageRepository.GetLandingPageDetail(mainAccountId, subAccountId);
            if (subAccountEntity == null)
            {
                return NotFound();
            }

            _landingPageRepository.DeleteIndividualAccountForSummaryAccount(subAccountEntity);
            if (!_landingPageRepository.Save())
            {
                return StatusCode(500, "A problem happened while handlig your request.");
            }

            return NoContent();
        }

    }
}
