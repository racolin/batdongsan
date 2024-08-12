using Application.ImagePages.Queries;
using Application.Sections.Queries;
using Application.Sliders.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class ContentController : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            var home = await Mediator.Send(new GetImagePageByPositionQuery((int)ImagePagePositionEnum.HomeScreen));
            var bgHome = await Mediator.Send(new GetImagePageByPositionQuery((int)ImagePagePositionEnum.BackgroundHomeScreen));
            var news = await Mediator.Send(new GetImagePageByPositionQuery((int)ImagePagePositionEnum.NewsScreen));
            var contact = await Mediator.Send(new GetImagePageByPositionQuery((int)ImagePagePositionEnum.ContactScreen));
            
            var slider = await Mediator.Send(new GetSliderByPositionQuery((int)SliderPositionEnum.ProjectScreen));
            var introduce = await Mediator.Send(new GetSectionByPositionQuery((int)SectionPositionEnum.IntroduceInHome));
            var newsMarket = await Mediator.Send(new GetSectionByPositionQuery((int)SectionPositionEnum.DescriptionNewsMarket));
            var newsProject = await Mediator.Send(new GetSectionByPositionQuery((int)SectionPositionEnum.DescriptionNewsProject));

            ViewBag.Home = home;
            ViewBag.BgHome = bgHome;
            ViewBag.News = news;
            ViewBag.Contact = contact;
            ViewBag.Slider = slider;
            ViewBag.Introduce = introduce;
            ViewBag.NewsMarket = newsMarket;
            ViewBag.NewsProject = newsProject;

            return View();
        }
    }
}
