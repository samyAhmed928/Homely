using Homely_modified_api.Data;
using Homely_modified_api.Dtos;
using Homely_modified_api.Models;
using Homely_modified_api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Homely_modified_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly HomelyDbcontext _homelyDBContext;
        private readonly IImageUploadService _imageUploadService;

        public PropertyController(HomelyDbcontext homelyDBContext, IImageUploadService imageUploadService)
        {
            _homelyDBContext = homelyDBContext;
            _imageUploadService = imageUploadService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromForm] PropertyDto propertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = await _homelyDBContext.Clients.FirstOrDefaultAsync(x => x.Id == propertyDto.clientId);
            if (client.NumberOfAdds == 0)
            {
                return NotFound("Upgrade your plan");
            }
            var property = new Property
            {
                Id = Guid.NewGuid(),
                Name = propertyDto.Name,
                Price = propertyDto.Price,
                Type = propertyDto.Type,
                Rent_period=propertyDto.Rent_period,
                Contract_type = propertyDto.Contract_type,
                NumberOfRooms = propertyDto.NumberOfRooms,
                NumberOfBaths = propertyDto.NumberOfBaths,
                NumberOfKitchens = propertyDto.NumberOfKitchens,
                Area = propertyDto.Area,
                BuildYear = propertyDto.BuildYear,
                Description = propertyDto.Description,
                city = propertyDto.city,
                Address = propertyDto.Address,
                featuresoftheproperty = propertyDto.featuresoftheproperty,
                clientId = propertyDto.clientId,
                client = client,
                Main_image = await _imageUploadService.Upload_single_image(propertyDto.main_image),
                ImagesUrls = new List<PropertyImage>()
                
            };

            property.client.NumberOfAdds--;
            //List<string> ImagesUrls = await _imageUploadService.Upload_images(propertyDto.Uploded_Images);


            //if (ImagesUrls != null)
            //{
            //    // Add each image URL from the DTO to the property's ImagesUrls collection
            //    foreach (var imageUrl in ImagesUrls)
            //    {
            //        var Prop_image = new PropertyImage
            //        {
            //            Id = new Guid(),
            //            PropertyId = property.Id,
            //            Property = property,
            //            Url = imageUrl
            //        };
            //        property.ImagesUrls.Add(Prop_image);
            //    }

            //}

            _homelyDBContext.Properties.Add(property);
            await _homelyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, property);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _homelyDBContext.Properties
                .Include(p => p.ImagesUrls)
                .ToListAsync();
            return Ok(properties);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetPropertyById")]
        public async Task<IActionResult> GetPropertyById([FromRoute] Guid id)
        {
            var property = await _homelyDBContext.Properties
                .Include(p => p.ImagesUrls)
                .Include(p => p.client) // Include the client navigation property
                .FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, PropertyDto propertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var property = await _homelyDBContext.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            property.Name = propertyDto.Name;
            property.Price = propertyDto.Price;
            // ... (update other properties as needed)

            _homelyDBContext.Properties.Update(property);
            await _homelyDBContext.SaveChangesAsync();

            return NoContent(); // No content needs to be returned after successful update
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _homelyDBContext.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            _homelyDBContext.Properties.Remove(property);
            await _homelyDBContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchProperties([FromBody] seaech_request searchRequest)
        {
            var query = _homelyDBContext.Properties
                .Include(p => p.ImagesUrls)
                .Include(p => p.client)
                .AsQueryable();
            Console.WriteLine($"count : {query.Count()}");
            Console.WriteLine($"Price : {searchRequest.Price}");

            // Apply filters based on search request
            if (searchRequest.Price.HasValue)
            {
                Console.WriteLine("Entered");
                var priceRange = 0.1m * searchRequest.Price;
                var minPrice = searchRequest.Price - priceRange;
                var maxPrice = searchRequest.Price + priceRange;
                Console.WriteLine($"min price : {minPrice}");
                Console.WriteLine($"maxPrice : {maxPrice}");
                Console.WriteLine($"Price : {searchRequest.Price}");

                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Type))
            {
                query = query.Where(p => p.Type.Contains(searchRequest.Type));
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Contract_type))
            {
                query = query.Where(p => p.Contract_type.Contains(searchRequest.Contract_type));
            }

            if (searchRequest.Area.HasValue)
            {
                var areaRange = 0.1 * searchRequest.Area;
                var minArea = searchRequest.Area - areaRange;
                var maxArea = searchRequest.Area + areaRange;
                query = query.Where(p => p.Area >= minArea && p.Area <= maxArea);
            }


            if (searchRequest.BuildYear.HasValue)
            {
                var yearRange = 1;
                var minYear = searchRequest.BuildYear - yearRange;
                var maxYear = searchRequest.BuildYear + yearRange;
                query = query.Where(p => p.BuildYear >= minYear && p.BuildYear <= maxYear);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Key_word))
            {
                query = query.Where(p => p.Name.Contains(searchRequest.Key_word) || p.Description.Contains(searchRequest.Key_word));
            }

            var properties = await query.ToListAsync();

            if (properties.Count == 0)
            {
                return NotFound();
            }

            return Ok(properties);
        }


    }
}


