using Microsoft.AspNetCore.Mvc;
using Services.DTO.AddRequests;
using Services.Abstractions;
using Servicos.Exceptions;
using AutoMenu.Models.Extensions;
using Services.Helpers;

namespace AutoMenu.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IRestaurantService _restaurantService;
        private readonly IConfiguration _configuration;

        public AccountController(IAddressService addressService, IRestaurantService restaurantService, IConfiguration configuration)
        {
            _addressService = addressService;
            _restaurantService = restaurantService;
            _configuration = configuration;
        }
        public async Task<IActionResult> Account()
        {
            // metodo IActionResult para a pagina index principal
            ViewBag.Css = "Account.css";
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            ViewBag.Cnpjs = restaurants.Select(restaurants => restaurants.Cnpj);
            return View();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([FromForm] RestaurantAddRequest restaurantAddRequest, [FromForm] AddressAddRequest addressAddRequest)
        {
            if (!ModelState.IsValid && _configuration["environmentVariables:ASPNETCORE_ENVIRONMENT"] == "Development")
            {
                return BadRequest(ModelState); //Custom error page pra depois
            }
            else if (!ModelState.IsValid)
                return BadRequest(string.Join(',', ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage)));


            var fk_address = await _addressService.AddAddressAsync(addressAddRequest);
            restaurantAddRequest.FkAddressId = fk_address.AddressID;
            try
            {
                await _restaurantService.AddRestaurantAsync(restaurantAddRequest);
            }
            catch (ExistingRestaurantException)
            {
                await _addressService.RemoveAddressByIDAsync(fk_address.AddressID);
                return BadRequest($"Um restaurante com o CNPJ {restaurantAddRequest.CNPJ} j� est� registrado!");
            }
            return RedirectToActionPermanent("", "Account");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromForm] string CNPJ, [FromForm] string password)
        {
            var restaurant = await _restaurantService.GetRestaurantByCNPJAsync(CNPJ);
            if (restaurant == null && password == null) return BadRequest("Senha ou CNPJ Invalido!");

            if (!PasswordHasher.VerifyPassword(password, restaurant.PasswordHash))
            {
                return BadRequest("Senha ou CNPJ Invalido!");
            }

            HttpContext.Session.SetObject("User", restaurant);

            return RedirectToActionPermanent("", "Interface");
        }
    }
}
