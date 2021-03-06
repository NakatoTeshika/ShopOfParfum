using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rgz.Models;
using Microsoft.EntityFrameworkCore;
using rgz.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace rgz.Models
{


    [Authorize]
    public class AdminController : Controller
    {
        private IRepository repository;
        public AdminController(IRepository rep)
        {
            repository = rep;

        }
        public IActionResult Index()
        {
            return RedirectToAction("Orders");
        }
        public ViewResult Orders()
        {
            return View(repository.GetClientAndGoods());
        }
        public ViewResult Goods()
        {
            return View(repository);
        }


        [HttpGet]
        public ViewResult AddGood()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGood(AddGoodModel mdl)
        {
            if (ModelState.IsValid)
            {
                repository.AddGood(
                    new Good
                    {
                        Description = mdl.Description,
                        Name = mdl.Name,
                        Price = mdl.Price,
                        //Street = mdl.Street,
                        StreetNumber = mdl.StreetNumber,
                        Brand = mdl.Brand,
                        ImgPath = mdl.ImgPath

                    }
                );
                return Redirect("/Admin/Goods");
            }
            return View();
        }


        public ViewResult Edit(int goodId)
        {
            AddGoodModel mdl = new AddGoodModel();
            var good = repository.Goods.FirstOrDefault(w => w.GoodId == goodId);
            mdl.Description = good.Description;
            mdl.Name = good.Name;
            mdl.Price = good.Price;
            mdl.Street = good.Adress;
            mdl.StreetNumber = good.StreetNumber;
            mdl.ImgPath = good.ImgPath;
            mdl.Brand = good.Brand;
            mdl.id = good.GoodId;

            return View(mdl);
        }

        public IActionResult EditGood(AddGoodModel addGood)
        {
            if (ModelState.IsValid)
            {

                var r = repository.Goods.FirstOrDefault(w => w.GoodId == addGood.id);
                r.Description = addGood.Description;
                r.Name = addGood.Name;
                r.Price = addGood.Price;
                r.Adress = addGood.Street;
                r.StreetNumber = addGood.StreetNumber;
                r.Brand = addGood.Brand;
                if (addGood.ImgPath != null)
                {
                    r.ImgPath = "/img/" + addGood.ImgPath;
                }
                repository.SaveChanges();
                return Redirect("/Admin/Goods");
            }
            return View("Checkout/Failure");
        }
        public IActionResult Delete(int goodId)
        {
            repository.DeleteGood(goodId);

            return RedirectToAction("Goods");
        }
    }
}